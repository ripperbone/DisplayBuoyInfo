$TestResultsDir = "DisplayBuoyInfoTest\TestResults"
$CoverageOutputDir = "Reports"

if (Test-Path $TestResultsDir) {
	Write-Host "Cleaning up test results directory: $TestResultsDir"
	Remove-Item $TestResultsDir -Recurse
}

Invoke-Expression "dotnet test --collect:`"XPlat Code Coverage`""

$CoverageFile = dir -Path DisplayBuoyInfoTest -Filter coverage.cobertura.xml -Recurse | %{$_.FullName}


Invoke-Expression "$env:userprofile\.nuget\packages\reportgenerator\4.8.13\tools\net5.0\ReportGenerator.exe -reports:$CoverageFile -targetDir:$CoverageOutputDir"