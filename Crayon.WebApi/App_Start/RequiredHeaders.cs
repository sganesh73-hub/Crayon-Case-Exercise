using System.Collections.Generic;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using WebActivatorEx;


namespace Crayon.WebApi
{
    /// <summary>
    /// Operation filter to add the requirement of the custom header
    /// </summary>
    public class RequiredHeaders : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry,
            ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            if (apiDescription.RelativePath.Contains("ExchangeRate"))
            {
                operation.parameters.Add(new Parameter
                {
                    name = "AuthToken",
                    @in = "header",
                    type = "string",
                    required = true,
                    description = "The token is needed to access the resource :AuthToken"
                });
            }
        }
    }

}
