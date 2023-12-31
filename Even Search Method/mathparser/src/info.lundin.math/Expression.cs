﻿/*
 * Author: Patrik Lundin, patrik@lundin.live
 * Web: http://www.lundin.live
 * 
 * Source code released under the Microsoft Public License (Ms-PL) 
 * http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace info.lundin.math
{
    /// <summary>
    /// Encapsulates the parsed expression tree and
    /// provides methods for serialization
    /// </summary>
    [Serializable]
    public class Expression
    {
        private Node tree;

        /// <summary>
        /// Creates instance
        /// </summary>
        public Expression()
        {

        }

        /// <summary>
        /// Creates instance using the Node as the expression tree
        /// </summary>
        /// <param name="tree">expression tree of Node instances</param>
        public Expression(Node tree)
        {
            this.tree = tree;
        }

        /// <summary>
        /// Serializes the expression to the stream
        /// </summary>
        /// <param name="stream">stream to write to</param>
        public void Save(Stream stream)
        {
            var bin = new BinaryFormatter();
            bin.Serialize(stream, ExpressionTree);
        }

        /// <summary>
        /// Attempts to load a serialized expression from the stream
        /// </summary>
        /// <param name="stream">stream to read from</param>
        public void Load(Stream stream)
        {
            var bin = new BinaryFormatter();
            Node tree = bin.Deserialize(stream) as Node;
            if (tree != null) ExpressionTree = tree;
        }

        /// <summary>
        /// Provides access to the expression tree
        /// </summary>
        public Node ExpressionTree 
        {
            get { return tree; }
            set { tree = value; }
        }
    }
}
