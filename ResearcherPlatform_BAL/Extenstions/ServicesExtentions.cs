using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResearchersPlatform_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Extenstions
{
    public static class ServicesExtentions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
                                               IConfiguration configuration)
         => services.AddDbContext<AppDbContext>(opts =>
                                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")
                                    , b => b.MigrationsAssembly("ResearchersPlatform")));
    }
}
