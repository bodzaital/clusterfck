# Clusterfck

![clusterfck v1.1](https://img.shields.io/badge/clusterfck-v1.1-purple) ![dotnet core v3.1](https://img.shields.io/badge/dotnet_core-3.1-green)

Welcome to clusterfck, or, what I call an afternoon well spent.

Clusterfck is like brainfuck but stupid.

Call the interpreter by `dotnet ./clusterfck.dll file_to_open [ --debug, --vs, --time ]`

### Parameters

- `file_to_open`: a clusterfck source file.
- `--debug`: pause on breakpoints.
- `--vs`: after finishing, wait for an Enter key before quitting the interpreter.
- `--time`: display the execution time from opening the source file to finishing.

## Commands

| Command   | Description                                                            |
|-----------|------------------------------------------------------------------------|
| `+`       | Increment the data pointer                                             |
| `-`       | Decrement the data pointer                                             |
| `>`       | Increment the register pointer                                         |
| `<`       | Decrement the register pointer                                         |
| `$`       | Save the current data into the current register                        |
| `#`       | Switch between integer and char modes                                  |
| `Đ`       | Load the current register into the current data                        |
| `=`       | Read the register into the output buffer                               |
| `_`       | Dump the output buffer onto the screen and reset it                    |
| `.`       | Set a breakpoint for --debug                                           |
| `(`       | Look at the data pointer's value and loop this section that many times |
| `)`       | Loop end marker                                                        |
| `x`       | Reset the registerPtr to 0.                                            |
| `÷`       | Reset the dataPtr to 0.                                                |
| `¤`       | Read input from the user into the current register                     |
| ``` ` ``` | Comment marker, anything between is ignored.                           |

### The data pointer

The data pointer is an imaginary source of data in the form of numbers, starts at (and resets to) 0. +/- can increment or decrement its value.

### The register and register pointer

As of writing this, there are 32 registers that can hold a number and the register pointer points to one of these. >/< can increment or decrement the register pointer. Other operations may also increment the register pointer (save, load, read, input).

### Char mode, input, and the output buffer

The interpreter starts out in integer mode, meaning numbers are treated as numbers i.e. '65' means '65'. Char mode, however, treats numbers as ASCII character codes, i.e. '65' means 'A'. This check is used during input from the user and reading into the output buffer.

The output buffer sits between the registers and the output. One by one, the registers are loaded into the output buffer (either as numbers or characters), then the entire output buffer can be displayed (and emptied).

### Looping

Looping is just a for loop where the length is the value of the data pointer before entering the loop. Subsequent changes to the data pointer are disregarded.

### Breakpoints

If the interpreter is called with the `--debug` parameter, on breakpoint commands the execution pauses and the values of the data pointer, register pointer, the registers, and the output buffer are displayed, alongside the character mode flag. On hitting `Enter`, execution resumes.

## Examples

### Hello World

Displays the text "Hello World" on the screen.

```clusterfck
`=== CHARACTER H ===`
+++++++(		`7 times...`
	++++++++++	`add 10`
)				`dataPtr: 70`
++				`add 2 to get 72 (H)`
$				`save dataPtr to the current register -> [0] = 72`

`=== CHARACTER E ===`

---				`subtract 3 from dataPtr -> 69 (E)`
$				`save dataPtr to the current register -> [1] = 69`

`=== CHARACTER L ===`

+++++++
$

`=== CHARACTER L ===`

$

`=== CHARACTER O ===`

+++
$

`=== CHARACTER SPACE ===`

÷				`reset dataPtr -> 0`
+++(
	++++++++++
)
++
$

`=== CHARACTER W ===`

÷
+++++++++(
	++++++++++
)
---
$

`=== CHARACTER O ===`

--------
$

`=== CHARACTER R ===`

+++
$

`=== CHARACTER L ===`

------
$

`=== CHARACTER D ===`

--------
$

x				`reset the registerPtr -> [0]`
÷				`reset the dataPtr -> 0`
#				`change mode: integer -> char`
++++(			`dump the entire register array into the output buffer`
	++++++++(
		=
	)
)
.
_				`dump the output buffer on the screen and empty the output buffer`
```

*Minimized:*

```clusterfck
+++++++(++++++++++)++$---$+++++++$$+++$÷+++(++++++++++)++$÷+++++++++(++++++++++)---$--------$+++$------$--------$x÷#++++(++++++++(=))._
```

### Hello user

Asks for a name and displays "Hello {name}" on the screen.

```clusterfck
`Writing 'NAME?' to the screen.`

#

++++++++(
	++++++++++
)
--
$

÷
++++++(
	++++++++++
)
+++++
$

÷
++++++++(
	++++++++++
)
---
$

--------
$

÷
++++++(
	++++++++++
)
+++
$

x
=====
_

`Preparing 'HELLO ' in the registers so the user input is placed after it.`

x
÷
+++++++(
	++++++++++
)
++
$

---
$

+++++++
$

$

+++
$

÷
+++(
	++++++++++
)
++
$

`Now reading the user input.`

¤

`And writing out the entire register array.`

÷
x
++++(
	++++++++(
		=
	)
)
_
```

*Minimized:*

```clusterfck
#++++++++(++++++++++)--$÷++++++(++++++++++)+++++$÷++++++++(++++++++++)---$--------$÷++++++(++++++++++)+++$x=====_x÷+++++++(++++++++++)++$---$+++++++$$+++$÷+++(++++++++++)++$¤÷x++++(++++++++(=))_
```

# ClusterASM

ClusterASM is an extension of clusterfck and a bit higher level language that compiles down to clusterfck. The compiler is `clusterfck-precomp`.

### Parameters

`dotnet ./clusterfck-precomp file`

Running this command will load the file and compile it down to clusterfck in a file called `file.cfasm`.

### Commands

Commands in ClusterASM are in the form `CMD PARAM`. Only one command can be on one line. Not all commands have a parameter, or rather, an implicit `VOID` parameter gets added if there is no parameter after a command.

| cf  | cfasm | Description                                                     |
|-----|-------|-----------------------------------------------------------------|
| `+` | INC   | Increment the data pointer by its parameter                     |
| `-` | DEC   | Decrement the data pointer by its parameter                     |
| `>` | RIG   | Increment the register pointer by its parameter                 |
| `<` | LEF   | Decrement the register pointer by its parameter                 |
| `$` | STR   | Store the current data into the current register, no parameter. |
| `#` | SWT   | Switch between integer and char modes, no parameter.            |
| `Đ` | LOD   | Load the current register into the current data, no parameter.  |
| `=` | REA   | Read a number of registers by its parameter.                    |
| `_` | DMP   | Dump the output to the output buffer, no parameter.             |
| `.` | BRP   | Set a breakpoint, no parameter.                                 |
| `(` | LPS   | Loop the code between LPS and LPE by its parameter.             |
| `)` | LPE   | Loop end marker, no parameter.                                  |
| `x` | RRG   | Reset the register pointer, no parameter.                       |
| `÷` | RDT   | Reset the data pointer, no parameter.                           |
| `¤` | GET   | Gets input from the user, no parameter.                         |

## Examples

### Hello World

Displays the text "Hello World" on the screen.

```clusterasm
INC 72
STR
DEC 3
STR
INC 7
STR
STR
INC 3
STR
RDT
INC 32
STR
INC 87
STR
DEC 8
STR
INC 3
STR
DEC 6
STR
DEC 8
STR
RRG
RDT
SWT
REA 32
DMP
```

## Recommended file extensions

The `.cf` files should contain raw clusterfck code. The new ClusterASM files should have an extension of `.cfasm`. But really, anything goes. It's all text files.

## Plans

- ClusterShrp: C#-like language that compiles to clusterfck
  - `dataPtr++` compiles to `+`
  - `for (i = dataPtr; i > 0; i--) { ...` compiles to `(`

- Visual Studio Code extensions for syntax highlight and a language server

...and many more projects of questionable natures.

## Changelog

### v1.2

*Repo:*

- Updated readme.
- Added ClusterASM/`clusterfck-precomp`.

*ClusterASM:*

ClusterASM is an extension of clusterfck that compiles down to clusterfck. Repetitive commands, like `++++++++++` becomes `INC 10`, etc. The compiler for ClusterASM is `clusterfck-precomp`.

### v1.1

*Repo:*

- Updated readme.

*Language:*

- Added new commands:
  - `¤` - read user input.
  - `x` - reset the registerPtr.
  - ``` ` ``` - surrounds comments.
- Changed commands:
  - `x` became `÷` - reset the dataPtr.
- Fixed how loops work. When entering a loop, the dataPtr is automatically reset, so multiplication now works as intended. Before, 3*10 loop would be 33 and not 30, because the extra 3 on the outside loop remained in the dataPtr.

*Interpreter:*

- Added new command line parameters:
  - `--vs`: load `main.cf` in debug mode and waits for an `Enter` keypress when finished. Useful for debugging from Visual Studio.
  - `--time`: display execution time.
- Changed how the Debugger works: the `WriteLine` and `Dump` methods are aware if the interpreter is in debug mode.
- Changed the `if-elseif...` blocks to a `switch-case`.
- Changed dumping to the screen from a `WriteLine` to a `Write` but there's still a newline somewhere...
- Fixed the way whitespaces are ignored: the space character '` `' is ignored as well.

### v1.0

Original release.