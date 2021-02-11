using System.IO;
using System;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartSchool.WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SmartSchool.WebAPI
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );
            
            
            
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            //Para criar o AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            //integração com IRepository
            services.AddScoped<IRepository, Repository>();


            //Criar Versionamentos pa a API
            services.AddVersionedApiExplorer(
                opt =>
                {
                    opt.GroupNameFormat = "'v'VVV";
                    opt.SubstituteApiVersionInUrl = true;
                })
            .AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1,0);
                opt.ReportApiVersions = true;
            });

            //Usar um ForEch para pegar a versão da API
            var apiProviderDescricao = services.BuildServiceProvider()
                                                .GetService<IApiVersionDescriptionProvider>();
            


            //Criar o Swagger
            services.AddSwaggerGen(
                options =>{
                    foreach (var description in apiProviderDescricao.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(
                            description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                            {
                                Title = "SmartSchool API",
                                Version = description.ApiVersion.ToString(),
                                TermsOfService = new Uri("http://TermosDeUso.com.br"),
                                Description = "Sistema para registros de alunos e professores.",
                                License = new Microsoft.OpenApi.Models.OpenApiLicense{
                                    Name = "SmartSchool License",
                                    Url = new Uri("http://mit.com")
                                },
                                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                                {
                                    Name = "Daniel Ferreira",
                                    Email = "daniel.i.ferreira@outlook.com",
                                    Url = new Uri ("http://programador.net.com")
                                }
                            });

                        
                    }
                    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    options.IncludeXmlComments(xmlCommentsFullPath);
                    
                }
            );
            services.AddCors();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiProviderDescricao)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin()
                .AllowAnyMethod().AllowAnyHeader());
            app.UseSwagger()
                .UseSwaggerUI(options =>{
                    foreach (var description in apiProviderDescricao.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
                        description.GroupName.ToUpperInvariant());
                        
                    }
                    options.RoutePrefix = "";
                });


            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
