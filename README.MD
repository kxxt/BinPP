# BinPP

An utility to make C51 recognize binary literals such as `0b10010010`.

> Please read this README from beginning to end.  
> **IF YOU DO NOT UNDERSTAND WHAT YOU ARE DOING , PLEASE STOP!**

> THIS SOFTWARE IS PROVIDED "AS-IS" , WITHOUT ANY EXPRESSED OR IMPLIED WARRANTY .USE IT AT YOUR OWN RISK !  
## Usage

1. Open Keil uVision.
2. Get to your project.
3. Open the option window of your target.
4. Get to the `User` tab.
5. Edit it .(Follow the picture below)
> It's okey if your binpp.exe isn't stored under your project directory.

![image-20201003104754898](README.assets/image-20201003104754898.png)

- In the `Before Build/Rebuild` Tab add your source file name to the arguments of the binpp.exe
- Set the error code to <0
- Have Fun !

- If the Build fails , the source files won't be restored automatically.
  - You need to manually execute `binpp.exe restore YOUR_PROJECT_DIRECTORY` to restore your source files.
  - Pro Tip : ( There's a easier way. )
    1. Open Keil uVision. 
    2. Click `Tools` Menu and then click `Customize Tool Menu...`
    3. Insert a new menu item and name it to `Restore Files`. ( Or anything you want. )
    4. Set the command to the path of your `binpp` executable .
    5. Leave the `Initial Folder` Empty
    6. Click `OK` .
    7. Now you can restore your project files easily simply by clicking `Tools > Restore Files`

## Known Issues

1. uVision may show dialogs such that the source files are changed outside it.(***No Fix***)
2. Binary literals inside comments,function names , etc (except string literals) will be replaced with hex literals .(***I won't fix this , but PR are welcomed***)
3. Source files won't auto restore if the C51 compiler (or other components of the toolchain) fails.

## Commandline Helps
```powershell
binpp FILES_LIST  # Process files and make backups
binpp restore # Recursively restore all directories under working directory.
binpp # The same as above
binpp restore FILES_OR_DIRECTORIES_LIST # restore files and recusively restore directories
```

## How it works
Read the source code thoroughly and you can know how it works!
