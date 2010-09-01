using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RubiksNotation
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: rcnc progran.rcn");
                return;
            }

            try
            {
                Scanner scanner = null;
                using (TextReader reader = File.OpenText(args[0]))
                {
                    scanner = new Scanner(reader);
                }

                Parser p = new Parser(scanner.Tokens);
                CodeGen gen = new CodeGen(p.Result, @"test.exe");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
