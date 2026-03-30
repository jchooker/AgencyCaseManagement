using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgencyCaseManagement.Web.TagHelpers
{
    [HtmlTargetElement("view-script")]
    public class ViewScriptTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ViewContext]
        public ViewContextAttribute ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var endpoint = httpContext?.GetEndpoint();

            //get from endpoint metada if using attribute routing
            var routePattern = (endpoint as RouteEndpoint)?.RoutePattern.RawText;

            //or from iroutenamemetadata, iactiondescriptor, etc
            var actionDescriptor = endpoint?.Metadata
                .GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();

            var controller = actionDescriptor?.ControllerName?.ToLower();
            var action = actionDescriptor?.ActionName?.ToLower();
        }
    }
}
