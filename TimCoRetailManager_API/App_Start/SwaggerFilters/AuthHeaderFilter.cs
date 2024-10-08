﻿using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TimCoRetailManager_API.App_Start.SwaggerFilters
{
    public class AuthHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter { name = "authorization", @in = "header", description = "access token", required = false, type = "string", @default = "bearer " });
        }
    }
}