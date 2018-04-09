namespace CommunityPro.DAL.Migrations
{
    using CommunityPro.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<CommunityPro.DAL.CommunityProEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\Migrations";
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
            catch (Exception e)
            {
                throw new Exception(
                     "Seed Failed - errors follow:\n" +
                     e.InnerException.InnerException.Message.ToString(), e
                 ); // Add the original exception as the innerException
            }
        }

        


        protected override void Seed(CommunityPro.DAL.CommunityProEntities context)
        {
            var qualifications = new List<Qualification>
            {
                new Qualification { DegreeName = "degree apprenticeship" },
                new Qualification { DegreeName = "bachelor of the arts" },
                new Qualification { DegreeName = "bachelor of science" },
                new Qualification { DegreeName = "graduate certificate" }
            };
            qualifications.ForEach(d => context.Qualifications.AddOrUpdate(n => n.DegreeName, d));
            SaveChanges(context);

            //var schools = new List<School>
            //{
            //    new School { Name = "Niagara Islamic School" },
            //    new School {Name = "Sacred Heart Catholic Elementary School" },
            //    new School { Name = "Father Hennepin Catholic Elementary School" },
            //    new School { Name = "Saint Michael Catholic High School" },
            //    new School { Name = "Our Lady of Mount Carmel Catholic Elementary School" },
            //    new School { Name = "Loretto Catholic Elementary School" },
            //    new School { Name = "Forestview Public School" },
            //    new School { Name = "Princess Margaret Public School" },
            //    new School { Name = "Pathways Academy and Early Learning Centre" },
            //    new School { Name = "Westlane Secondary School" },
            //    new School { Name = "Greendale Public School" },
            //    new School { Name = "Cardinal Newman Catholic Elementary School" },
            //    new School { Name = "Cherrywood Acres Public School" },
            //    new School { Name = "St. Vincent de Paul Catholic Elementary School " }

            //};
            //schools.ForEach(d => context.Schools.AddOrUpdate(n => n.Name, d));
            //SaveChanges(context);

            var skills = new List<Skill>
            {
                new Skill { SkillName = "Artistic" },
                new Skill {SkillName = "Childcare"},
                new Skill { SkillName ="Grading Exams" },
                new Skill { SkillName = "Group Counseling" },
                new Skill { SkillName = "Improve Study Habits" },
                new Skill { SkillName = "Instruction" },
                new Skill { SkillName = "Microsoft Office" },
                new Skill { SkillName = "Networking" },
                new Skill { SkillName = "Teaching" },
                new Skill { SkillName = "Writing Lesson Plans" },
                new Skill { SkillName = "Parent Communications" },
                new Skill { SkillName = "Management" },
                new Skill { SkillName = "Scheduling"}

            };
            skills.ForEach(d => context.Skills.AddOrUpdate(n => n.SkillName, d));
            SaveChanges(context);



            var salarytype = new List<SalaryType>
            {
                new SalaryType { Salarytype = "Per Hour" },
                new SalaryType { Salarytype = "Per Month" },
                new SalaryType { Salarytype = "Per Year" }
            };
            salarytype.ForEach(d => context.SalaryTypes.AddOrUpdate(n => n.Salarytype, d));
            SaveChanges(context);


            var positions = new List<Position>
            {
                new Position {  Name="Computer Teacher",JobCode=1004, Summary="The ideal candidates are flexible and experienced teachers who can support students to achieve their best, and master valuable skills for the international work environment. Successful candidates will teach a variety of concepts, with an emphasis on student-centered learning and class participation."
                },
                new Position { JobCode=1244, Name="IT technician", Summary="The Academy for Mathematics & English tutors students from SK to grade 12 for Math, Grades K to Grade 9 in English, and grades 11 & 12 in Physics and Chemistry. We are currently recruiting Enthusiastic & Knowledgeable candidates to be our tutors at Mississauga location to work with younger children (JK � Grade 4)."},
                new Position { JobCode=2444, Name=".Net Developer", Summary="A Senior .NET Developer is responsible for playing a central role in the design, development, and delivery of integration systems and data stores. Also, responsible for developing mature code to support product owners, stakeholders and other IT team members."},
                new Position { JobCode=3434, Name="Math Instructor", Summary="Responsible Teacher binder and electronic copy. April 02-June 27(will lead to more employment upon completion of module)...."},
                new Position { JobCode=4434, Name="Suzuki Viola Instructor", Summary="The Royal Conservatory School is seeking a group and individual Suzuki Viola Instructor. Reporting to the Dean, The Royal Conservatory School, the Suzuki Viola Instructor is responsible for providing high quality viola instruction to students ages 4+ individually and in a group setting. The ideal candidate will have 4+ years teaching experience and will have completed Suzuki Level 1, Viola and be registered through the Suzuki Association of the Americas."},
                new Position { JobCode=5434, Name="Frech teacher position", Summary="Responsible Teacher binder and electronic copy. SCH3U/4U & SNC1D/2D - Chemistry & Science. April 02-June 27(will lead to more employment upon completion of..."},
                new Position { JobCode=6434, Name="Chemistry/Science Teacher", Summary="Responsible Teacher binder and electronic copy. SCH3U/4U & SNC1D/2D - Chemistry & Science. April 02-June 27(will lead to more employment upon completion of..."},
                new Position { JobCode=7434, Name="Kindergarten Teacher", Summary="St. Gregory the Great Catholic School requires a full-time 1.0 FTE Kindergarten Teacher. This is a Temporary contract. This school is located in Blackfalds, AB.Please submit a complete application package which includes a cover letter, resume, pastoral reference and current criminal record check along with three references."},
                new Position { JobCode=8434, Name="Intermediate Classroom Teacher", Summary="St. Augustine School in Kitsilano is hiring an intermediate teacher for September 2019. This is a full-time position. The successful candidate will also assist with the extra-curricular program of the school. Please, send resume to principal@staugustineschool.ca or call the school office at 604-731-8024."},
                new Position { JobCode=9434, Name="Teacher Teaching On Call", Summary="School District No. 52 (Prince Rupert) is located within traditional Ts�msyen Territory on the beautiful north coast of B.C. and services the small communities of Hartley Bay and Port Edward in addition to the city of Prince Rupert. Currently we have one secondary school, one middle school, five elementary schools, one store-front site and one �on reserve� school (Hartley Bay, K-12). Our student population is approximately 2,000 students, 60% of which are aboriginal.Our community boasts an excellent recreation facility (ice arena, swimming pool and gymnasium,) an 18 hole golf course, racquet centre, tennis court, museum and a 700 seat performing art centre."},
                new Position { JobCode=1034, Name="Urdu - International Language Instructor", Summary="Qualified applicants are invited to apply for this vacancy through the Apply to Education (ATE) web-site at www.applytoeducation.com.  Applicants are required to submit a resume detailing education, experience, and qualifications; documentation confirming certifications; and written permission to contact two professional references.  Cover letters may be addressed to Anna Massimiliano, Principal, Adult and Continuing Education.  To view the posting on the ATE web-site, select �Search Jobs�, select �Support Applicants�, then use the search section to look for positions with the Simcoe County District School Board."},
                new Position { JobCode=1134, Name="Grade 1-2 Teacher", Summary="Shihiya School is a band-run school owned and operated by the Splatsin Indian Band. We are located in the interior of British Columbia adjacent to the city of Enderby. We are the southernmost band of the Secwepemc (Shuwap) people. We are a certified FNSA school and are committed to consistency and competency in our programs and approaches. If you value high standards, working collaboratively with a great staff, and opportunities for further educational development; we may be for you."},
                new Position { JobCode=1334, Name="Psychologist/Psychological Associate", Summary="The position will focus mainly on providing psychological consultation and programming support, as well as building the capacity of other Board staff in the areas of developmental disorders, behavioural and mental health issues.  Overall, we are looking for an individual with strong assessment, consultation and intervention skills who has demonstrated leadership to support capacity building of other professional staff, and the ability to work effectively on a multidisciplinary team."},
                new Position { JobCode=1434, Name="Substitute Teacher in Cat Lake", Summary="Windigo Education Authority is looking for a qualified candidate to teach Gr 4 at Lawrence Wesley Education Centre in Cat Lake, ON, for the remainder of the school year.  Applicants must have a Bachelor of Education degree and be a member in good standing with the Ontario College of Teachers or be eligible for membership."},
                new Position { JobCode=1534, Name="French Immersion", Summary="We are adding French teachers to our occasional teacher roster!  Candidates may be also be considered for Long Term assignments.We currently have 13 Elementary Schools offering Early French Immersion (Grade 1) programs and two Secondary Schools offering Extended and French Immersion programs."},
                new Position { JobCode=1634, Name="Temporary Fulltime Senior High Math Teacher", Summary="ADLC is the premier distance learning partner for primary and secondary education in Alberta. Since 1923, ADLC has worked with students, teachers, and administrators to provide distance education at any time, any place, and any pace. Operated by Pembina Hills Public Schools, under a service agreement with Alberta Education, ADLC offers online educational opportunities to students in Grades 1-12.  Duties are to commence as soon as possible and end on June 30, 2019."},
                new Position { JobCode=1734, Name="Grade 3 Teacher", Summary="St. Augustine School is hiring a Grade 3 teacher for September 2019.This is a full - time position.The successful candidate will also assist with the extra - curricular program of the school.Please, send resume to principal@staugustineschool.ca or call the school office at 604 - 731 - 8024."}
            };
            positions.ForEach(d => context.Postions.AddOrUpdate(n => n.Name, d));
            SaveChanges(context);



            var applicants = new List<Applicant>
            {
                new Applicant { FirstName = "Shah", MiddleName = "Rukh",  LastName = "Khan", Phone=2896732136, EMail="srk@outlook.com", QualificationID = 1,
                    Skills = new List<Skill> { new Skill { SkillName="C#" },
                        new Skill { SkillName = "C++"},
                        new Skill { SkillName = "C" } }  },
                new Applicant { FirstName = "Jhon", MiddleName = "T",  LastName = "Trevolta", Phone=2897732136, EMail="jhont@outlook.com", QualificationID = 2,
                  Skills = new List<Skill> { new Skill { SkillName="PHP" } ,
                        new Skill { SkillName = "AngularJS" } } },
                new Applicant { FirstName = "Tom", MiddleName = "C",  LastName = "Cruise", Phone=1896732136, EMail="tomcruise@outlook.com",QualificationID = 3,
                  Skills = new List<Skill> { new Skill { SkillName="Physics" },
                        new Skill { SkillName = "Chemistry"},
                        new Skill { SkillName = "Math - Statistics" } }  },
                new Applicant { FirstName = "Abby", MiddleName = "S",  LastName = "Sandel", Phone=2456732136, EMail="sandel@outlook.com", QualificationID = 4 },
                new Applicant { FirstName = "Marc", MiddleName = "M",  LastName = "Thomas", Phone=2656732136, EMail="thomas@outlook.com", QualificationID = 1  },
                new Applicant { FirstName = "Pierrick", MiddleName = "C",  LastName = "Calvez", Phone=1256732136, EMail="calvez@outlook.com", QualificationID = 2 },
                new Applicant { FirstName = "Joshua", MiddleName = "J",  LastName = "James", Phone=2846732136, EMail="james@outlook.com", QualificationID = 3 },
                new Applicant { FirstName = "Tyler", MiddleName = "R",  LastName = "Finck", Phone=2895572136, EMail="finck@outlook.com", QualificationID = 4},
                new Applicant { FirstName = "Josie", MiddleName = "S",  LastName = "Smith", Phone=1996732136, EMail="js@outlook.com", QualificationID = 1 },
                new Applicant { FirstName = "King", MiddleName = "J",  LastName = "Dev", Phone=2796332236, EMail="kjd@outlook.com", QualificationID = 2 },
                new Applicant { FirstName = "Teresa", MiddleName = "A",  LastName = "Jhon", Phone=2896532136, EMail="teresa@outlook.com", QualificationID = 3 }
            };
            applicants.ForEach(d => context.Applicants.AddOrUpdate(n => n.LastName, d));
            SaveChanges(context);

            //var postings = new List<Posting>
            //{
            // new Posting{ PositionID = 1, NumberOpen=5, SalaryTypeID=1 , FTEType = 0, Schools = new List<School> { new School { ID=1 },new School { ID =2} }, Salary=21,ClosingDate= DateTime.Parse("2019-04-25") ,StartDate=DateTime.Parse("2019-03-29"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 2, NumberOpen=4, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=2 },new School { ID =4} },  Salary=21,ClosingDate= DateTime.Parse("2019-04-23") ,StartDate=DateTime.Parse("2019-03-25"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 1, NumberOpen=5, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=3 },new School { ID =6} },  Salary=21,ClosingDate= DateTime.Parse("2019-05-24") ,StartDate=DateTime.Parse("2019-04-26"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 2, NumberOpen=6, SalaryTypeID=3 , FTEType = 0,Schools = new List<School> { new School { ID=4 },new School { ID =3} },  Salary=21,ClosingDate= DateTime.Parse("2019-06-25") ,StartDate=DateTime.Parse("2019-05-27"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 3, NumberOpen=7, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=2 },new School { ID =2} },  Salary=21,ClosingDate= DateTime.Parse("2019-07-26") ,StartDate=DateTime.Parse("2019-06-28"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 4, NumberOpen=3, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=3 },new School { ID =6} },  Salary=21,ClosingDate= DateTime.Parse("2019-08-27") ,StartDate=DateTime.Parse("2019-07-29"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 5, NumberOpen=1, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=4 },new School { ID =7} },  Salary=21,ClosingDate= DateTime.Parse("2019-09-28") ,StartDate=DateTime.Parse("2019-08-30"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 6, NumberOpen=2, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=6 },new School { ID =1} },  Salary=21,ClosingDate= DateTime.Parse("2019-10-29") ,StartDate=DateTime.Parse("2019-09-13"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 7, NumberOpen=8, SalaryTypeID=3 , FTEType = 0,Schools = new List<School> { new School { ID=5 },new School { ID =3} },  Salary=21,ClosingDate= DateTime.Parse("2019-12-31") ,StartDate=DateTime.Parse("2019-10-24"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 8, NumberOpen=9, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=2 },new School { ID =5} },  Salary=21,ClosingDate= DateTime.Parse("2019-12-23") ,StartDate=DateTime.Parse("2019-11-23"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 9, NumberOpen=5, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=3 },new School { ID =7} },  Salary=21,ClosingDate= DateTime.Parse("2019-12-28") ,StartDate=DateTime.Parse("2019-12-25"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 10, NumberOpen=4, SalaryTypeID=3 , FTEType = 0,Schools = new List<School> { new School { ID=4 },new School { ID =5} },  Salary=21,ClosingDate= DateTime.Parse("2019-06-11") ,StartDate=DateTime.Parse("2019-04-26"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 2, NumberOpen=7, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=5 },new School { ID =3} },  Salary=21,ClosingDate= DateTime.Parse("2019-07-12") ,StartDate=DateTime.Parse("2019-03-27"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 3, NumberOpen=2, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=5 },new School { ID =2} },  Salary=21,ClosingDate= DateTime.Parse("2019-08-15") ,StartDate=DateTime.Parse("2019-04-28"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 4, NumberOpen=5, SalaryTypeID=3 , FTEType = 0,Schools = new List<School> { new School { ID=3 },new School { ID =4} },  Salary=21,ClosingDate= DateTime.Parse("2019-09-17") ,StartDate=DateTime.Parse("2019-05-29"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 5, NumberOpen=7, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=6 },new School { ID =5} },  Salary=21,ClosingDate= DateTime.Parse("2019-12-05") ,StartDate=DateTime.Parse("2019-06-23"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 6, NumberOpen=9, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=7 },new School { ID =6} },  Salary=21,ClosingDate= DateTime.Parse("2019-11-12") ,StartDate=DateTime.Parse("2019-07-22"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 7, NumberOpen=11, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=4 },new School { ID =3} },  Salary=21,ClosingDate= DateTime.Parse("2019-12-22") ,StartDate=DateTime.Parse("2019-04-02"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 8, NumberOpen=9, SalaryTypeID=3 , FTEType = 0,Schools = new List<School> { new School { ID=3},new School { ID =1} },  Salary=21,ClosingDate= DateTime.Parse("2019-04-23") ,StartDate=DateTime.Parse("2019-04-11"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 9, NumberOpen=2, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=2 },new School { ID =1} },  Salary=21,ClosingDate= DateTime.Parse("2019-05-02") ,StartDate=DateTime.Parse("2019-04-23"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 2, NumberOpen=1, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=1 },new School { ID =3} },  Salary=21,ClosingDate= DateTime.Parse("2019-06-22") ,StartDate=DateTime.Parse("2019-04-29"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 3, NumberOpen=3, SalaryTypeID=3 , FTEType = 0,Schools = new List<School> { new School { ID=2 },new School { ID =1} },  Salary=21,ClosingDate= DateTime.Parse("2019-07-22") ,StartDate=DateTime.Parse("2019-04-23"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 4, NumberOpen=4, SalaryTypeID=1 , FTEType = 0,Schools = new List<School> { new School { ID=1 },new School { ID =5} },  Salary=21,ClosingDate= DateTime.Parse("2019-08-22") ,StartDate=DateTime.Parse("2019-04-22"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            // new Posting{ PositionID = 3, NumberOpen=5, SalaryTypeID=2 , FTEType = 0,Schools = new List<School> { new School { ID=1 },new School { ID =6} },  Salary=21,ClosingDate= DateTime.Parse("2019-09-22") ,StartDate=DateTime.Parse("2019-04-23"), Details=" position will focus mainly on providing psychological consultation and programming support, as well as bu"},
            //};
            //postings.ForEach(d => context.Postings.AddOrUpdate(n => n.ClosingDate, d));
            //SaveChanges(context);

        }
    }
}
