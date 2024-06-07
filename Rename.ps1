# Get the directory containing the script
$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Definition
$scriptName = $MyInvocation.MyCommand.Name

# Parameters
$rootPath = $scriptDirectory                    # Root directory to start the search
$findText = "UnBeatMod"                  # Text to find
$replaceText = "UnAbleMod" #Read-Host "Enter the replacement text"  # Prompt the user to enter the replacement text


# Function to replace text in file content
function Replace-TextInFile {
    param (
        [string]$filePath,
        [string]$findText,
        [string]$replaceText
    )

    (Get-Content -LiteralPath $filePath) -replace $findText, $replaceText | Set-Content -LiteralPath $filePath
}

# Function to rename files
function Rename-Files {
    param (
        [string]$rootPath,
        [string]$findText,
        [string]$replaceText,
        [string]$scriptName
    )

    Get-ChildItem -LiteralPath $rootPath -Recurse -File | ForEach-Object {
        $filePath = $_.FullName

        # Skip the script file itself
        if ($_.Name -eq $scriptName) {
            return
        }

        # Replace text in file content
        Replace-TextInFile -filePath $filePath -findText $findText -replaceText $replaceText

        # Rename file if necessary
        if ($_.Name.Contains($findText)) {
            $newName = $_.Name.Replace([regex]::Escape($findText), $replaceText)
            $newPath = Join-Path $_.DirectoryName -ChildPath $newName
            Rename-Item -LiteralPath $filePath -NewName $newPath
        }
    }
}

# Execute the function
Rename-Files -rootPath $rootPath -findText $findText -replaceText $replaceText -scriptName $scriptName

echo "Done!"
pause
exit
