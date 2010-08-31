using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RubiksNotation
{
    public sealed class Scanner
    {
        private readonly IList<object> _results;
        public IList<object> Tokens
        {
            get { return _results; }
        }

        public static readonly object OpenParen = new object();
        public static readonly object CloseParen = new object();
        public static readonly object OpenBracket = new object();
        public static readonly object CloseBracket = new object();
        public static readonly object Prime = new object();

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
                        _results.Add(Scanner.Prime);
                    break;

                    case '(':
                        input.Read();
                        _results.Add(Scanner.OpenParen);
                    break;

                    case ')':
                        input.Read();
                        _results.Add(Scanner.CloseParen);
                    break;

                    case '[':
                        input.Read();
                        _results.Add(Scanner.OpenBracket);
                    break;

                    case ']':
                        input.Read();
                        _results.Add(Scanner.CloseBracket);
                    break;

                    default:
                        throw new Exception("error RCNC001: unknown character '" + ch + "'");
                }

            }
        }


    }
}
