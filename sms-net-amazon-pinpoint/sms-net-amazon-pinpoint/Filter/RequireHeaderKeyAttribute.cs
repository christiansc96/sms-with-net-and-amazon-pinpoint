using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace sms_net_amazon_pinpoint.Filter
{
    public class RequireHeaderKeyAttribute : ActionFilterAttribute
    {
        private readonly string _headerName;
        private readonly int _requiredLength;

        public RequireHeaderKeyAttribute(string headerName, int requiredLength = 20)
        {
            _headerName = headerName;
            _requiredLength = requiredLength;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_headerName, out var key) ||
                string.IsNullOrEmpty(key) || key.ToString().Length != _requiredLength)
                context.Result = new BadRequestResult();
            base.OnActionExecuting(context);
        }
    }
}