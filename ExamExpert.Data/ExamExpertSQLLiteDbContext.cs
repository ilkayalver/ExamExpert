using ExamExpert.Data.Entities;
using ExamExpert.Data.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data
{
    public class ExamExpertSQLLiteDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }

        public ExamExpertSQLLiteDbContext() : base()
        {

        }

        public ExamExpertSQLLiteDbContext(DbContextOptions<ExamExpertSQLLiteDbContext> options) : base(options)
        {

        } //If more than one different db contexts, DbContextOptions extension must be used for migration folders of EF.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExamMap());
            modelBuilder.ApplyConfiguration(new QuestionMap());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ISqlLiteBaseMap).Assembly);

            modelBuilder.Entity<Role>().HasData(new List<Role>()
            {
                new Role(){ Id=1, Name="Admin", NormalizedName="ADMIN", CreateDate=DateTime.Today },
                new Role(){ Id=2, Name="Teacher", NormalizedName="TEACHER", CreateDate=DateTime.Today },
                new Role(){ Id=3, Name="Student", NormalizedName="STUDENT", CreateDate=DateTime.Today }
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
