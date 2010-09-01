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
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

namespace RubiksNotation
{
    public sealed class CodeGen
    {
        ILGenerator _il = null;

        LocalBuilder _Ptr = null;
        LocalBuilder _Array = null;

        MethodInfo _printMethod = null;
        MethodInfo _readMethod = null;

        Stack _labels = null;

        public CodeGen(Statement stmt, string fileName)
        {
            AssemblyName name = new AssemblyName(Path.GetFileNameWithoutExtension(fileName));
            AssemblyBuilder asmb = System.AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
            ModuleBuilder modb = asmb.DefineDynamicModule(fileName);
            TypeBuilder typeBuilder = modb.DefineType("Program");

            MethodBuilder methb = typeBuilder.DefineMethod("Main", MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);

            _printMethod = typeof(Console).GetMethod("Write", new Type[] { typeof(Char) });
            _readMethod = typeof(Console).GetMethod("Read", new Type[] { });

            _labels = new Stack();

            // CodeGenerator
            _il = methb.GetILGenerator();

            // declare the data pointer
            _Ptr = _il.DeclareLocal(typeof(int));
            _il.Emit(OpCodes.Ldc_I4_0);
            _il.Emit(OpCodes.Stloc, _Ptr);

            // delcare the data array
            _Array = _il.DeclareLocal(typeof(int[]));
            _il.Emit(OpCodes.Ldc_I4, 30000);
            _il.Emit(OpCodes.Newarr, typeof(int));
            _il.Emit(OpCodes.Stloc, _Array);

            // Go Compile!
            GenStmt(stmt);

            _il.Emit(OpCodes.Ret);
            typeBuilder.CreateType();
            modb.CreateGlobalFunctions();
            asmb.SetEntryPoint(methb);
            asmb.Save(fileName);
        }

        private void GenStmt(Statement stmt)
        {
            if (stmt is Sequence)
            {
                Sequence seq = (Sequence)stmt;
                GenStmt(seq.First);
                GenStmt(seq.Second);
            }
            else if (stmt is MathStatement)
            {
                MathStatement m = (MathStatement)stmt;

                if (m.Op == MathOperator.Add)
                {
                    _il.Emit(OpCodes.Ldloc, _Array);
                    _il.Emit(OpCodes.Ldloc, _Ptr);
                    _il.Emit(OpCodes.Ldloc, _Array);
                    _il.Emit(OpCodes.Ldloc, _Ptr);
                    _il.Emit(OpCodes.Ldelem_I4);
                    _il.Emit(OpCodes.Ldc_I4_1);
                    _il.Emit(OpCodes.Add);
                    _il.Emit(OpCodes.Stelem_I4);
                }
                else if (m.Op == MathOperator.Subtract)
                {
                    _il.Emit(OpCodes.Ldloc, _Array);
                    _il.Emit(OpCodes.Ldloc, _Ptr);
                    _il.Emit(OpCodes.Ldloc, _Array);
                    _il.Emit(OpCodes.Ldloc, _Ptr);
                    _il.Emit(OpCodes.Ldelem_I4);
                    Label notTooNegative = _il.DefineLabel();
                    _il.Emit(OpCodes.Dup);
                    _il.Emit(OpCodes.Ldc_I4, 256);
                    _il.Emit(OpCodes.Add);
                    _il.Emit(OpCodes.Brtrue, notTooNegative);
                    _il.Emit(OpCodes.Pop);	// wrap from -256 to 255
                    _il.Emit(OpCodes.Ldc_I4, 256);
                    _il.MarkLabel(notTooNegative);
                    _il.Emit(OpCodes.Ldc_I4_1);
                    _il.Emit(OpCodes.Sub);
                    _il.Emit(OpCodes.Stelem_I4);
                }
            }
            else if (stmt is Print)
            {
                _il.Emit(OpCodes.Ldloc, _Array);
                _il.Emit(OpCodes.Ldloc, _Ptr);
                _il.Emit(OpCodes.Ldelem_I4);
                _il.Emit(OpCodes.Call, _printMethod);
            }
            else if (stmt is ReadInt)
            {
                _il.Emit(OpCodes.Ldloc, _Array);
                _il.Emit(OpCodes.Ldloc, _Ptr);
                _il.Emit(OpCodes.Call, _readMethod);
            }
            else if (stmt is PointerStatement)
            {
                PointerStatement p = (PointerStatement)stmt;

                _il.Emit(OpCodes.Ldloc, _Ptr);
                _il.Emit(OpCodes.Ldc_I4_1);

                if (p.Op == PointerOperator.Inc)
                {
                    _il.Emit(OpCodes.Add);
                }
                else if (p.Op == PointerOperator.Dec)
                {
                    _il.Emit(OpCodes.Sub);
                }

                _il.Emit(OpCodes.Stloc, _Ptr);
            }
            else if (stmt is WhileStatement)
            {
                WhileStatement w = (WhileStatement)stmt;

                Label startWhile = _il.DefineLabel();
                Label endWhile = _il.DefineLabel();
                _labels.Push(endWhile);
                _labels.Push(startWhile);
                _il.MarkLabel(startWhile);
                _il.Emit(OpCodes.Ldloc, _Array);
                _il.Emit(OpCodes.Ldloc, _Ptr);
                _il.Emit(OpCodes.Ldelem_I4);
                _il.Emit(OpCodes.Brfalse, endWhile);

                GenStmt(w.Body);

                if (_labels.Count == 0)
                {
                    throw new Exception("RCNC007: closing token appeared before opening token");
                }

                _il.Emit(OpCodes.Br, (Label)_labels.Pop());
                _il.MarkLabel((Label)_labels.Pop());
            }
            else
            {
                throw new Exception("RCNC006: don't know how to generate type '" + stmt.GetType().Name + "'");
            }
        }

    }
}
