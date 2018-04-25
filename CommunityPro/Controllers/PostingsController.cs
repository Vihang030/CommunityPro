using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityPro.DAL;
using CommunityPro.Models;
using PagedList;
using CommunityProject.ViewModels;
using Microsoft.AspNet.Identity;

namespace CommunityPro.Controllers
{
    public class PostingsController : Controller
    {

        private CommunityProEntities db = new CommunityProEntities();

        // GET: Postings
        public ActionResult Index(string sortDirection, string sortField,
              string actionButton, string searchName, string numberPosition, 
              string startDate,string closingDate,string schools, string[] selectedSkills, int? page)
        {

            //Prepare a ghost Posting to help maintain the selected skills
            var posting = new Posting();


            //LINQ - Start with the Includes
            var postings = db.Postings
                .Include(p => p.Position);

            //Add as many filters as you want
            if (!String.IsNullOrEmpty(searchName))
            {
                postings = postings.Where(p => p.Position.Name.ToUpper().Contains(searchName.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = searchName;
            }

            if (!String.IsNullOrEmpty(numberPosition))
            {
                postings = postings.Where(p => p.NumberOpen.ToString().ToUpper().Contains(numberPosition.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = numberPosition;
            }

            if (!String.IsNullOrEmpty(startDate))
            {
                postings = postings.Where(p => p.StartDate.ToString().ToUpper().Contains(startDate.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = startDate;
            }

            if (!String.IsNullOrEmpty(closingDate))
            {
                postings = postings.Where(p => p.ClosingDate.ToString().ToUpper().Contains(startDate.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = closingDate;
            }


            //if (!String.IsNullOrEmpty(schools))
            //{
            //    postings = postings.Where(p => p.Schools.ToString().ToUpper().Contains(startDate.ToUpper()));
            //    ViewBag.Filtering = " in";//Flag filtering
            //    ViewBag.searchName = schools;
            //}

            //Before we sort, see if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                //Reset paging if ANY button was pushed
                page = 1;

                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }
            //Now we know which field and direction to sort by, but a Switch is hard to use for 2 criteria
            //so we will use an if() structure instead.
            if (sortField == "# of Position")//Sorting by Applicant Name
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.NumberOpen);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.NumberOpen);
                }
            }
            else if (sortField == "Closing Date")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.ClosingDate);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.ClosingDate);
                }
            }
            else if (sortField == "Start Date")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.StartDate);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.StartDate);
                }
            }
            else //By default sort by Position NAME
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.Position.Name);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.Position.Name);
                }
            }



            //Set sort for next time
            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(postings.ToPagedList(pageNumber, pageSize));
        }

        // GET: Postings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings
                .Include(p => p.Schools)
                .Include(p => p.Position.Skills)
                .Include(p => p.Position.Qualifications)
                .Where(i => i.ID == id)
                .SingleOrDefault();
            if (posting == null)
            {
                return HttpNotFound();
            }

            PopulateAssignedQualificationData(posting);
            PopulateAssignedSkillData(posting);
            PopulateAssignedSchoolData(posting);
            return View(posting);
        }

        //Show qualifications of the position
        private void PopulateAssignedQualificationData(Posting posting)
        {
            var allQualifications = db.Qualifications;
            var pQualifications = new HashSet<int>(posting.Position.Qualifications.Select(b => b.ID));
            var viewModelSelected = new List<QualificationVM>();

            foreach (var qualification in allQualifications)
            {
                if (pQualifications.Contains(qualification.ID))
                {
                    viewModelSelected.Add(new QualificationVM
                    {
                        QualificationID = qualification.ID,
                        QualificationName = qualification.DegreeName,
                        //Assigned = true
                    });
                }

            }

            ViewBag.selQualifications = new MultiSelectList(viewModelSelected, "QualificationID", "QualificationName");
        }

        //Show Skills of the position
        private void PopulateAssignedSkillData(Posting posting)
        {
            var allSkills = db.Skills;
            var pSkills = new HashSet<int>(posting.Position.Skills.Select(b => b.ID));
            var viewModelSelected = new List<SkillVM>();
            //var viewModelAvailable = new List<SkillVM>();

            foreach (var skill in allSkills)
            {
                if (pSkills.Contains(skill.ID))
                {
                    viewModelSelected.Add(new SkillVM
                    {
                        SkillID = skill.ID,
                        SkillName = skill.SkillName,
                        //Assigned = true
                    });
                }
                //else
                //{
                //    viewModelAvailable.Add(new SkillVM
                //    {
                //        SkillID = skill.ID,
                //        SkillName = skill.SkillName,
                //        //Assigned = true
                //    });
                //}

            }

            ViewBag.selSkills = new MultiSelectList(viewModelSelected, "SkillID", "SkillName");
            //ViewBag.availSkills = new MultiSelectList(viewModelAvailable, "SkillID", "SkillName");
        }

        public ActionResult CreateStart()
        {
            ViewBag.PositionID = new SelectList(db.Postions, "ID", "Name");
            return View("CreateStart");
        }

        // GET: Postings/Create
        public ActionResult Create(int? PositionID)
        {
            Position position = db.Postions
               .Include(p => p.Skills)
               .Include(p => p.Qualifications)
               .Where(p => p.ID == PositionID)
               .SingleOrDefault();
            //if (position == null)
            //{
            //    ModelState.AddModelError("", "No Position to use as a Template");
            //    return View();
            //}
            //We have the positon to use as a template
            var postings = new Posting()
            {
                PositionID = position.ID,
                Position = position,
                Skills = position.Skills,
                Qualifications = position.Qualifications,
                Details = position.Summary
            };

            var posting = new Posting();
            posting.Schools = new List<School>();


            ViewBag.PositionID = new SelectList(db.Postions, "ID", "JobCode");
            ViewBag.SalaryTypeID = new SelectList(db.SalaryTypes, "ID", "Salarytype");
            ViewBag.SchoolID = new SelectList(db.Schools, "ID", "Name");
            
            PopulateAssignedPostingQualificationData(postings);
            PopulateAssignedPostingSkillData(postings);
            PopulateAssignedSchoolData(posting);
            return View();
        }
        

        // POST: Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PositionID,FTEType,NumberOpen,Status,StartDate,ClosingDate,Details,Salary,SalaryTypeID")] Posting posting,
            string[] selectedOptions, string[] selectedSkills)
        {
            if (selectedOptions != null)
            {
                posting.Schools = new List<School>();
                foreach (var cond in selectedOptions)
                {
                    var condToAdd = db.Schools.Find(int.Parse(cond));
                    posting.Schools.Add(condToAdd);
                }
            }
            if (selectedSkills != null)
            {
                posting.Skills = new List<Skill>();
                foreach (var skill in selectedSkills)
                {
                    var skillToAdd = db.Skills.Find(int.Parse(skill));
                    posting.Skills.Add(skillToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Postings.Add(posting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PositionID = new SelectList(db.Postions, "ID", "Name", posting.PositionID);
            ViewBag.SalaryTypeID = new SelectList(db.SalaryTypes, "ID", "Salarytype", posting.SalaryTypeID);
            return View(posting);
        }

        // GET: Postings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings
                .Include(p => p.Schools)
                .Include(p => p.Skills)
                .Include(p => p.Qualifications)
                .Where(i => i.ID == id)
                .SingleOrDefault();
            if (posting == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionID = new SelectList(db.Postions, "ID", "Name", posting.PositionID);
            ViewBag.SalaryTypeID = new SelectList(db.SalaryTypes, "ID", "Salarytype", posting.SalaryTypeID);
            PopulateAssignedPostingSkillData(posting);
            PopulateAssignedPostingQualificationData(posting);
            PopulateAssignedSchoolData(posting);
            return View(posting);
        }

        // POST: Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PositionID,FTEType,NumberOpen,Status,StartDate,ClosingDate,Details,Salary,SalaryTypeID")] Posting posting,
            int? id, string[] selectedOptions)
        {
            Posting postingToUpdate = db.Postings
              .Include(p => p.Schools)
              .Include(p => p.Skills)
              .Include(p => p.Qualifications)
              .Where(a => a.ID == id).SingleOrDefault();

            if (ModelState.IsValid)
            {
                UpdatePostingSchools(selectedOptions, postingToUpdate);
                UpdatePostingSkills(selectedOptions, postingToUpdate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PositionID = new SelectList(db.Postions, "ID", "Name", posting.PositionID);
            ViewBag.SalaryTypeID = new SelectList(db.SalaryTypes, "ID", "Salarytype", posting.SalaryTypeID);
            return View(posting);
        }

        //edit the skills
        private void UpdatePostingSkills(string[] selectedOptions, Posting postingToUpdate)
        {
            if (selectedOptions == null)
            {
                postingToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var postingConds = new HashSet<int>
                (postingToUpdate.Skills.Select(c => c.ID));//IDs of the currently selected conditions
            foreach (var cond in db.Skills)
            {
                if (selectedOptionsHS.Contains(cond.ID.ToString()))
                {
                    if (!postingConds.Contains(cond.ID))
                    {
                        postingToUpdate.Skills.Add(cond);
                    }
                }
                else
                {
                    if (postingConds.Contains(cond.ID))
                    {
                        postingToUpdate.Skills.Remove(cond);
                    }
                }
            }
        }

        private void UpdatePostingSchools(string[] selectedOptions, Posting postingToUpdate)
        {
            if (selectedOptions == null)
            {
                postingToUpdate.Schools = new List<School>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var postingConds = new HashSet<int>
                (postingToUpdate.Schools.Select(c => c.ID));//IDs of the currently selected conditions
            foreach (var cond in db.Schools)
            {
                if (selectedOptionsHS.Contains(cond.ID.ToString()))
                {
                    if (!postingConds.Contains(cond.ID))
                    {
                        postingToUpdate.Schools.Add(cond);
                    }
                }
                else
                {
                    if (postingConds.Contains(cond.ID))
                    {
                        postingToUpdate.Schools.Remove(cond);
                    }
                }
            }

        }

        // GET: Postings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            return View(posting);
        }

        // POST: Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posting posting = db.Postings.Find(id);
            db.Postings.Remove(posting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Show a listbox to add extra skills in positions in postings create
        private void PopulateAssignedPostingQualificationData(Posting postings)
        {
            var allQualifications = db.Qualifications;
            var pQualifications = new HashSet<int>(postings.Qualifications.Select(b => b.ID));
            var viewModelAvailable = new List<QualificationVM>();
            var viewModelSelected = new List<QualificationVM>();

            foreach (var qualification in allQualifications)
            {
                if (pQualifications.Contains(qualification.ID))
                {
                    viewModelSelected.Add(new QualificationVM
                    {
                        QualificationID = qualification.ID,
                        QualificationName = qualification.DegreeName,
                        Assigned = pQualifications.Contains(qualification.ID)
                    });
                }
                else
                {
                    viewModelAvailable.Add(new QualificationVM
                    {
                        QualificationID = qualification.ID,
                        QualificationName = qualification.DegreeName,
                        Assigned = pQualifications.Contains(qualification.ID)
                    });
                }
            }

            ViewBag.selQualis = new MultiSelectList(viewModelSelected, "QualificationID", "QualificationName");
            ViewBag.availQualis = new MultiSelectList(viewModelAvailable, "QualificationID", "QualificationName");
        }

        //Show a listbox to add extra skills in positions in postings create
        private void PopulateAssignedPostingSkillData(Posting postings)
        {
            var allSkills = db.Skills;
            var pSkills = new HashSet<int>(postings.Skills.Select(b => b.ID));
            var viewModelAvailable = new List<SkillVM>();
            var viewModelSelected = new List<SkillVM>();

            foreach (var skill in allSkills)
            {
                if (pSkills.Contains(skill.ID))
                {
                    viewModelSelected.Add(new SkillVM
                    {
                        SkillID = skill.ID,
                        SkillName = skill.SkillName,
                        Assigned = pSkills.Contains(skill.ID)
                    });
                }
                else
                {
                    viewModelAvailable.Add(new SkillVM
                    {
                        SkillID = skill.ID,
                        SkillName = skill.SkillName,
                        Assigned = pSkills.Contains(skill.ID)
                    });
                }
            }

            ViewBag.selSkills = new MultiSelectList(viewModelSelected, "SkillID", "SkillName");
            ViewBag.availSkills = new MultiSelectList(viewModelAvailable, "SkillID", "SkillName");
        }

        //making a string to show the skills and qualifications to autocomplete when position is choosen
        [HttpGet]
        public ActionResult GetAPosition(int? ID)
        {
            try
            {
                Position position = db.Postions
                    .Include(d => d.Skills)
                    .Include(p => p.Qualifications)
                    .Where(d => d.ID == ID)
                    .SingleOrDefault();
                //Build a string of html for the skills collection
                string Skills = "";
                string Qualifications = "";

                foreach (var s in position.Skills)
                {
                    Skills += "<ul class='list-group'><li class='list-group-item disabled col-md-12'> &#45; " + s.SkillName + "</li></ul>";
                }
                foreach (var s in position.Qualifications)
                {
                    Qualifications += "<ul class='list-group'><li class='list-group-item disabled col-md-12'>&#45; " + s.DegreeName + "</li></ul>";
                }
                //Using an annomous object as a DTO to avoid serialization errors
                var pos = new
                {
                    position.Name,
                    Skills,
                    Qualifications,
                    position.Summary

                };
                return Json(pos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        //Select the schools that you want to assign posting to
        private void PopulateAssignedSchoolData(Posting posting)
        {
            var allSchools = db.Schools;
            var pSchools = new HashSet<int>(posting.Schools.Select(b => b.ID));
            var viewModelAvailable = new List<SchoolVM>();
            var viewModelSelected = new List<SchoolVM>();

            foreach (var school in allSchools)
            {
                if (pSchools.Contains(school.ID))
                {
                    viewModelSelected.Add(new SchoolVM
                    {
                        SchoolID = school.ID,
                        SchoolName = school.Name,
                        //Assigned = true
                    });
                }
                else
                {
                    viewModelAvailable.Add(new SchoolVM
                    {
                        SchoolID = school.ID,
                        SchoolName = school.Name,
                        //Assigned = true
                    });
                }
            }
            
            ViewBag.selOpts = new MultiSelectList(viewModelSelected, "SchoolID", "SchoolName");
            ViewBag.availOpts = new MultiSelectList(viewModelAvailable, "SchoolID", "SchoolName");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
