using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Student> Students {  get; set; }
        public DbSet<Researcher> Researchers { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<SectionQuiz> SectionQuizzes { get; set; }
        public DbSet<FinalQuiz> FinalQuizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Specality> Specalities { get; set; }
        public DbSet<TaskIdea> Tasks { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Invitation> Invitations{ get; set; }
        public DbSet<Notification> Notifications{ get; set; }
        public DbSet<Request> Requests{ get; set; }
        public DbSet<Paper> Papers{ get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<QuizResults> QuizResults { get; set; }
        public DbSet<StudentQuizTrails> StudentQuizTrails { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfigrations());
            builder.Entity<User>().UseTptMappingStrategy().ToTable("Users");
            builder.Entity<Student>()
                .ToTable("Students").HasBaseType<User>();

            //builder.Entity<Researcher>()
            //    .ToTable("Researchers").HasBaseType<Student>();
           
            builder.Entity<FinalQuiz>()
                .ToTable("FinalQuizzes").HasBaseType<Quiz>();
            builder.Entity<SectionQuiz>()
                 .ToTable("SectionQuizzes").HasBaseType<Quiz>();

            builder.Entity<Researcher>().HasMany(p => p.Ideas).WithMany(p => p.Participants);
            builder.Entity<Researcher>().HasMany(p => p.Tasks).WithMany(p => p.Participants);
            builder.Entity<Researcher>().HasMany(r => r.Notifications).WithMany(n => n.Researchers);
            builder.Entity<Researcher>().HasMany(r => r.Invitations).WithMany(i => i.Researchers);

            builder.Entity<Student>().HasMany(r => r.Courses).WithMany(c => c.Students);
            builder.Entity<Student>().HasMany(s => s.Badges).WithMany(b => b.Students);
            builder.Entity<Idea>().HasOne(p => p.ResearcherCreator).WithMany(p => p.IdeasLeader);

            builder.Entity<Researcher>().HasIndex(r => r.Points);
            builder.Entity<Researcher>().HasIndex(r => r.StudentId).IsUnique();



        }
    }
}
