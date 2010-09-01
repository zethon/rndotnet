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

namespace RubiksNotation
{
    public enum Symbols
    {
        OpenParen,
        CloseParen,
        Prime
    }

    public sealed class Scanner
    {


        private readonly IList<object> _results;
        public IList<object> Tokens
        {
            get { return _results; }
        }

        public Scanner(TextReader input)
        {
            _results = new List<object>();
            Scan(input);
        }

        private void Scan(TextReader input)
        {
            while (input.Peek() != -1)
            {
                char ch = (char)input.Peek();

                if (char.IsWhiteSpace(ch))
                {
                    input.Read();
                }
                else if (char.IsLetter(ch))
                {
                    input.Read();
                    _results.Add(ch);
                }
                else if (char.IsDigit(ch))
                {
                    StringBuilder accum = new StringBuilder();

                    while (char.IsDigit(ch))
                    {
                        accum.Append(ch);
                        input.Read();

                        if (input.Peek() == -1)
                        {
                            break;
                        }
                        else
                        {
                            ch = (char)input.Peek();
                        }
                    }

                    _results.Add(int.Parse(accum.ToString()));
                }
                else switch (ch)
                {
                    case '\'':
                        input.Read();
                        _results.Add(Symbols.Prime);
                    break;

                    case '(':
                        input.Read();
                        _results.Add(Symbols.OpenParen);
                    break;

                    case ')':
                        input.Read();
                        _results.Add(Symbols.CloseParen);
                    break;

                    default:
                        throw new Exception("error RCNC001: unknown character '" + ch + "'");
                }

            }
        }


    }
}
