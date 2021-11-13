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
        [string]$apiEndpoint
    )

    $contents = @{
        apiEndpoint = $apiEndpoint
    } | ConvertTo-Json

    New-Item -path $filePath -type "file" -value $contents | Out-Null
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
            -ContentType "text/plain" `
            -TimeoutSec 1800 `
            -UseBasicParsing `

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

    $host.UI.WriteErrorLine("Error executing command: $args")

    if($null -eq $exception.Response) {
        $host.UI.WriteErrorLine("Unable to contact server.");
    }
    else {
        $result = $exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($result)
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd();
        $host.UI.WriteErrorLine($responseBody)
    }
}

# *****************************************************************************
# Displays error message on the console
# *****************************************************************************
function DisplayGenericExceptionMessage {
    param(
        [Parameter(Mandatory)]
        [System.Net.Exception]$exception
    )
    
    $host.UI.WriteErrorLine("Error executing command: $args")
    $host.UI.WriteErrorLine($exception)
}

# *****************************************************************************
# Main script execution
# *****************************************************************************
$args = FilterArgsAndLoadScriptSettings($args)
LoadConfiguration($MyInvocation.MyCommand.Name)

try {
    $result = ExecuteCommand($args)
    return $result.Content
}
catch [System.Net.WebException] {
    DisplayHttpExceptionMessage($_.Exception)
}
catch {
    DisplayGenericExceptionMessage($_.Exception)
}

return $result.Content
