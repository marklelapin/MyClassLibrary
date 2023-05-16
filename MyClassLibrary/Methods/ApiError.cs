using MyExtensions;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyClassLibrary.Methods
{
    public class ApiError
    {
        public int StatusCode { get; set; } = 500;
        public string Type { get; set; }
        public string Message { get;set; }
        public string ExceptionMessage { get; set; } = string.Empty;
        public string Help { get;set; } = string.Empty;
        
        public string Response()
        {
            return JsonSerializer.Serialize(this);

        }
        
        public ApiError( HttpStatusCode statusCode,string type, string message,string help ="",Exception? exception=null)
        {
            Type = type;
            Message = message;
            StatusCode = (int)statusCode; 
            if (exception != null) { ExceptionMessage = exception.Message ?? string.Empty; }
            Help = help;       
        }




    }
}
