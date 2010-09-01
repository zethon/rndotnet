using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksNotation
{
    public enum MathOperator
    {
        Add,
        Subtract
    }

    public enum PointerOperator
    {
        Inc,
        Dec
    }

    public abstract class Statement
    {
        public int Value = 1;
    }

    public class MathStatement : Statement
    {
        public MathOperator Op;
    }

    public class Print : Statement
    {

    }

    public class ReadInt : Statement
    {

    }

    public class PointerStatement : Statement
    {
        public PointerOperator Op;
    }

    public class WhileStatement : Statement
    {
        public Statement Body;
    }
}
