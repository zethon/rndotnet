using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksNotation
{
    public abstract class Expression
    {
    }

    public class IntLiteral : Expression
    {
        public int Value;
    }

    public class MathExpression : Expression
    {
        public Expression Value;
        public MathOperator Op;
    }
}
