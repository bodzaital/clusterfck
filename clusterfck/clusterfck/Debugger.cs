using System;
using System.Collections.Generic;
using System.Text;

namespace clusterfck
{
    public static class Debugger
    {
        public static void WriteLine(string m, ConsoleColor foregroundColor = ConsoleColor.Red)
        {
            ConsoleColor fg = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(m);
            Console.ForegroundColor = fg;
        }

        public static void Dump(bool charMode, int dataPtr, int registerPtr, int programCtr, int[] registers, StringBuilder outputBuffer)
        {
            Console.WriteLine($"│ {nameof(charMode)}: {charMode}");
            Console.WriteLine($"│ {nameof(dataPtr)}: {dataPtr}");
            Console.WriteLine($"│ {nameof(registerPtr)}: {registerPtr}");
            Console.WriteLine($"│ {nameof(programCtr)}: {programCtr}");

            StringBuilder s = new StringBuilder();

            Console.WriteLine($"│ {string.Join(',', registers)}");

            Console.WriteLine($"│ {nameof(outputBuffer)}: {outputBuffer}");
        }
    }
}
