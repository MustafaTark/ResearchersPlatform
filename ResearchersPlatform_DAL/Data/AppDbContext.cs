﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResearchersPlatform_DAL.Configurations;
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
        public DbSet<RequestIdea> Requests{ get; set; }
        public DbSet<Paper> Papers{ get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<QuizResults> QuizResults { get; set; }
        public DbSet<StudentQuizTrails> StudentQuizTrails { get; set; }
        public DbSet<IdeaMessage> IdeaMessages { get; set; }
        public DbSet<TaskMessage> TaskMessages { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemCategory> ProblemCategories { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<ExpertRequest> ExpertRequests { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<IdeaFile> IdeaFiles { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfigrations());
            builder.ApplyConfigurationsFromAssembly(typeof(ResearcherEntityTypeConfigurations).Assembly);
            builder.AddIndexes();
            builder.AddInhertanceTables();
            builder.AddManyToManyTables();
            builder.AddOneToManyRelationship();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}
    }
}
