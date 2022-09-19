using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CongestionTaxCalculatorWebApi.Filters
{
    public class AddHeaderOperationFilter : IOperationFilter
    {
        private readonly string _parameterName;
        private readonly string _description;
        private readonly string _controllerName;

        public AddHeaderOperationFilter(string description, string parameterName, string controllerName)
        {
            _description = description;
            _parameterName = parameterName;
            _controllerName = controllerName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (context.MethodInfo.DeclaringType.Name.Equals(_controllerName))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Description = _description,
                    Name = _parameterName,
                    In = ParameterLocation.Header,
                    Required = context.MethodInfo.DeclaringType.Name.Equals(_controllerName),
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}
