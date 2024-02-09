using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public ValidationFilterAttribute()
        {                
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var aciton = context.RouteData.Values["action"];
            var contoller = context.RouteData.Values["controller"];

            var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("DTO")).Value;

            if(param is null)
            {
                context.Result = new BadRequestObjectResult($"Sent object in controller: {contoller} and action: {aciton} is null");
                return;
            }

            if(!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
}
