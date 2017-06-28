param (
    # The path to the source directory. Default $Env:BUILD_SOURCESDIRECTORY is set by TFS.
    [Parameter(Mandatory=$false, Position=0)]
    [ValidateNotNullOrEmpty()]
    [string]$SourceDirectory = $Env:BUILD_SOURCESDIRECTORY,

    # The build number. Default $Env:BUILD_BUILDNUMBER is set by TFS and must be configured according your regex.
    [Parameter(Mandatory=$false, Position=1)]
    [ValidateNotNullOrEmpty()]
    [string]$BuildNumber = $Env:BUILD_BUILDNUMBER
)

function Update-AssemblyVersion {
  
  param ([string]$version)
  
  # TODO regex replace instead of expecting -rc
  $safe_version = $version -replace "-rc","";
  $new_version = 'AssemblyVersion("' + $safe_version + '")';
  $new_file_version = 'AssemblyFileVersion("' + $safe_version + '")';
  $new_info_version = 'AssemblyInformationalVersion("' + $version + '")';
  Write-output $new_info_version;
  
  foreach ($o in $input) {
    Write-output $o.FullName
    $tmp_file = $o.FullName + ".tmp"

     Get-Content $o.FullName -encoding utf8 |
        %{$_ -replace 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $new_version } |
        %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $new_file_version }  |
        %{$_ -replace 'AssemblyInformationalVersion\(".*"\)', $new_info_version }  |
        Set-Content $tmp_file -encoding utf8
    
    move-item $tmp_file $o.FullName -force
  }
}

Get-ChildItem $SourceDirectory -recurse "AssemblyInfo.cs" | Update-AssemblyVersion $BuildNumber
