using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI3_1.Filters
{
    public class AddHeaderAttribute : ResultFilterAttribute
    {
        readonly string _name;
        readonly string _value;

        public AddHeaderAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(_name, new string[] { _value, "Method: OnResultExecuting" });
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {   
            //below code will give error response has been prepared, now we cannot modify the header
            //context.HttpContext.Response.Headers.Add(_name, new string[] { _value, "Method: OnResultExecuted" });
            base.OnResultExecuted(context);
        }

    }
}