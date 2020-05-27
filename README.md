# Clusterfck

Welcome to clusterfck, or, what I call an afternoon well (that's questionable) spent.

Clusterfck is like brainfuck but actually bad. Why? I don't know, I basically spent a couple of hours in C# and this is the result.

## Commands

There are 11 commands in clusterfck.

| Command | Description                                                                 |
|---------|-----------------------------------------------------------------------------|
| +       | Increment the data pointer                                                  |
| -       | Decrement the data pointer                                                  |
| >       | Increment the register pointer                                              |
| <       | Decrement the register pointer                                              |
| $       | Save the current data into the current register                             |
| #       | Switch between integer and char modes                                       |
| Đ       | Load the current register into the current data                             |
| =       | Read the register into the output buffer and increment the register pointer |
| _       | Dump the output buffer onto the screen and reset the output buffer          |
| .       | Set a breakpoint for --debug                                                |
| (       | Look at the data pointer's value and loop this section that many times      |
| )       | Loop end marker                                                             |

Looping is basically:

```c
int t = dataPtr
for (i = 0; i < t; i++) { ... }
```

The dataPtr (data pointer) is an imaginary source of data which produces numbers and starts out as 0. +/- can manipulate the dataPtr. The interpreter starts out in integer mode, # changes into char mode (or back), which basically reads the numbers as ASCII characters.

## Hello World

Or just "hello" because I don't have the patience to do this.

```
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+++++++$
---$
+++++++$
$
+++$

x
+++++(
	<
)

x
#

x
+++++(
	=
)

_
```

Going line by line (dataPtr and registerPtr starts at 0):

```
Increment dataPtr by 65 (ASCII: A)

Increment dataPtr by 7 (to 72) and save into the current register (0)
Decrement dataPtr by 3 (to 69) and save into the current register (1)
Increment dataPtr by 7 (to 76) and save into the current register (2)
Save into the current register (3)
Increment dataPtr by 3 (to 79) and save into the current register (4)

Reset the dataPtr to 0

5 times, do:
	decrement the register pointer

Reset the dataPtr to 0

Switch into character mode

5 times, do:
	Read the register into the output buffer

Dump the output buffer onto the screen
```

Clusterfck.exe can do debugging with the `--debug` flag. When a "." is reached, it halts and displays the values of dataPtr, charMode, registerPtr, the values of filled registers, and the outputBuffer.