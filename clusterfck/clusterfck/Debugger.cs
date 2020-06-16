using System;
using System.Collections.Generic;
using System.Text;

namespace clusterfck
{
    public static class Debugger
    {
        public static bool debugMode = false;
        public static bool vsMode = false;
        public static bool timeMode = false;

        public static void WriteLine(string m, ConsoleColor foregroundColor = ConsoleColor.Red, bool mustDisplay = false)
        {
            if (debugMode || mustDisplay)
            {
                ConsoleColor fg = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine(m);
                Console.ForegroundColor = fg;
            }
        }

        public static void Dump(bool charMode, int dataPtr, int registerPtr, int programCtr, int[] registers, StringBuilder outputBuffer)
        {
            if (debugMode)
            {
                Console.WriteLine($"│ {nameof(charMode)}: {charMode}");
                Console.WriteLine($"│ {nameof(dataPtr)}: {dataPtr}");
                Console.WriteLine($"│ {nameof(registerPtr)}: {registerPtr}");
                Console.WriteLine($"│ {nameof(programCtr)}: {programCtr}");
                Console.WriteLine($"│ {string.Join(',', registers)}");
                Console.WriteLine($"│ {nameof(outputBuffer)}: {outputBuffer}");
            }
        }

        public static string RemoveBreakpoints(string input)
        {
            return debugMode ? input : input.Replace(".", string.Empty);
        }
    }
}
