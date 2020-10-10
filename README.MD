# BinPP

An utility to make C51 recognize binary literals such as `0b10010010`.

> Please read this README from beginning to end.  
> **IF YOU DO NOT UNDERSTAND WHAT YOU ARE DOING , PLEASE STOP!**

> THIS SOFTWARE IS PROVIDED "AS-IS" , WITHOUT ANY EXPRESSED OR IMPLIED WARRANTY .USE IT AT YOUR OWN RISK !  
## Usage

1. Open Keil uVision.
2. Get to your project.
3. Copy `binpp.exe` and `System.ValueTuple.dll`
3. Open the option window of your target.
4. Get to the `User` tab.
5. Edit it .(Follow the picture below)

![image-20201003104754898](README.assets/image-20201003104754898.png)

- In the `Before Build/Rebuild` Tab add your source file name to the arguments of the binpp.exe
- Set the error code to <0
- Have Fun !

- If the Build fails , the source files won't be restored automatically.
  - You need to manually double click the binpp.exe in your project folder to restore source files under the project folder.

## Known Issues

1. uVision may show dialogs such that the source files are changed outside it.(***No Fix***)
2. Binary literals inside comments,function names , etc will be replaced with hex literals .(***I won't fix this , but PR are welcomed***)
3. Source files won't auto restore if the C51 compiler (or other components of the toolchain) fails.
