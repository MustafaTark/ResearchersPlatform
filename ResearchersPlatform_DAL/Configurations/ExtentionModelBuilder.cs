using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        }
        public static void AddManyToManyTables(this ModelBuilder builder)
        {
            builder.Entity<Student>().HasMany(r => r.Courses).WithMany(c => c.Students);
            builder.Entity<Student>().HasMany(s => s.Badges).WithMany(b => b.Students);
        }
        public static void AddOneToManyRelationship(this ModelBuilder builder)
        {
            builder.Entity<Idea>().HasOne(p => p.ResearcherCreator).WithMany(p => p.IdeasLeader);

        }

    }
}
