$scriptsDir = (get-item $PSScriptRoot).FullName
$configFile = ($MyInvocation.MyCommand.Name) + ".config"

function CreateConfig {
    param(
        [Parameter(Mandatory=$true, HelpMessage="Command runner URL for CLI")]
        [string]$apiEndpoint,
        [Parameter(Mandatory=$true, HelpMessage="Token URL from Identity Provider")]
        [string]$tokenEndpoint,
        [Parameter(Mandatory=$true, HelpMessage="Audience (API name)")]
        [string]$audience,
        [Parameter(Mandatory=$true, HelpMessage="Authorization Audience (API scope)")]
        [string]$scope,
        [Parameter(Mandatory=$true, HelpMessage="Client Id")]
        [string]$clientId,
        [Parameter(Mandatory=$true, HelpMessage="Client Secret")]
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

    New-Item -path . -name $configFile -type "file" -value $contents | Out-Null
}

function Login {
    $body = @{ 
        grant_type = "client_credentials"
        audience = $script:config.audience
        scope = $script:config.scope
        client_id = $script:config.client_id
        client_secret = GetPlainText($script:config.client_secret | ConvertTo-SecureString)
    } | ConvertTo-Json
    
    $global:token = Invoke-WebRequest -Uri $script:config.tokenEndpoint -Method Post -Body $body -ContentType "application/json" `
        | ConvertFrom-Json | Select-Object -ExpandProperty "access_token"
}

function GetPlainText {
	param(
		[parameter(Mandatory = $true)]
		[System.Security.SecureString]$SecureString
    )
    
	$bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureString);
 
    try {
        return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr);
    }
    finally {
        [Runtime.InteropServices.Marshal]::FreeBSTR($bstr);
    }
}

if (!(Test-Path $configFile)) {
    CreateConfig
}

$script:config = Get-Content -Raw -Path $configFile | ConvertFrom-Json

if($global:token -eq $null) {
    Login
}

$headers = @{ Authorization = "Bearer $global:token" }
$body = $args | ForEach-Object -Process { if($_.ToString().Contains(" ")) { return "`"$_`"" } else { $_ }  }

$result = Invoke-WebRequest -Uri $script:config.apiEndpoint -Method Post -Body $body -Headers $headers -ContentType "text/plain" `

return $result.Content