using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI3_1.Filters
{
    public class WeatherActionFilterAttribute : ActionFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        public WeatherActionFilterAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.Add(_name, _value);
            base.OnActionExecuting(context);
        }
    }
}