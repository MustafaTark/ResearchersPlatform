using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Configurations
{
    public static class ExtentionModelBuilder
    {
      

        public static void AddIndexes(this ModelBuilder builder)
        {
            builder.Entity<Topic>().HasIndex(p => p.MinmumPoints);
            builder.Entity<Researcher>().HasIndex(r => r.Points);
            builder.Entity<Researcher>().HasIndex(r => r.StudentId).IsUnique();
            builder.Entity<Topic>().HasIndex(n => n.Name).IsUnique();
            builder.Entity<Skill>().HasIndex(n => n.Name).IsUnique();
            builder.Entity<Specality>().HasIndex(n => n.Name).IsUnique();
        }
        public static void AddInhertanceTables(this ModelBuilder builder) 
        {
            builder.Entity<User>().UseTptMappingStrategy().ToTable("Users");
            builder.Entity<Student>()
                .ToTable("Students").HasBaseType<User>();
            builder.Entity<FinalQuiz>()
                 .ToTable("FinalQuizzes").HasBaseType<Quiz>();
            builder.Entity<SectionQuiz>()
                 .ToTable("SectionQuizzes").HasBaseType<Quiz>();
            //builder.Entity<Response>().HasOne(r => r.Problem).WithOne(p => p.Response);

        }
        public static void AddManyToManyTables(this ModelBuilder builder)
        {
            builder.Entity<Student>().HasMany(r => r.Courses).WithMany(c => c.Students);
            builder.Entity<Student>().HasMany(s => s.Badges).WithMany(b => b.Students);
        }
        public static void AddOneToManyRelationship(this ModelBuilder builder)
        {
            builder.Entity<Idea>().HasOne(p => p.ResearcherCreator).WithMany(p => p.IdeasLeader);
            builder.Entity<ExpertRequest>().HasOne(i => i.IdeaObject).WithMany(r => r.ExpertRequests);

        }

    }
}
