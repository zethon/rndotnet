/*
 * Copyright 2010 Adalid Claure
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at 
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0 
 *      
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, 
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 * See the License for the specific language governing permissions and 
 * limitations under the License. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace RubiksNotation
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string strTitle = ((AssemblyTitleAttribute)asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            string strCopyright = ((AssemblyCopyrightAttribute)asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            FileVersionInfo verInfo = FileVersionInfo.GetVersionInfo("rcnc.exe");
            
            Console.WriteLine("{0} version {1}",strTitle,verInfo.FileVersion);
            Console.WriteLine("{0}. All rights reserved.",strCopyright); 

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
                CodeGen gen = new CodeGen(p.Result, Path.GetFileNameWithoutExtension(args[0]) + ".exe");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
