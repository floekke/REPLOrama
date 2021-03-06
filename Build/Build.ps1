Properties {
	$build_dir = Split-Path $psake.build_script_file
    $base_dir = (Resolve-Path "$build_dir\..").Path	
    $solution  = "$base_dir\REPLOrama.sln"
    $project = "$base_dir\REPLOrama\REPLOrama.csproj"
    $build_artifacts_dir = "$base_dir\BuildArtifacts"
    $nuget_exe = "$base_dir\packages\NuGet.CommandLine.3.3.0\tools\NuGet.exe"
}

Task Default -depends NugetPack

Task Clean {
    Set-Location $base_dir
    rm -Force *.nupkg
}

Task NugetRestore -depends Clean {
    Write-Host "Running nuget restore in $(Get-Location)."
    Set-Location $base_dir
    Exec { & $nuget_exe restore }
}

Task Build -depends NugetRestore {
   Write-Host "Building solution $solution"
   mkdir -Force $build_artifacts_dir
   Exec { msbuild $solution /p:OutDir=$build_artifacts_dir }
}

# TODO pack from build artifacts instead!
Task NugetPack -depends Build {
    Write-Host "Packing.."
    Exec { & $nuget_exe pack $project}
}

Task NugetPush -depends NugetPack {
    Exec {
     & $nuget_exe setApiKey "9149e089-a886-4bd7-99e3-bdcee45c6409"
     & $nuget_exe push REPLOrama.*.nupkg
    }
}