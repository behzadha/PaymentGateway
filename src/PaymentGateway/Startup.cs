using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentGateway.BankGatewayProvider;
using PaymentGateway.BankGatewayProvider.Saman;
using PaymentGateway.DataModel;
using PaymentGateway.DataModel.DomainModel;
using PaymentGateway.Model;

namespace PaymentGateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.            

            services.AddMvc();
            services.AddDbContext<PaymentGatewayContext>(options =>  options.UseSqlServer(Configuration.GetConnectionString("PaymentGatewayConnectionString")));
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRepository<Payment>, PaymentRepository>();
            services.AddTransient<PaymentGateway.Service.ServiceInterface.IPaymentService,Service.ServiceInterface.PaymentService>();

            var samanBankGatewayConfiguration = new SamanBankGatewayConfiguration();
            Configuration.GetSection("SamanGatewayConfiguration-Terminal1").Bind(samanBankGatewayConfiguration);
            services.Configure<ConfigurationSettings>(Configuration.GetSection("ConfigurationSettings"));
            services.AddSingleton<IBankGatewayProvider>(new SamanBankGateway(samanBankGatewayConfiguration));

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info() { Title = "Payment API" , Version = "v1"}));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PaymentGatewayContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSwagger();
            //  app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","Payment API"));
         //   app.UseSwaggerUI(c=>c.SwaggerEndpoint());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            DbInitializer.Initialize(context);
        }
    }
}
