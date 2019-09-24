[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$ErrorActionPreference = "Stop"

# *****************************************************************************
# Creates an environment-specific configuration file
# *****************************************************************************
function CreateConfigFile {
    param(
        [Parameter(Mandatory)]
        [string]$filePath,
        [Parameter(Mandatory, HelpMessage="Command runner URL for CLI")]
        [string]$apiEndpoint,
        [Parameter(Mandatory, HelpMessage="Token URL from Identity Provider")]
        [string]$tokenEndpoint,
        [Parameter(Mandatory, HelpMessage="Audience (API name)")]
        [string]$audience,
        [Parameter(Mandatory, HelpMessage="Authorization Audience (API scope)")]
        [string]$scope,
        [Parameter(Mandatory, HelpMessage="Client Id")]
        [string]$clientId,
        [Parameter(Mandatory, HelpMessage="Client Secret")]
        [SecureString]$clientSecret
    )

    $contents = @{
        apiEndpoint = $apiEndpoint
        tokenEndpoint = $tokenEndpoint
        audience = $audience
        scope = $scope
        client_id = $clientId
        client_secret = $clientSecret | ConvertFrom-SecureString
    } | ConvertTo-Json

    New-Item -path $filePath -type "file" -value $contents | Out-Null
}

# *****************************************************************************
# Authenticates with the OAuth2/OpenIdConnect server (Auth0). Also, a global
# token is set and used by this script for future calls to increase 
# performance.
# *****************************************************************************
function Login {
    try {
        $body = @{ 
            grant_type = "client_credentials"
            audience = $script:config.audience
            scope = $script:config.scope
            client_id = $script:config.client_id
            client_secret = GetPlainText($script:config.client_secret | ConvertTo-SecureString)
        } | ConvertTo-Json
        
        # Ignore assignment warning. This global var is used in a function later and across calls to this script
        $global:token = Invoke-WebRequest -Uri $script:config.tokenEndpoint -Method Post -Body $body -ContentType "application/json" `
            | ConvertFrom-Json | Select-Object -ExpandProperty "access_token"
    }
    catch {
        $host.UI.WriteErrorLine("Error executing command: $args")
        $host.UI.WriteErrorLine("Unable to login.")
    }
}

# *****************************************************************************
# Gets the plain-text value from a PowerShell SecureString
# *****************************************************************************
function GetPlainText {
	param(
		[parameter(Mandatory = $true)]
		[System.Security.SecureString]$SecureString
    )
    
	$bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureString);
 
    try {
        return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr);
    }
    catch {
        $host.UI.WriteErrorLine("Unable to retrieve SecureString.")
    }
    finally {
        [Runtime.InteropServices.Marshal]::FreeBSTR($bstr);
    }
}

# *****************************************************************************
# Wraps a command arg value in double quotes if it contains spaces or is
# escaped with a "\"
# *****************************************************************************
function WrapArgInQuotesIfNeeded {
    param(
		[parameter(Mandatory = $true)]
		[string]$_
    )

    if($_.ToString().Contains(" ") -or $_.ToString().Contains("\")) {
        return "`"$_`""
    }
    else {
        $_
    }
}

# *****************************************************************************
# Load the $script variables and return filtered $args 
# *****************************************************************************
function FilterArgsAndLoadScriptSettings {
    param(
        [Parameter(Mandatory,ValueFromPipeline)]
        [object[]]$args
    )
    Process {
        $newArgs = @()
        
        for ($i = 0; $i -lt $args.Count; $i++) {
            
            # Check for "env" arg
            if($args[$i] -eq "--env") {
                $nextArg = $args[$i + 1]
                if($nextArg.StartsWith("--")) {
                    throw [System.InvalidOperationException] "`"--env`" used with no environment specified."
                }
        
                $script:env = $nextArg
                $i++
            }
            # Check for "verbose" arg
            elseif($args[$i] -eq "--verbose" -or $args[$i] -eq "-v") {
                $script:verbose = $true
            }
            else {
                $newArgs = $newArgs += $args[$i]
            }
        }
        
        # set script-wide default "env" value
        if($null -eq $script:env) {
            if($null -eq $global:env) {
                $script:env = "local"
            }
            else {
                $script:env = $global:env
            }
        }

        # set script-wide default "verbose" value
        if($null -eq $script:verbose) {
            if($null -eq $global:verbose) {
                $script:verbose = $false
            }
            else {
                $script:verbose = $global:verbose
            }
        }
    
        return $newArgs
    }   
}

# *****************************************************************************
# Load the $script:config var
# *****************************************************************************
function LoadConfiguration {
    param(
        [Parameter(Mandatory)]
        [string]$scriptName
    )
    $scriptsDir = (get-item $PSScriptRoot).FullName
    $configFileName = "$scriptName.$script:env.config"
    $configFilePath = Join-Path -Path $scriptsDir -ChildPath $configFileName
    
    if (!(Test-Path $configFilePath)) {
        CreateConfigFile($configFilePath)
    }
    
    $script:config = Get-Content -Raw -Path $configFilePath | ConvertFrom-Json
}

# *****************************************************************************
# Sends the request to the API
# *****************************************************************************
function ExecuteCommand {
    param(
        [Parameter(Mandatory,ValueFromPipeline)]
        [object[]]$args
    )
    Process {
        $headers = @{ Authorization = "Bearer $global:token" }

        $body = $args | ForEach-Object -Process { `
            return WrapArgInQuotesIfNeeded($_)
        }

        if($script:verbose -eq $true) {
            Write-Host "Processing: $body"
        }

        $result = Invoke-WebRequest `
            -Uri $script:config.apiEndpoint `
            -Method Post `
            -Body $body `
            -Headers $headers `
            -ContentType "text/plain" `

        if($script:verbose -eq $true) {
            Write-Host "OK!"
        }

        return $result
    }
}

# *****************************************************************************
# Displays error message on the console
# *****************************************************************************
function DisplayHttpExceptionMessage {
    param(
        [Parameter(Mandatory)]
        [System.Net.WebException]$exception
    )

    $result = $exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($result)
    $reader.BaseStream.Position = 0
    $reader.DiscardBufferedData()
    $responseBody = $reader.ReadToEnd();
    
    $host.UI.WriteErrorLine("Error executing command: $args")
    $host.UI.WriteErrorLine($responseBody)
}

# *****************************************************************************
# Displays error message on the console
# *****************************************************************************
function DisplayGenericExceptionMessage {
    param(
        [Parameter(Mandatory)]
        [System.Net.Exception]$exception,
        [Parameter(Mandatory,ValueFromPipeline)]
        [object[]]$args
    )
    Process {
        $host.UI.WriteErrorLine("Error executing command: $args")
        $host.UI.WriteErrorLine($exception)
    }
}

# *****************************************************************************
# Main script execution
# *****************************************************************************
$args = FilterArgsAndLoadScriptSettings($args)
LoadConfiguration($MyInvocation.MyCommand.Name)

if($null -eq $global:token) {
    Login
}

try {
    $result = ExecuteCommand($args)
    return $result.Content
}
catch [System.Net.WebException] {
    # perform a single retry after attempting to reauthenticate
    if([System.Net.HttpStatusCode]::Unauthorized -eq $_.Exception.Response.StatusCode) {
        try {
            Login
            $result = ExecuteCommand($args)
            return $result.Content
        }
        catch [System.Net.WebException] {
            DisplayHttpExceptionMessage($_.Exception)
        }
    }
    else {
        DisplayHttpExceptionMessage($_.Exception)
    }
}
catch {
    DisplayGenericExceptionMessage($_.Exception)
}

return $result.Content