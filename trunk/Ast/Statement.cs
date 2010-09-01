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
