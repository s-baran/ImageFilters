using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilters
{
    public class UnsupportedImageFormatException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedImageFormatException"/> class.
        /// </summary>
        public UnsupportedImageFormatException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedImageFormatException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// 
        public UnsupportedImageFormatException(string message) :
            base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedImageFormatException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// <param name="paramName">Name of the invalid parameter.</param>
        /// 
        public UnsupportedImageFormatException(string message, string paramName) :
            base(message, paramName)
        { }
    }
     
}
