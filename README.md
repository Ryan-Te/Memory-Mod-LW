# Memory-Mod-LW
A port of my TUNG memory mod to LW  
  
  
you can create a data file like this  
```
MMROM
b, t;
DATA
...
...
...
```  
b is the base  
use D for Decimal,  
H for Hexadecimal,  
B for Binary and  
O for Octal.  

t is the format.  

if you remove t and the comma,  it will format the rom to be based on n digits  
8 for binary,  
3 for octal,  
3 for decimal and  
2 for hex.  

if t is "lb" then every line break means a new value  
if t is "lbs" then every line break and space means a new value  

in lb and lbs format, if you put more digits than it expects then they will be cut off.  
if you put less, it will act as if there is invisible 0s behind it.  
thats pretty much it for creating these files.  
  
  
to load it you execute `server "setRomLoc LOCATION`. LOCATION is with the Logic World  directory as the root.  
then you pulse the `Change Flash Location` peg (the tiny one) and then pulse the Flash peg.
  
Pin Reference:  
![oof](https://ryan-te.github.io/Images/GitHub/Memory_Mod/Memory-Reference.png)
