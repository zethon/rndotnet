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

        private Statement ParseStatement()
        {
            Statement result = null;

            if (_index == _tokens.Count())
            {
                throw new Exception("error RCNC003: expected statement, got EOF");
            }

            if (_tokens[_index] is char && (char)_tokens[_index] == 'U')
            {
                MathStatement stmt = new MathStatement();
                stmt.Op = MathOperator.Add;

                if (_index + 1 < _tokens.Count())
                {
                    while (_tokens[_index + 1] is int || _tokens[_index + 1] == Scanner.Prime)
                    {
                        if (_tokens[_index + 1] is int)
                        {
                            stmt.Value = (int)_tokens[_index + 1];
                        }
                        else
                        {
                            stmt.Op = stmt.Op == MathOperator.Add ? MathOperator.Subtract : MathOperator.Add;
                        }

                        _index++;
                    }
                }

                result = stmt;
                _index++;
            }
            else if (_tokens[_index] is char && (char)_tokens[_index] == 'F')
            {
                //_index++;

                Statement stmt = null;

                if (_index + 1 < _tokens.Count())
                {
                    while (_tokens[_index+1] == Scanner.Prime)
                    {
                        if (stmt == null || stmt is ReadInt)
                        {
                            stmt = new Print();
                        }
                        else
                        {
                            stmt = new ReadInt();
                        }

                        _index++;
                    }
                }
                result = stmt;
                _index++;
            }
            else if (_tokens[_index] == Scanner.OpenParen)
            {
                _index++;

                WhileStatement stmt = new WhileStatement();
                stmt.Body = ParseStatement();

                if (_index == _tokens.Count() || _tokens[_index] != Scanner.CloseParen)
                {
                    throw new Exception("error RCNC004: expected ')'");
                }

                _index++;
            }

            //if (_index <= _tokens.Count())
            //{
                if (_index <= _tokens.Count() )
                    //&& 
                      //  _tokens[_index] != Scanner.CloseParen && _tokens[_index] != Scanner.CloseBracket)
                {
                    Sequence seq = new Sequence();
                    seq.First = result;
                    seq.Second = ParseStatement();
                    result = seq;
                }
            //}


            return result;
         
        }

    }
}
