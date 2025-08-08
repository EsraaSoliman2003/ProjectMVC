using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication3.Filters
{
    public class MaintenanceModeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var today = DateTime.Now.DayOfWeek;
            if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
            {
                context.Result = new ViewResult
                {
                    ViewName = "Maintenance"
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

    }
}
