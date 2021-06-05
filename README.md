# Solasta Mod Template

A barebones template to create Mods in Solasta.

	- Leverages [ModMaker](https://github.com/cabarius/WrathModMaker):
		- Thanks Hsinyu Chan for developing it for Pathfinder: Kingmaker
		- Thanks Narnia for improving it for Pathfinder: WoTR

# How to create a new Project from this Template

1. Install all required development pre-requisites:
	- [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/downloads/)
	- [.NET "Current" x86 SDK](https://dotnet.microsoft.com/download/visual-studio-sdks)
	- A correctly configured [GIT command line](https://git-scm.com/downloads):
		```
		git config --global user.name "Your name here"
		git config --global user.email "your_email@example.com"
		```
2. Manually create a new repository on your GitHub account
3. Open a PowerShell console and run [CREATE_SOLASTA_MOD.ps1](https://raw.githubusercontent.com/SolastaMods/SolastaModTemplate/main/CREATE_SOLASTA_MOD.ps1) at the base folder you would like your project folder to be created
	- Enter a Mod Name (must match repository name created on step 3)
	- Enter your GitHub User Name

# How to Compile

1. Download and install [Unity Mod Manager (UMM)](https://www.nexusmods.com/site/mods/21)
2. Execute UMM, Select Solasta, and Install
3. Download and install [SolastaModApi](https://www.nexusmods.com/solastacrownofthemagister/mods/48) using UMM
4. Create the environment variable *SolastaInstallDir* and point it to your Solasta game home folder
	- tip: search for "edit the system environment variables" on windows search bar
5. Use "Install Release" or "Install Debug" to have the Mod installed directly to your Game Mods folder

NOTE Unity Mod Manager and this mod template make use of [Harmony](https://go.microsoft.com/fwlink/?linkid=874338)

# How to publish (first time)

1. Create a new repo on GitHub on Browser UI
2. Run CREATE_SOLASTA_MOD.PS1 on my computer to get template and first commit to Repo
3. Develop / Test the Mod
4. Create new hidden Mod on Nexus page with minimum required entries. Get Nexus URL
5. Edit version entries on CSPROJ, Info.json, and Repository.json (I always start with 0.0.1)
6. Edit Info.json and fix Nexus URL
7. Release Mod on GitHub using Vx.y.z as TAG/RELEASE convention (I always start with V0.0.1)
8. Update Nexus page with download file and set mod to unhidden

# How to publish (update)

1. Develop / Test the Mod
2. Edit version entries on CSPROJ, Info.json, and Repository.json
3. Update DownloadURL on Repository.json
4. Release Mod on GitHub using Vx.y.z as TAG/RELEASE convention
5. Update Nexus page with new release

# How to Debug

1. Open Solasta game folder
	* Rename UnityPlayer.dll to UnityPlayer.dll.original
	* Rename WinPixEventRuntime.dll to WinPixEventRuntime.dll.original (if it exists)
	* Change / Add below entries to *Solasta_Data\boot.config* (ensure there are no dups):
		```
		wait-for-managed-debugger=1
		player-connection-debug=1
		```
2. Download and install [7zip](https://www.7-zip.org/a/7z1900-x64.exe)
3. Download [Unity Editor 2019.4.19](https://download.unity3d.com/download_unity/ca5b14067cec/Windows64EditorInstaller/UnitySetup64-2019.4.19f1.exe)
4. Open Downloads folder
	* Right-click UnitySetup64-2019.4.1f1.exe, 7Zip -> Extract Here
	* Navigate to Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win64_development_mono
		* Copy *UnityPlayer.dll* and *WinPixEventRuntime.dll* to clipboard
	* Navigate to the Solasta game folder
		* Rename *UnityPlayer.dll* to *UnityPlayer.dll.original*
		* Paste *UnityPlayer.dll* and *WinPixEventRuntime.dll* from clipboard
5. You can now attach the Unity Debugger from Visual Studio 2019, Debug -> Attach Unity Debug
