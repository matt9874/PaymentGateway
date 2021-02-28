using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.API.Mappers;
using PaymentGateway.API.Models;
using PaymentGateway.Application;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain;

namespace PaymentGateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;

            }).ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();
                    ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                        context.HttpContext, 
                        context.ModelState);

                    problemDetails.Detail = "Errors field contains information on problem";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    ActionExecutingContext actionExecutingContext =  context as ActionExecutingContext;
                    int errorCount = context.ModelState.ErrorCount;
                    int? actionParametersCount = actionExecutingContext?.ActionArguments.Count;
                    int expectedActionParametersCount = context.ActionDescriptor.Parameters.Count;

                    if (errorCount > 0 && actionParametersCount == expectedActionParametersCount)
                    {
                        problemDetails.Title = "One or more validation errors occurred.";
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;

                        return new UnprocessableEntityObjectResult(problemDetails)
                        { 
                            ContentTypes = {"application/problem+json" }
                        };
                    }

                    problemDetails.Title = "One or more errors in input request.";
                    problemDetails.Status = StatusCodes.Status400BadRequest;

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
