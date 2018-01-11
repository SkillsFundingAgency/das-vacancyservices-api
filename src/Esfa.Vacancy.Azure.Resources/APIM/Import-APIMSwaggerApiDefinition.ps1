<#
.SYNOPSIS
Update an APIM API with a swagger definition

.PARAMETER ResourceGroupName
The name of the resource group that contains the APIM instnace

.PARAMETER InstanceName
The name of the APIM instnace

.PARAMETER ApiName
The name of the API to update

.PARAMETER SwaggerSpecificationName
The full path to the swagger defintion

For example:
https://sit-manage-vacancy.apprenticeships.sfa.bis.gov.uk/swagger/docs/v1

#>
[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [String]$ResourceGroupName,
    [Parameter(Mandatory=$true)]
    [String]$InstanceName,
    [Parameter(Mandatory=$true)]
    [String]$ApiName,
    [Parameter(Mandatory=$true)]
    [String]$SwaggerSpecificationUrl
)

try {
    # --- Build context and retrieve apiid
    Write-Host "Building APIM context for $ResourceGroupName\$InstanceName"
    $Context = New-AzureRmApiManagementContext -ResourceGroupName $ResourceGroupName -ServiceName $InstanceName
    Write-Host "Retrieving ApiId for API $ApiName"
    $ApiId = (Get-AzureRmApiManagementApi -Context $Context -Name $ApiName).ApiId

    # --- Throw if ApiId is null
    if (!$ApiId) {
        throw "Could not retrieve ApiId for API $ApiName"
    }

    # --- Import swagger definition
    Write-Host "Updating API $ApiId\$InstanceName from definition $SwaggerSpecficiationUrl"
    Import-AzureRmApiManagementApi -Context $Context -SpecificationFormat "Swagger" -SpecificationUrl $SwaggerSpecificationUrl -ApiId $ApiId -ErrorAction Stop -Verbose:$VerbosePreference
} catch {
   throw $_
}