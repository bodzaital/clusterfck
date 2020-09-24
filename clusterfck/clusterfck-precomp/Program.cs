using System;
using System.IO;
using System.Text;

namespace clusterfck_precomp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] commands = File.ReadAllLines(args[0]);
            StringBuilder sb = new StringBuilder();
            foreach (string command in commands)
            {
                string[] part;
                if (!command.Contains(' '))
                {
                    part = new string[2]
                    {
                        command,
                        "VOID",
                    };
                }
                else
                {
                    part = command.Split(' ');
                }

                switch (part[0])
                {
                    case "INC":
                        int inc_ctr = int.Parse(part[1]);
                        for (int i = 0; i < inc_ctr; i++)
                        {
                            sb.Append('+');
                        }
                        break;
                    case "DEC":
                        int dec_ctr = int.Parse(part[1]);
                        for (int i = 0; i < dec_ctr; i++)
                        {
                            sb.Append('-');
                        }
                        break;
                    case "RIG":
                        int rig_ctr = int.Parse(part[1]);
                        for (int i = 0; i < rig_ctr; i++)
                        {
                            sb.Append('>');
                        }
                        break;
                    case "LEF":
                        int left_ctr = int.Parse(part[1]);
                        for (int i = 0; i < left_ctr; i++)
                        {
                            sb.Append('<');
                        }
                        break;
                    case "STR":
                        sb.Append('$');
                        break;
                    case "SWT":
                        sb.Append('#');
                        break;
                    case "LOD":
                        sb.Append('Đ');
                        break;
                    case "REA":
                        int rea_ctr = int.Parse(part[1]);
                        for (int i = 0; i < rea_ctr; i++)
                        {
                            sb.Append('=');
                        }
                        break;
                    case "DMP":
                        sb.Append('_');
                        break;
                    case "BRP":
                        sb.Append('.');
                        break;
                    case "LPS":
                        int lps_ctr = int.Parse(part[1]);
                        sb.Append('÷');
                        for (int i = 0; i < lps_ctr; i++)
                        {
                            sb.Append('+');
                        }
                        sb.Append('(');
                        break;
                    case "LPE":
                        sb.Append(')');
                        break;
                    case "RRG":
                        sb.Append('x');
                        break;
                    case "RDT":
                        sb.Append('÷');
                        break;
                    case "GET":
                        sb.Append('¤');
                        break;
                    default:
                        break;
                }

            }

            using StreamWriter sw = new StreamWriter($"{args[0]}.cfasm");
            sw.WriteLine(sb.ToString());
            Console.WriteLine($"Compiled to {args[0]}.cfasm");
        }
    }
}
