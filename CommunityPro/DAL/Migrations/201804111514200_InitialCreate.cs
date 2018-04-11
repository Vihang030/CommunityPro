namespace CommunityPro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CP.ApplicantImage",
                c => new
                    {
                        ApplicantImageID = c.Int(nullable: false),
                        imageContent = c.Binary(),
                        imageMimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ApplicantImageID)
                .ForeignKey("CP.Applicant", t => t.ApplicantImageID, cascadeDelete: true)
                .Index(t => t.ApplicantImageID);
            
            CreateTable(
                "CP.Applicant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Phone = c.Long(nullable: false),
                        EMail = c.String(nullable: false, maxLength: 255),
                        QualificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("CP.Qualification", t => t.QualificationID)
                .Index(t => t.QualificationID);
            
            CreateTable(
                "CP.Application",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicantID = c.Int(nullable: false),
                        PostingID = c.Int(nullable: false),
                        Comments = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("CP.Applicant", t => t.ApplicantID)
                .ForeignKey("CP.Posting", t => t.PostingID)
                .Index(t => t.ApplicantID)
                .Index(t => t.PostingID);
            
            CreateTable(
                "CP.aFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 500),
                        ApplicantID = c.Int(nullable: false),
                        ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("CP.Application", t => t.ApplicationID)
                .ForeignKey("CP.Applicant", t => t.ApplicantID, cascadeDelete: true)
                .Index(t => t.ApplicantID)
                .Index(t => t.ApplicationID);
            
            CreateTable(
                "CP.FileContent",
                c => new
                    {
                        FileIContentD = c.Int(nullable: false),
                        Content = c.Binary(),
                        MimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.FileIContentD)
                .ForeignKey("CP.aFile", t => t.FileIContentD, cascadeDelete: true)
                .Index(t => t.FileIContentD);
            
            CreateTable(
                "CP.Posting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PositionID = c.Int(nullable: false),
                        FTEType = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOpen = c.Int(nullable: false),
                        Status = c.String(maxLength: 9),
                        StartDate = c.DateTime(),
                        ClosingDate = c.DateTime(nullable: false),
                        Details = c.String(nullable: false),
                        Salary = c.Int(nullable: false),
                        SalaryTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("CP.Position", t => t.PositionID)
                .ForeignKey("CP.SalaryType", t => t.SalaryTypeID)
                .Index(t => t.PositionID)
                .Index(t => t.SalaryTypeID);
            
            CreateTable(
                "CP.Position",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobCode = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Summary = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.JobCode, unique: true, name: "IX_Unique_JobCode");
            
            CreateTable(
                "CP.Qualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DegreeName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "CP.Skill",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SkillName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.SkillName, unique: true, name: "IX_Unique_Skill");
            
            CreateTable(
                "CP.SalaryType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Salarytype = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "CP.School",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "CP.QualificationPosition",
                c => new
                    {
                        Qualification_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Qualification_ID, t.Position_ID })
                .ForeignKey("CP.Qualification", t => t.Qualification_ID, cascadeDelete: true)
                .ForeignKey("CP.Position", t => t.Position_ID, cascadeDelete: true)
                .Index(t => t.Qualification_ID)
                .Index(t => t.Position_ID);
            
            CreateTable(
                "CP.QualificationPosting",
                c => new
                    {
                        Qualification_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Qualification_ID, t.Posting_ID })
                .ForeignKey("CP.Qualification", t => t.Qualification_ID, cascadeDelete: true)
                .ForeignKey("CP.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.Qualification_ID)
                .Index(t => t.Posting_ID);
            
            CreateTable(
                "CP.SkillApplicant",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Applicant_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Applicant_ID })
                .ForeignKey("CP.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("CP.Applicant", t => t.Applicant_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Applicant_ID);
            
            CreateTable(
                "CP.SkillPosition",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Position_ID })
                .ForeignKey("CP.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("CP.Position", t => t.Position_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Position_ID);
            
            CreateTable(
                "CP.SkillPosting",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Posting_ID })
                .ForeignKey("CP.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("CP.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Posting_ID);
            
            CreateTable(
                "CP.SchoolPosting",
                c => new
                    {
                        School_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.School_ID, t.Posting_ID })
                .ForeignKey("CP.School", t => t.School_ID, cascadeDelete: true)
                .ForeignKey("CP.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.School_ID)
                .Index(t => t.Posting_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("CP.aFile", "ApplicantID", "CP.Applicant");
            DropForeignKey("CP.SchoolPosting", "Posting_ID", "CP.Posting");
            DropForeignKey("CP.SchoolPosting", "School_ID", "CP.School");
            DropForeignKey("CP.Posting", "SalaryTypeID", "CP.SalaryType");
            DropForeignKey("CP.SkillPosting", "Posting_ID", "CP.Posting");
            DropForeignKey("CP.SkillPosting", "Skill_ID", "CP.Skill");
            DropForeignKey("CP.SkillPosition", "Position_ID", "CP.Position");
            DropForeignKey("CP.SkillPosition", "Skill_ID", "CP.Skill");
            DropForeignKey("CP.SkillApplicant", "Applicant_ID", "CP.Applicant");
            DropForeignKey("CP.SkillApplicant", "Skill_ID", "CP.Skill");
            DropForeignKey("CP.QualificationPosting", "Posting_ID", "CP.Posting");
            DropForeignKey("CP.QualificationPosting", "Qualification_ID", "CP.Qualification");
            DropForeignKey("CP.QualificationPosition", "Position_ID", "CP.Position");
            DropForeignKey("CP.QualificationPosition", "Qualification_ID", "CP.Qualification");
            DropForeignKey("CP.Applicant", "QualificationID", "CP.Qualification");
            DropForeignKey("CP.Posting", "PositionID", "CP.Position");
            DropForeignKey("CP.Application", "PostingID", "CP.Posting");
            DropForeignKey("CP.FileContent", "FileIContentD", "CP.aFile");
            DropForeignKey("CP.aFile", "ApplicationID", "CP.Application");
            DropForeignKey("CP.Application", "ApplicantID", "CP.Applicant");
            DropForeignKey("CP.ApplicantImage", "ApplicantImageID", "CP.Applicant");
            DropIndex("CP.SchoolPosting", new[] { "Posting_ID" });
            DropIndex("CP.SchoolPosting", new[] { "School_ID" });
            DropIndex("CP.SkillPosting", new[] { "Posting_ID" });
            DropIndex("CP.SkillPosting", new[] { "Skill_ID" });
            DropIndex("CP.SkillPosition", new[] { "Position_ID" });
            DropIndex("CP.SkillPosition", new[] { "Skill_ID" });
            DropIndex("CP.SkillApplicant", new[] { "Applicant_ID" });
            DropIndex("CP.SkillApplicant", new[] { "Skill_ID" });
            DropIndex("CP.QualificationPosting", new[] { "Posting_ID" });
            DropIndex("CP.QualificationPosting", new[] { "Qualification_ID" });
            DropIndex("CP.QualificationPosition", new[] { "Position_ID" });
            DropIndex("CP.QualificationPosition", new[] { "Qualification_ID" });
            DropIndex("CP.Skill", "IX_Unique_Skill");
            DropIndex("CP.Position", "IX_Unique_JobCode");
            DropIndex("CP.Posting", new[] { "SalaryTypeID" });
            DropIndex("CP.Posting", new[] { "PositionID" });
            DropIndex("CP.FileContent", new[] { "FileIContentD" });
            DropIndex("CP.aFile", new[] { "ApplicationID" });
            DropIndex("CP.aFile", new[] { "ApplicantID" });
            DropIndex("CP.Application", new[] { "PostingID" });
            DropIndex("CP.Application", new[] { "ApplicantID" });
            DropIndex("CP.Applicant", new[] { "QualificationID" });
            DropIndex("CP.ApplicantImage", new[] { "ApplicantImageID" });
            DropTable("CP.SchoolPosting");
            DropTable("CP.SkillPosting");
            DropTable("CP.SkillPosition");
            DropTable("CP.SkillApplicant");
            DropTable("CP.QualificationPosting");
            DropTable("CP.QualificationPosition");
            DropTable("CP.School");
            DropTable("CP.SalaryType");
            DropTable("CP.Skill");
            DropTable("CP.Qualification");
            DropTable("CP.Position");
            DropTable("CP.Posting");
            DropTable("CP.FileContent");
            DropTable("CP.aFile");
            DropTable("CP.Application");
            DropTable("CP.Applicant");
            DropTable("CP.ApplicantImage");
        }
    }
}
