﻿using MyExtensions;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace MyClassLibrary.ErrorHandling
{
    public class ApiErrorResponse<T>
    {
        private readonly ILogger<T> _logger;

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }

        private Exception? InnerException { get; set; }

        public string Body
        {
            get
            {
                string output = $"Message: {Message}\nDetail: {Detail}.";
                return output;
            }
        }

        public ApiErrorResponse(Exception exception, ILogger<T> logger)
        {
            _logger = logger;

            string exceptionType = exception.GetType().ToString();
            string identifiedExceptionType = new IdentifiedException().GetType().ToString();    

            if (exceptionType == identifiedExceptionType)
            {
                IdentifiedException identifiedException = (IdentifiedException)exception;

                StatusCode = identifiedException.StatusCode ?? HttpStatusCode.InternalServerError;
                Message = identifiedException.Message ?? "Unknown Error";

                Detail = identifiedException.Detail ?? identifiedException.InnerException?.Message ?? "None";

                InnerException = identifiedException.InnerException;
            }
            else
            {
                StatusCode = HttpStatusCode.InternalServerError;

                Message = $"An error of type {exceptionType} has occured.";
                Detail = exception.Message;

                InnerException = exception;
            }


        }



        private void LogError()
        {
            if (StatusCode == HttpStatusCode.BadRequest ||
                StatusCode == HttpStatusCode.Unauthorized ||
                StatusCode == HttpStatusCode.NotFound)
            {
            }
            else
            {
                _logger.LogError(InnerException, "{StatusCode} - {Message}", StatusCode, Message);
            }
        }
    }
}
