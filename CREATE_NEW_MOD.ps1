# get user input
$ORG_NAME = "SolastaMods"
$TEMPLATE = "SolastaModTemplate"
$GIT_REPO = Read-Host -Prompt "Enter your GitHub repository name"
$GIT_USER = Read-Host -Prompt "Enter your GitHub username"

# confirm destructive action
$MESSAGE = "Existing $GIT_REPO folder will be deleted and recreated. CONTINUE"
while( -not (($choice=(Read-Host $MESSAGE)) -match "y|n")) {"Y or N ?"}
if($choice -match "n") 
{
    "INFO: Aborted"
    exit
}

# download and unzip Solasta Mod Template to New Mod Name
Invoke-RestMethod `
    -Uri "https://github.com/SolastaMods/$TEMPLATE/archive/refs/heads/main.zip" `
    -OutFile "$TEMPLATE.zip"
Remove-Item -Recurse -Force "$TEMPLATE-main" -ErrorAction Ignore
Expand-Archive -Path "$TEMPLATE.Zip" -DestinationPath .
Remove-Item -Recurse -Force "$GIT_REPO" -ErrorAction Ignore
Rename-Item "$TEMPLATE-main" "$GIT_REPO"

# replace all SolastaModTemplate with ModName on all files
$FILES = Get-ChildItem -Path ".\$GIT_REPO" -Recurse -File | select -ExpandProperty FullName
foreach($FILE in $FILES)
{
    (((Get-Content -path $FILE -Raw) `
        -replace $TEMPLATE, $GIT_REPO) `
        -replace $ORG_NAME, $GIT_USER) | Set-Content -Path $FILE
}

# rename / delete files
Remove-Item "$GIT_REPO\README.md"
Remove-Item "$GIT_REPO\CREATE_NEW_MOD.PS1"

Rename-Item "$GIT_REPO\$TEMPLATE\$TEMPLATE.csproj" "$GIT_REPO.csproj"
Rename-Item "$GIT_REPO\$TEMPLATE" "$GIT_REPO"
Rename-Item "$GIT_REPO\$TEMPLATE.sln" "$GIT_REPO.sln"
Rename-Item "$GIT_REPO\README-TEMPLATE.md" "README.md"

# first commit
cd "$GIT_REPO"
git init
git add .
git commit -m "initial commit by SolastaMod"

# first push
$MESSAGE = "Have you created your repo <$GIT_REPO> on GitHub"
while( -not (($choice=(Read-Host $MESSAGE)) -match "y|n")) { "Y or N ?"}
if($choice -match "n")
{
    "INFO: don't forget to create your repo and push your first commit with these commands"
    ""
    "git remote add origin 'https://github.com/$GIT_USER/$GIT_REPO.git'"
    "git push --set-upstream origin master"
    ""
    cd ..
    exit
}
git remote add origin "https://github.com/$GIT_USER/$GIT_REPO.git"
git push --set-upstream origin master
cd ..