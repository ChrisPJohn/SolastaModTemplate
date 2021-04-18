# get user input
$TEMPLATE = "SolastaModTemplate"
$MOD_NAME = Read-Host -Prompt "Enter your Mod/Repo name (no special characters): "
$MOD_DESC = Read-Host -Prompt "Enter an one line Mod description: "
$GIT_USER = Read-Host -Prompt "Enter your GitHub username: "

# confirm destructive action
while( -not ( ($choice= (Read-Host "Existing $MOD_NAME folder will be recreated. ARE YOU SURE?")) -match "y|n")){ "Y or N ?"}
if($choice.ToUpper() -eq "N") {exit}

# download and unzip Solasta Mod Template to New Mod Name
Invoke-RestMethod `
    -Uri "https://github.com/SolastaMods/$TEMPLATE/archive/refs/heads/main.zip" `
    -OutFile "$TEMPLATE.zip"
Remove-Item -Recurse -Force "$TEMPLATE-main" -ErrorAction Ignore
Expand-Archive -Path "$TEMPLATE.Zip" -DestinationPath .
Remove-Item -Recurse -Force "$MOD_NAME" -ErrorAction Ignore
Rename-Item "$TEMPLATE-main" "$MOD_NAME"

# replace all SolastaModTemplate with ModName on all files
$FILES = Get-ChildItem -Path ".\$MOD_NAME" -Recurse -File | select -ExpandProperty FullName
foreach($FILE in $FILES)
{
    ((Get-Content -path $FILE -Raw) -replace $TEMPLATE, $MOD_NAME) | Set-Content -Path $FILE
}

# rename necessary files
Rename-Item "$MOD_NAME\$TEMPLATE\$TEMPLATE.csproj" "$MOD_NAME.csproj"
Rename-Item "$MOD_NAME\$TEMPLATE" "$MOD_NAME"
Rename-Item "$MOD_NAME\$TEMPLATE.sln" "$MOD_NAME.sln"

# create repo on GitHub
# $CREDENTIAL = Get-Credential -UserName $GIT_USER -Message "Enter your GitHub credentials"
# $BODY = @{name="$MOD_NAME"; description="$MOD_DESCRIPTION"}
# Invoke-RestMethod -Uri "https://api.github.com/user/repos" -Method POST -Body $BODY
while( -not ( ($choice= (Read-Host "Manually create your Repo <$MOD_NAME> on GitHub.com. CAN I CONTINUE?")) -match "y|n")){ "Y or N ?"}
if($choice.ToUpper() -eq "N") {exit}

# first commit
cd "$MOD_NAME"
git init
git add .
git commit -m "initial commit by SolastaMod"
git remote add origin "https://github.com/$GIT_USER/$MOD_NAME.git"
git push --set-upstream origin master
cd ..