# Clusterfck

Welcome to clusterfck, or, what I call an afternoon well (that's questionable) spent.

Clusterfck is like brainfuck but actually bad. Why? I don't know, I basically spent a couple of hours in C# and this is the result.

Call the interpreter by `clusterfck.exe file_to_open [ --debug ]`

## Commands

There are 11 commands in clusterfck.

| Command | Description                                                                 | Equivalent C#                                    |
|---------|-----------------------------------------------------------------------------|--------------------------------------------------|
| +       | Increment the data pointer                                                  | `dataPtr++`                                      |
| -       | Decrement the data pointer                                                  | `dataPtr--`                                      |
| >       | Increment the register pointer                                              | `registerPtr++`                                  |
| <       | Decrement the register pointer                                              | `registerPtr--`                                  |
| $       | Save the current data into the current register                             | `registers[registerPtr++] = dataPtr`             |
| #       | Switch between integer and char modes                                       | `charMode = !charMode`                           |
| Đ       | Load the current register into the current data                             | `dataPtr = registers[registerPtr++]`             |
| =       | Read the register into the output buffer and increment the register pointer | `outputBuffer += charMode ? (char)registers[registerPtr++] : registers[registerPtr++]` |
| _       | Dump the output buffer onto the screen and reset the output buffer          | `Console.WriteLine(outputBuffer)`                |
| .       | Set a breakpoint for --debug                                                |                                                  |
| (       | Look at the data pointer's value and loop this section that many times      | `int i = dataPtr; for (i = 0; i < t; i++) { ...` |
| )       | Loop end marker                                                             | `... }`                                          |
| x       | Reset the dataPtr to 0.                                                     | `dataPtr = 0`                                    |

The dataPtr is an imaginary source of data which produces numbers, starting out at 0.

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