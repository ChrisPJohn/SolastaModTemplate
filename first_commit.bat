setlocal EnableDelayedExpansion
set /p REPO_NAME="Enter a repo name: "
set /p DESCRIPTION="Enter a repo description: "
set /p PROJECT_PATH="what is the absolute path to your local project directory? "
set /p USERNAME="What is your github username? "
cd %PROJECT_PATH%
git init
git add .
git commit -m "initial commit -setup with bat script"
curl -u %USERNAME% https://api.github.com/user/repos -d "{\"name\": \"%REPO_NAME%\", \"description\": \"%DESCRIPTION%\"}"
git remote add origin https://github.com/%USERNAME%/%REPO_NAME%.git
git push --set-upstream origin master