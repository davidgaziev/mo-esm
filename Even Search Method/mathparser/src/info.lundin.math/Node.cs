﻿/*
 * Author: Patrik Lundin, patrik@lundin.live
 * Web: http://www.lundin.live
 * 
 * Source code released under the Microsoft Public License (Ms-PL) 
 * http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace info.lundin.math
{
    /// <summary>
    /// Types of Node(s)
    /// </summary>
    public enum NodeType
    {
        Undefined,
        Expression,
        Variable,
        Value
    }

    /// <summary>
    /// Class Node, represents a Node in a tree data structure representation of a mathematical expression.
    /// </summary>
    [Serializable]
    public sealed class Node
    {
        [NonSerialized]
        private Operator op;

       // [NonSerialized]
        private Value value;

        [NonSerialized]
        private Value restore;

        private Node arg1 = null;
        private Node arg2 = null;
        private int args = 0;
        private NodeType type;
        private string variable;
        private string op_symbol;

        /// <summary>
        /// Creates a Node containing the specified Operator and arguments.
        /// This will automatically mark this Node as a TYPE_EXPRESSION
        /// </summary>
        /// <param name="_operator">the string representing an operator</param>
        /// <param name="_arg1">the first argument to the specified operator</param>
        /// <param name="_arg2">the second argument to the specified operator</param>
        internal Node(Operator op, Node arg1, Node arg2)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.op = op;
            this.args = 2;
            this.op_symbol = op.Symbol;

            this.type = NodeType.Expression;
        }

        /// <summary>
        /// Creates a Node containing the specified Operator and argument.
        /// This will automatically mark this Node as a TYPE_EXPRESSION
        /// </summary>
        /// <param name="_operator">the string representing an operator</param>
        /// <param name="_arg1">the argument to the specified operator</param>
        internal Node(Operator op, Node arg1)
        {
            this.arg1 = arg1;
            this.op = op;
            this.args = 1;
            this.op_symbol = op.Symbol;

            this.type = NodeType.Expression;
        }

        /// <summary>
        /// Creates a Node containing the specified variable.
        /// This will automatically mark this Node as a TYPE_VARIABLE
        /// </summary>
        /// <param name="variable">the string representing a variable</param>
        internal Node(string variable)
        {
            this.variable = variable;
            this.type = NodeType.Variable;
        }

        /// <summary>
        /// Creates a Node containing the specified value.
        /// This will automatically mark this Node as a TYPE_CONSTANT
        /// </summary>
        /// <param name="value">the value for this Node</param>
        internal Node(double value)
        {
            this.value = new DoubleValue() { Value = value };
            this.type = NodeType.Value;
        }

        /// <summary>
        /// Returns the String operator of this Node 
        /// </summary>
        internal Operator Operator
        {
            get { return this.op; }
        }

        /// <summary>
        /// Gets or sets the value of this Node 
        /// </summary>
        internal Value Value
        {
           get { return this.value; }
           set { this.value = value; }
        }

        /// <summary>
        /// Returns the String variable of this Node 
        /// </summary>
        internal String Variable
        {
            get { return (this.variable); }
        }

        /// <summary>
        /// Returns the number of arguments this Node has
        /// </summary>
        internal int Arguments
        {
            get { return (this.args); }
        }

        /// <summary>
        /// Returns the node type
        /// </summary>
        internal NodeType Type
        {
            get { return (this.type); }
        }

        /// <summary>
        /// Returns the first argument of this Node
        /// </summary>
        internal Node FirstArgument
        {
            get { return this.arg1; }
        }

        /// <summary>
        /// Returns the second argument of this Node
        /// </summary>
        internal Node SecondArgument
        {
            get { return this.arg2; }
        }

        [OnSerializing]
        public void OnSerializing(StreamingContext ctx)
        {
            if (Type != NodeType.Value)
            {
                restore = value;
                value = null;
            }
        }

        [OnSerialized]
        public void OnSerializedMethod(StreamingContext context)
        {
            if (Type != NodeType.Value && restore != null)
            {
                value = restore;
                restore = null;
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext ctx)
        {
            var oplist = new DefaultOperators();
            this.op = oplist.Operators.SingleOrDefault(o => o.Symbol == op_symbol);
        }
        
    } // End class Node
}
