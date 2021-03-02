using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Extensions;
using PaymentGateway.API.Mappers;
using PaymentGateway.API.Middleware;
using PaymentGateway.API.Models;
using PaymentGateway.Application;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using PaymentGateway.Persistence;

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
            services.AddScoped<IBankClient, DummyBankClient>();
            services.AddScoped<IMapper<BankPaymentResponse, PaymentResult>, PaymentResponseMapper>();
            services.AddScoped<IMapper<PaymentRequest, BankPaymentRequest>, PaymentRequestMapper>();
            services.AddScoped<IMapper<(int, ProcessPaymentDto), PaymentRequest>, ProcessPaymentMapper>();
            services.AddScoped<IProcessPaymentService, ProcessPaymentService>();
            services.AddScoped<IMapper<PaymentRequest, PaymentDetailsDto>, PaymentDetailsMapper>();

            services.AddScoped<IMerchantsRepository, DummyMerchantsRepository>();
            services.AddScoped<IPaymentsRepository, DummyPaymentsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();
                    string errorInformation = context.GetErrorInformation();
                    logger.LogError(exceptionHandlerPathFeature?.Error, errorInformation);

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected error happened");
                });
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<EnableBufferingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
