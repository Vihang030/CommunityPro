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

namespace CommunityPro.Controllers
{
    public class PositionsController : Controller
    {
        private CommunityProEntities db = new CommunityProEntities();

        // GET: Positions
        public ActionResult Index(string sortDirection, string sortField,
            string actionButton, string searchName, string searchJobCode, int? page)
        {
            IQueryable<Position> positions = db.Postions.Include(a => a.Skills);
            ViewBag.Filtering = "";

            //Filter by the Name of the position
            if (!String.IsNullOrEmpty(searchName))
            {

                positions = positions.Where(p => p.Name.ToUpper().Contains(searchName.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = searchName;
            }


            //Filter by the Job code of the position
            if (!String.IsNullOrEmpty(searchJobCode))
            {
                positions = positions.Where(p => p.JobCode.ToString().ToUpper().Contains(searchJobCode.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchPhone = searchJobCode;
            }


            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                //Reset paging if ANY button was pushed


                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

            if (sortField == "Job Code")//Sorting by Applicant Name
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    positions = positions
                        .OrderBy(p => p.JobCode);
                }
                else
                {
                    positions = positions
                        .OrderByDescending(p => p.JobCode);
                }
            }
            else //By default sort by Applicant
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    positions = positions
                        .OrderBy(p => p.Name);
                }
                else
                {
                    positions = positions
                        .OrderByDescending(p => p.Name);
                }

            }

            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(positions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Postions
              .Include(p => p.Skills)
              .Include(q => q.Qualifications)
              .Where(i => i.ID == id)
              .SingleOrDefault();
            if (position == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedSkillData(position);
            PopulateAssignedQualificationData(position);
            return View(position);
        }


        private void PopulateAssignedQualificationData(Position position)
        {
            var allQualifications = db.Qualifications;

            var pQualifications = new HashSet<int>(position.Qualifications.Select(b => b.ID));

            var qualificationsAvailable = new List<QualificationVM>();
            var qualificationsSelected = new List<QualificationVM>();

            foreach (var qualification in allQualifications)
            {
                if (pQualifications.Contains(qualification.ID))
                {
                    qualificationsSelected.Add(new QualificationVM
                    {
                        QualificationID = qualification.ID,
                        QualificationName = qualification.DegreeName,
                        //Assigned = true
                    });
                }
                else
                {
                    qualificationsAvailable.Add(new QualificationVM
                    {
                        QualificationID = qualification.ID,
                        QualificationName = qualification.DegreeName,
                        //Assigned = false
                    });
                }
            }

            ViewBag.selQua = new MultiSelectList(qualificationsSelected, "QualificationID", "QualificationName");
            ViewBag.availQua = new MultiSelectList(qualificationsAvailable, "QualificationID", "QualificationName");

        }

        private void PopulateAssignedSkillData(Position position)
        {
            var allSkills = db.Skills;

            var pSkills = new HashSet<int>(position.Skills.Select(b => b.ID));

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
                        //Assigned = true
                    });
                }
                else
                {
                    viewModelAvailable.Add(new SkillVM
                    {
                        SkillID = skill.ID,
                        SkillName = skill.SkillName,
                        //Assigned = false
                    });
                }
            }

            ViewBag.selOpts = new MultiSelectList(viewModelSelected, "SkillID", "SkillName");
            ViewBag.availOpts = new MultiSelectList(viewModelAvailable, "SkillID", "SkillName");

        }


        // GET: Positions/Create
        public ActionResult Create()
        {
            var position = new Position();
            position.Skills = new List<Skill>();
            position.Qualifications = new List<Qualification>();
            PopulateAssignedSkillData(position);
            PopulateAssignedQualificationData(position);
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,JobCode,Name,Summary")] Position position, string[] selectedOptions, string[] selectedQualifications)
        {
            //Add the selected Skills and Qualifications
            if (selectedOptions != null)
            {
                position.Skills = new List<Skill>();
                foreach (var cond in selectedOptions)
                {
                    var condToAdd = db.Skills.Find(int.Parse(cond));
                    position.Skills.Add(condToAdd);
                }
              
            }

            if(selectedQualifications != null)
            {

                position.Qualifications = new List<Qualification>();
                foreach (var cond in selectedQualifications)
                {
                    var condToAdd = db.Qualifications.Find(int.Parse(cond));
                    position.Qualifications.Add(condToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                db.Postions.Add(position);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(position);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Postions
                .Include(p => p.Skills)
                .Include(q => q.Qualifications)
                .Where(i => i.ID == id)
                .SingleOrDefault();
            if (position == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedSkillData(position);
            PopulateAssignedQualificationData(position);
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,JobCode,Name,Summary")] Position position, int? id, string[] selectedOptions, string[] selectedQualifications)
        {
            Position positionToUpdate = db.Postions
                .Include(p => p.Skills)
                .Include(q => q.Qualifications)
                .Where(a => a.ID == id).SingleOrDefault();

            if (ModelState.IsValid)
            {
                UpdatePositionSkills(selectedOptions, positionToUpdate);
                UpdatePositionQualifications(selectedQualifications, positionToUpdate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedSkillData(positionToUpdate);
            PopulateAssignedQualificationData(positionToUpdate);
            return View(position);
        }

        //Method for updating Qualifications for positions
        private void UpdatePositionQualifications(string[] selectedQualifications, Position positionToUpdate)
        {
            if (selectedQualifications == null)
            {
                positionToUpdate.Qualifications = new List<Qualification>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedQualifications);
            var positionQua = new HashSet<int>
             (positionToUpdate.Qualifications.Select(c => c.ID));//IDs of the currently selected Qualifications

            foreach (var cond in db.Qualifications)
            {
                if (selectedOptionsHS.Contains(cond.ID.ToString()))
                {
                    if (!positionQua.Contains(cond.ID))
                    {
                        positionToUpdate.Qualifications.Add(cond);
                    }
                }
                else
                {
                    if (positionQua.Contains(cond.ID))
                    {
                        positionToUpdate.Qualifications.Remove(cond);
                    }
                }
            }

        }

        //Method for updating Skills for positions
        private void UpdatePositionSkills(string[] selectedOptions, Position positionToUpdate)
        {

            if (selectedOptions == null)
            {
                positionToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var positionConds = new HashSet<int>
                (positionToUpdate.Skills.Select(c => c.ID));//IDs of the currently selected Skills

            foreach (var cond in db.Skills)
            {
                if (selectedOptionsHS.Contains(cond.ID.ToString()))
                {
                    if (!positionConds.Contains(cond.ID))
                    {
                        positionToUpdate.Skills.Add(cond);
                    }
                }
                else
                {
                    if (positionConds.Contains(cond.ID))
                    {
                        positionToUpdate.Skills.Remove(cond);
                    }
                }
            }

        }


        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Postions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Position position = db.Postions.Find(id);
            db.Postions.Remove(position);
            db.SaveChanges();
            return RedirectToAction("Index");
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
