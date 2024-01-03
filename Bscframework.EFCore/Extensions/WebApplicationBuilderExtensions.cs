using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bscframework.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bscframework.EFCore.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRepository<TRepositoryInterface, TRepositoryImplementation>(this WebApplicationBuilder builder)
            where TRepositoryInterface : class, IGenericRepository
            where TRepositoryImplementation : class, TRepositoryInterface
        {
            builder.Services.AddScoped<TRepositoryInterface, TRepositoryImplementation>();
            builder.Services.AddScoped<IGenericRepository, TRepositoryImplementation>();
            return builder;
        }

        public static WebApplicationBuilder AddUnitOfWork<TContext>(this WebApplicationBuilder builder)
            where TContext : DbContext
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return builder;
        }

    }
}
