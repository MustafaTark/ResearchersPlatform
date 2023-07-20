using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using ResearchersPlatform.Hubs;
using ResearchersPlatform_BAL.Contracts;
using ResearchersPlatform_BAL.Repositories;
using ResearchersPlatform_DAL.Data;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform.Extenstions
{
    public static class ServicesExtentions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
                                               IConfiguration configuration)
         => services.AddDbContext<AppDbContext>(opts =>
                                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")
                                    , b => b.MigrationsAssembly("ResearchersPlatform")));
        public static void ConfigureIdentity<T>(this IServiceCollection services) where T : User
        {
            var authBuilder = services.AddIdentityCore<T>
                (o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 10;
                    o.User.RequireUniqueEmail = true;
                });
            authBuilder = new IdentityBuilder(authBuilder.UserType, typeof(IdentityRole), services);
            authBuilder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt =>
            {
                //opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                    };
                })
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"), "jwtBearerScheme2");

        }
        public static void ConfigureLifeTime(this IServiceCollection services)
        {
            services.AddScoped<User, Student>();
            //services.AddScoped<Student, Researcher>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<GenericRepository<Student>,StudentRepository>();
            services.AddScoped<GenericRepository<Course>,CourseRepository>();
            services.AddScoped<GenericRepository<Researcher>,ResearcherRepository>();
            services.AddScoped<GenericRepository<TaskIdea>,TaskRepository>();
            services.AddScoped<GenericRepository<Idea>,IdeaRepository>();
            services.AddScoped<GenericRepository<SectionQuiz>, SectionQuizRepository>();
            services.AddScoped<GenericRepository<FinalQuiz>, FinalQuizRepository>();
            services.AddScoped<GenericRepository<Section>, SectionRepository>();
            services.AddScoped<GenericRepository<Specality>, SpecialityRepository>();
            services.AddScoped<GenericRepository<Paper>, PaperRepository>();
            services.AddScoped<GenericRepository<Invitation>, InvitationRepository>();
            services.AddScoped<GenericRepository<RequestIdea>, RequestRepository>();
            services.AddScoped<GenericRepository<TaskIdea>, TaskRepository>();
            services.AddScoped<GenericRepository<User>, AdminRepository>();
            services.AddScoped<GenericRepository<Problem>, ProblemRepository>();
            services.AddScoped<GenericRepository<ExpertRequest>, ExpertRequestRepository>();
            services.AddScoped<GenericRepository<Response>, ResponseRepository>();
            

            services.AddScoped<IRepositoryManager,RepositoryManager>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IFilesManager, FilesManager>();
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IStudentRepository,StudentRepository>();
            services.AddScoped<ICourseRepository,CourseRepository>();
            services.AddScoped<IResearcherRepository,ResearcherRepository>();
            services.AddScoped<IIdeaRepository,IdeaRepository>();
            services.AddScoped<ITaskRepository,TaskRepository>();
            services.AddScoped<ISectionQuizRepository, SectionQuizRepository>();
            services.AddScoped<IFinalQuizRepository, FinalQuizRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            services.AddScoped<IPaperRepository , PaperRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<ITaskRepository , TaskRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IProblemRepository, ProblemRepository>();
            services.AddScoped<IExpertRequestRepository, ExpertRequestRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();

            services.AddTransient<IEmailService, EmailService>();

        }
    }
}
