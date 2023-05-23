using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.ErrorHandling
{
    public class IdentifiedException : Exception
    {

        public HttpStatusCode? StatusCode;
        public string? Detail;

        public IdentifiedException()
        {
        }

        public IdentifiedException(string message, string? detail = null)
            : base(message)
        {
            Detail = detail;
        }

        public IdentifiedException(string message, HttpStatusCode statusCode, string? detail = null)
            : base(message)
        {
            StatusCode = statusCode;
            Detail = detail;
        }

        public IdentifiedException(string message, Exception inner, string? detail = null)
            : base(message, inner)
        {
            Detail = detail;
        }

        public IdentifiedException(string message, HttpStatusCode statusCode, Exception inner, string? detail = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
            Detail = detail;
        }
    }
}
