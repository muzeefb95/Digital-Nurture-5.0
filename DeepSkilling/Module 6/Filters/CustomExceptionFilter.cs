using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi_Handson.Filters
{
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string message = $"[{DateTime.Now}] Exception: {context.Exception.Message}\nStackTrace: {context.Exception.StackTrace}\n";
            File.AppendAllText("custom_exceptions.log", message);

            context.Result = new ObjectResult(new { Message = "An unexpected error occurred on the server.", Details = context.Exception.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
