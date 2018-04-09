using CommunityPro.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CommunityPro.DAL
{
    public class CommunityProEntities : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Posting> Postings { get; set; }
        public DbSet<Position> Postions { get; set; }
        public DbSet<SalaryType> SalaryTypes { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<aFile> Files { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<ApplicantImage> ApplicantImages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //This option keeps table names in singular form, my personal preference.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.HasDefaultSchema("CP");

            //Added for cascade delte for all Files with Applicant
            modelBuilder.Entity<Applicant>()
                .HasMany(a => a.Files)
                .WithRequired(p => p.Applicant)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Applicant>()
                .HasOptional(w => w.ApplicantImage)
                .WithRequired(p => p.Applicant)
                .WillCascadeOnDelete(true);


            //Added for cascade delete for File Content with File
            modelBuilder.Entity<aFile>()
                .HasOptional(w => w.FileContent)
                .WithRequired(p => p.aFile)
                .WillCascadeOnDelete(true);
        }

        
    }
}