using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksNotation
{
    public class Parser
    {
        private int _index;
        private IList<object> _tokens;

        private readonly Statement _result;
        public Statement Result
        {
            get { return _result; }
        }

        public Parser(IList<object> tokens)
        {
            _tokens = tokens;
            _index = 0;

            _result = ParseStatement();

            if (_index != tokens.Count())
                throw new Exception("error RCNC002: expected EOF");
        }

        private bool IsCommand(object obj)
        {
            bool bRet = false;

            if (obj is char)
            {
                char c = (char)obj;

                switch (c)
                {
                    case 'R':
                    case 'F':
                        bRet = true;
                        break;

                    default:
                        bRet = false;
                        break;
                }
            }
            else if (obj is Symbols && (Symbols)obj == Symbols.OpenParen || (Symbols)obj == Symbols.CloseParen)
            {
                bRet = true;
            }

            return bRet;
        }


        private bool ParseIsPrime()
        {
            bool bRet = false;
            int tempdex = _index+1;

            while (tempdex < _tokens.Count() && !IsCommand(_tokens[tempdex]))
            {
                if (_tokens[tempdex] is Symbols && (Symbols)_tokens[tempdex] == Symbols.Prime)
                {
                    bRet = !bRet;
                }

                tempdex++;
            }

            return bRet;
        }

        private int ParseIntLiteral()
        {
            int iRet = 1;
            int tempdex = _index+1;

            while (tempdex < _tokens.Count() && !IsCommand(_tokens[tempdex]))
            {
                if (_tokens[tempdex] is int)
                {
                    iRet = (int)_tokens[tempdex];
                    break;
                }

                tempdex++;
            }

            return iRet;
        }

        private void IncrementIndex()
        {
            _index++;

            while (_index < _tokens.Count() && !IsCommand(_tokens[_index]))
            {
                _index++;
            }
        }

        private Statement ParseStatement()
        {
            Statement result = null;

            if (_index >= _tokens.Count())
            {
                throw new Exception("error RCNC003: expected statement, got EOF");
            }

            if (_tokens[_index] is char && (char)_tokens[_index] == 'R')
            {
                MathStatement stmt = new MathStatement 
                { 
                    Op = ParseIsPrime() ? MathOperator.Subtract : MathOperator.Add,
                    Value = ParseIntLiteral()
                };

                result = stmt;
                IncrementIndex();
            }
            else if (_tokens[_index] is char && (char)_tokens[_index] == 'F')
            {
                Statement stmt;

                if (ParseIsPrime())
                {
                    stmt = new ReadInt();
                }
                else
                {
                    stmt = new Print();
                }
                
                stmt.Value = ParseIntLiteral();
                
                result = stmt;
                IncrementIndex();
            }
            else if (_tokens[_index] is char && (char)_tokens[_index] == 'U')
            {
                Statement stmt = new PointerStatement
                {
                    Op = ParseIsPrime() ? PointerOperator.Dec : PointerOperator.Inc,
                    Value = ParseIntLiteral()
                };

                result = stmt;
                IncrementIndex();
            }
            else if (_tokens[_index] is Symbols && (Symbols)_tokens[_index] == Symbols.OpenParen)
            {
                _index++;
                WhileStatement stmt = new WhileStatement();
                stmt.Body = ParseStatement();

                result = stmt;

                if (_index >= _tokens.Count() || !(_tokens[_index] is Symbols) || (Symbols)_tokens[_index] != Symbols.CloseParen)
                {
                    throw new Exception("RCNC004: unmatched ')'");
                }

                _index++;
            }
            else
            {
                throw new Exception("RCNC005: unrecognized command token");
            }

            if (_index  < _tokens.Count() && (!(_tokens[_index] is Symbols) || (Symbols)_tokens[_index] != Symbols.CloseParen))
            {
                Sequence seq = new Sequence();
                seq.First = result;
                seq.Second = ParseStatement();

                result = seq;
            }

            return result;
        }

    }
}
