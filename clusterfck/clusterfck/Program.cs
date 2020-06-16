using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace clusterfck
{
    class Program
    {
        static void Main(string[] args)
        {
            // When no arguments are given, assume debugging from Visual Studio: load main.cf in debug mode.
            if (args.Length == 0)
            {
                args = new string[3] { "main.cf", "--debug", "--vs" };
            }

            string file = args[0];
            Debugger.debugMode = args.Contains("--debug");
            Debugger.vsMode = args.Contains("--vs");
            Debugger.timeMode = args.Contains("--time");

            Debugger.WriteLine($"Debuging {file} - on breakpoint enter to continue.", ConsoleColor.Yellow);
            
            if (!File.Exists(file))
            {
                Debugger.WriteLine($"File {file} does not exist.", mustDisplay: true);
                return;
            }

            // Diagnostics from opening the file.
            Stopwatch s = new Stopwatch();
            s.Start();

            // Read the source.
            string input;
            using (StreamReader sr = new StreamReader(file))
            {
                input = sr.ReadToEnd();
            }

            // Yeet out the whitespace characters.
            input = input.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace(" ", string.Empty);

            // Yeet out the break points if not in debug mode.
            input = Debugger.RemoveBreakpoints(input);

            bool charMode = false;

            int dataPtr = 0;
            int registerPtr = 0;

            int[] registers = new int[32];
            bool isComment = false;

            StringBuilder outputBuffer = new StringBuilder();
            Stack<IterationHandler> iterationHandlers = new Stack<IterationHandler>();

            for (int programCtr = 0; programCtr < input.Length; programCtr++)
            {
                char v = input[programCtr];

                if (isComment)
                {
                    if (v != '`')
                    {
                        continue;
                    }

                    isComment = false;
                    continue;
                }

                if (!new char[] { '+', '-', '>', '<', '#', '$', 'Đ', '=', '_', '.', '(', ')', 'x', '¤', '`', '÷' }.Any(x => x == v))
                {
                    Debugger.WriteLine($"Unknown symbol '{v}' at {programCtr}. Terminating.");
                    break;
                }

                switch (v)
                {
                    case '+':
                        dataPtr++; break;
                    case '-':
                        dataPtr--; break;
                    case '>':
                        registerPtr++; break;
                    case '<':
                        registerPtr--; break;
                    case '$':
                        registers[registerPtr++] = dataPtr; break;
                    case 'Đ':
                        dataPtr = registers[registerPtr]; break;
                    case '#':
                        charMode = !charMode; break;
                    case '¤':
                        string user_input = Console.ReadLine();
                        if (charMode)
                        {
                            // if in charmode, save every character of the user_input as ascii numbers.
                            for (int i = 0; i < user_input.Length; i++)
                            {
                                registers[registerPtr++] = Encoding.ASCII.GetBytes(user_input)[i];
                            }
                        }
                        else
                        {
                            // try to save the input as number.
                            if (int.TryParse(user_input, out int user_input_number))
                            {
                                registers[registerPtr++] = user_input_number;
                            }
                        }
                        break;
                    case '=':
                        if (charMode)
                        {
                            outputBuffer.Append((char)registers[registerPtr++ % registers.Length]);
                        }
                        else
                        {
                            outputBuffer.Append(registers[registerPtr++ % registers.Length]);
                        }
                        break;
                    case '_':
                        Console.Write(outputBuffer);
                        outputBuffer = new StringBuilder();
                        break;
                    case '÷':
                        dataPtr = 0; break;
                    case 'x':
                        registerPtr = 0; break;
                    case '(':
                        iterationHandlers.Push(new IterationHandler(dataPtr - 1, programCtr));
                        dataPtr = 0;
                        break;
                    case ')':
                        if (iterationHandlers.Peek().RemainingCount > 0)
                        {
                            iterationHandlers.Peek().RemainingCount--;
                            programCtr = iterationHandlers.Peek().Head;
                        }
                        else
                        {
                            iterationHandlers.Pop();
                        }
                        break;
                    case '.':
                        Debugger.WriteLine($"Breakpoint at {programCtr}");
                        Debugger.Dump(charMode, dataPtr, registerPtr, programCtr, registers, outputBuffer);
                        Debugger.WriteLine("PAUSED.");

                        Console.ReadLine();
                        break;
                    case '`':
                        isComment = true;
                        break;
                    default:
                        break;
                }
            }

            s.Stop();

            if (Debugger.timeMode)
            {
                Console.WriteLine($"Done in {s.ElapsedMilliseconds}ms");
            }

            if (Debugger.vsMode)
            {
                Console.ReadLine();
            }
        }
    }
}
