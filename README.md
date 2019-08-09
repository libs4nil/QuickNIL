# QuickNIL
Quick CLI tool for prototyping in NIL

All it does is run a .nil script.
It does NOT add any extra APIs except the basic libraries/modules Lua already adds to NIL, and with the exception of an array named AppArgs which will be a table containing the extra arguments a user gives from the command prompt.

This is just for quick experimentation with NIL, and nothing more.

# WIP
This project is still in WIP... It may not function properly, yet!


# Compile

If you want to compile this yourself, create a folder for my work, and type the following from that folder

~~~shell
git clone https://github.com/libs4nil/QuickNIL
git clone https://github.com/Tricky1975/TrickyUnits
mkdir Bubble
mkdir Bubble/NIL
git clone https://github.com/jpbubble/NIL-isn-t-Lua Bubble/NIL
~~~
After doing that you can just use Visual Studio to build QuickNIL. Please do make sure NLua is properly linked to this project, or this may not work.


