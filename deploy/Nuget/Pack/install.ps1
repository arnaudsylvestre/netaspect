param($installPath, $toolsPath, $package, $project)
    $targetsFile = [System.IO.Path]::Combine($toolsPath, 'NetAspect.targets')

    Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
    $msbuild = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1

    $projectUri = new-object Uri('file://' + $project.FullName)
    $targetUri = new-object Uri('file://' + $targetsFile)
    $relativePath = $projectUri.MakeRelativeUri($targetUri).ToString().Replace([System.IO.Path]::AltDirectorySeparatorChar, [System.IO.Path]::DirectorySeparatorChar)
	

	$im = $msbuild.Xml.CreateImportElement($relativePath)
	$im.Condition = "Exists('$relativePath')"
    $msbuild.Xml.InsertAfterChild($im,  ($msbuild.Xml.Imports | Select-Object -Last 1))
    
	$project.Save()