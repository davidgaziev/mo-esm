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
    /// Dictionary of expressions for caching expressions
    /// </summary>
    public class ExpressionDictionary : IDictionary<string, Expression>
    {
        private IDictionary<string, Expression> dictionary;

        /// <summary>
        /// Creates dictionary
        /// </summary>
        public ExpressionDictionary()
        {
            dictionary = new Dictionary<string, Expression>();
        }

        /// <summary>
        /// Serializes the expression to the stream
        /// </summary>
        /// <param name="stream">stream to write to</param>
        public void Save(Stream stream)
        {
            var bin = new BinaryFormatter();
            bin.Serialize(stream, dictionary);
        }

        /// <summary>
        /// Attempts to load a serialized expression from the stream
        /// </summary>
        /// <param name="stream">stream to read from</param>
        public void Load(Stream stream)
        {
            var bin = new BinaryFormatter();

            IDictionary<string, Expression> obj
                = bin.Deserialize(stream) as IDictionary<string, Expression>;

            if (obj != null) dictionary = obj;
        }

        /// <summary>
        /// Adds an expression to the dictionary with the key
        /// </summary>
        /// <param name="key">key to use</param>
        /// <param name="value">expression to add</param>
        public void Add(string key, Expression value)
        {
            dictionary.Add(key, value);
        }

        #region Standard Dictionary Method Implementations

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out Expression value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public ICollection<Expression> Values
        {
            get { return dictionary.Values; }
        }

        public Expression this[string key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, Expression> item)
        {
            dictionary.Add(item);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, Expression> item)
        {
            return dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Expression>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return dictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, Expression> item)
        {
            return dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, Expression>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        #endregion
    }
}
