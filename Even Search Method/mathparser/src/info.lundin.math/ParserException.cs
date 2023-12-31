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
using System.Text;

namespace info.lundin.math
{
    /// <summary>
    /// Exception class for parser related exceptions
    /// </summary>
    public class ParserException : System.Exception
    {
        public ParserException()
            : base()
        {

        }

        public ParserException(string message)
            : base(message)
        {

        }

        public ParserException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
