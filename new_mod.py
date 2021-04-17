# AUTHOR: Zappastuff
#
# INSTRUCTIONS: 
#   - Requires Python 3.x installed 
#   - Requires 'pip install requests'
#
import requests, zipfile, io, os, shutil, sys

# validate arguments
if len(sys.argv) != 2:
    print("USAGE: py new_mod.py <DESIRED_MOD_NAME>")
    exit(1)

# setup some ugly globals
DMN = sys.argv[1]
TEMPLATE = "SolastaModTemplate"
ZIPFOLDER = f"{TEMPLATE}-main"
FILES = [
    f"{ZIPFOLDER}/{TEMPLATE}.sln",
    f"{ZIPFOLDER}/README.md",
    f"{ZIPFOLDER}/{TEMPLATE}/{TEMPLATE}.csproj",
    f"{ZIPFOLDER}/{TEMPLATE}/Info.json",
    f"{ZIPFOLDER}/{TEMPLATE}/main.cs" 
]

# download and unzip the mod template
try:
    shutil.rmtree(ZIPFOLDER)
except:
    pass
response = requests.get(url = f"https://github.com/SolastaMods/{TEMPLATE}/archive/refs/heads/main.zip")
payload = response.content
zip = zipfile.ZipFile(io.BytesIO(payload))
zip.extractall()

# replace all occurences of SolastaModTemplate in all required files
for file in FILES:
    with open(file, "rt", encoding="utf-8") as file_handle:
        file_content = file_handle.read()
    file_content = file_content.replace(TEMPLATE, DMN)
    with open(file, "wt", encoding="utf-8") as file_handle:
        file_handle.write(file_content)

# rename required files and folders
os.rename(f"{ZIPFOLDER}/{TEMPLATE}/{TEMPLATE}.csproj", f"{ZIPFOLDER}/{TEMPLATE}/{DMN}.csproj")
os.rename(f"{ZIPFOLDER}/{TEMPLATE}.sln", f"{ZIPFOLDER}/{DMN}.sln")
os.rename(f"{ZIPFOLDER}/{TEMPLATE}", f"{ZIPFOLDER}/{DMN}")
try:
    shutil.rmtree(DMN)
except:
    pass
os.rename(f"{ZIPFOLDER}", f"{DMN}")