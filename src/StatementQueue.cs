/*
 * MIT License
 * 
 * Copyright (c) 2025 Runic Compiler Toolkit Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;

namespace Runic
{
    public class StatementQueue : IStatementStream
    {
#if NET6_0_OR_GREATER
        IStatementStream? _statementStream;
#else
        IStatementStream _statementStream;
#endif
        public StatementQueue(IStatementStream statementStream)
        {
            _statementStream = statementStream;
        }
        public StatementQueue(Statement[] statements)
        {
            _statementStream = null;
            FrontLoadStatements(statements);
        }
        LinkedList<Statement> _queue = new LinkedList<Statement>();
        public void FrontLoadStatement(Statement statement)
        {
            _queue.AddFirst(statement);
        }
        public void FrontLoadStatements(Statement[] statements)
        {
            for (int n = 0; n < statements.Length; n++)
            {
                _queue.AddFirst(statements[statements.Length - 1 - n]);
            }
        }
#if NET6_0_OR_GREATER
        public Statement? PeekStatement()
#else
        public Statement PeekStatement()
#endif
        {
            if (_queue.Count > 0) { return _queue.First.Value; }
            if (_statementStream != null)
            {
#if NET6_0_OR_GREATER
                Statement? statement = _statementStream.ReadNextStatement();
#else
                Statement statement = _statementStream.ReadNextStatement();
#endif
                _queue.AddFirst(statement);
                return statement;
            }
            return null;
        }
#if NET6_0_OR_GREATER
        public Statement? ReadNextStatement()
#else
        public Statement ReadNextStatement()
#endif
        {
            if (_queue.Count > 0)
            {
                Statement statement = _queue.First.Value;
                _queue.RemoveFirst();
                return statement;
            }
            if (_statementStream != null)
            {
#if NET6_0_OR_GREATER
                Statement? statement = _statementStream.ReadNextStatement();
#else
                Statement statement = _statementStream.ReadNextStatement();
#endif
                return statement;
            }
            return null;
        }
    }
}
