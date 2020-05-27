using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace clusterfck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The clusterfck interpreter.");

            // When debugging with F5, just load a main.cf.
            if (args.Length == 0)
            {
                args = new string[1] { "main.cf" };
            }

            string file = args[0];
            bool debugMode = args.Contains("--debug");

            if (debugMode)
            {
                Debugger.WriteLine($"Debuging {file} - on breakpoint enter to continue.", ConsoleColor.Yellow);
            }

            if (!File.Exists(file))
            {
                Debugger.WriteLine($"File {file} does not exist.");
                return;
            }

            string input;
            using (StreamReader sr = new StreamReader(file))
            {
                input = sr.ReadToEnd();
            }

            // Yeet out the whitespace characters.
            input = input.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);

            if (!debugMode)
            {
                input = input.Replace(".", string.Empty);
            }

            bool charMode = false;

            int dataPtr = 0;
            int registerPtr = 0;

            int[] registers = new int[32];

            StringBuilder outputBuffer = new StringBuilder();
            Stack<IterationHandler> iterationHandlers = new Stack<IterationHandler>();

            for (int programCtr = 0; programCtr < input.Length; programCtr++)
            {
                char v = input[programCtr];

                if (!new char[] { '+', '-', '>', '<', '#', '$', 'Đ', '=', '_', '.', '(', ')', 'x' }.Any(x => x == v))
                {
                    Debugger.WriteLine($"Unknown symbol {v} at {programCtr}. Terminating.");
                    break;
                }

                if (v == '+')
                {
                    dataPtr++;
                }

                if (v == '-')
                {
                    dataPtr--;
                }

                if (v == '>')
                {
                    registerPtr++;
                }

                if (v == '<')
                {
                    registerPtr--;
                }

                if (v == '$')
                {
                    registers[registerPtr++] = dataPtr;
                }

                if (v == 'Đ')
                {
                    dataPtr = registers[registerPtr];
                }

                if (v == '#')
                {
                    charMode = !charMode;
                }

                if (v == '=')
                {
                    if (charMode)
                    {
                        outputBuffer.Append((char)registers[registerPtr++]);
                    }
                    else
                    {
                        outputBuffer.Append(registers[registerPtr++]);
                    }
                }

                if (v == '_')
                {
                    Console.WriteLine(outputBuffer);
                    outputBuffer = new StringBuilder();
                }

                if (v == 'x')
                {
                    dataPtr = 0;
                }

                if (v == '(')
                {
                    iterationHandlers.Push(new IterationHandler(dataPtr - 1, programCtr));
                }

                if (v == ')')
                {
                    if (iterationHandlers.Peek().RemainingCount > 0)
                    {
                        iterationHandlers.Peek().RemainingCount--;
                        programCtr = iterationHandlers.Peek().Head;
                    }
                    else
                    {
                        iterationHandlers.Pop();
                    }
                }

                if (v == '.')
                {
                    Debugger.WriteLine($"Breakpoint at {programCtr}");
                    Debugger.Dump(charMode, dataPtr, registerPtr, programCtr, registers, outputBuffer);
                    Debugger.WriteLine("PAUSED.");

                    Console.ReadLine();
                }
            }
        }
    }
}
