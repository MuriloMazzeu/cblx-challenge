using CblxChallenge.Domain.Application;
using CblxChallenge.Domain.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using FluentValidation.AspNetCore;
using CblxChallenge.Domain.Entities;

namespace CblxChallenge
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IReceivedMineralsQueryService, ReceivedMineralsQueryService>();
            services.AddScoped<IFreighterCommandService, FreighterCommandService>();
            services.AddScoped<IReceivedMineralsRepository, FirestoreService>();
            services.AddScoped<IFreighterRepository, FirestoreService>();
            services.AddScoped<ISmk186Service, Smk186Service>();

            var assembly = typeof(FreighterTransportEntity).Assembly;
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);

            services.AddHttpClient();
            services.AddCors(o =>
            {
                o.AddPolicy("allow", b =>
                {
                    b.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddControllers().AddFluentValidation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("allow");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
