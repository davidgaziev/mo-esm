﻿/*
 * Author: Patrik Lundin, patrik@lundin.live
 * Web: http://www.lundin.live
 * 
 * Source code released under the Microsoft Public License (Ms-PL) 
 * http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace info.lundin.math
{
    /// <summary>
    /// Enumeration of value types
    /// </summary>
    public enum ValueType
    {
        Invalid,
        String,
        Constant
    }

    /// <summary>
    /// Base class for values
    /// </summary>
    /// <remarks>see derived types</remarks>
    [Serializable]
    public abstract class Value
    {
        public ValueType Type { get; internal set; }

        public override abstract string ToString();

        public abstract string ToString(IFormatProvider format);

        public abstract double ToDouble();

        public abstract double ToDouble(IFormatProvider format);

        public abstract void SetValue(string value);

        public abstract void SetValue(string value, IFormatProvider format);

        public abstract void SetValue(double value);

        public abstract void SetValue(double value, IFormatProvider format);
    }
}
