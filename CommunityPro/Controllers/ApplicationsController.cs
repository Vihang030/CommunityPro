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
using Microsoft.AspNet.Identity;
using PagedList;

namespace CommunityPro.Controllers
{
    public class ApplicationsController : Controller
    {
        private CommunityProEntities db = new CommunityProEntities();

        // GET: Applications
        public ActionResult Index(string sortDirection, string sortField,
            string actionButton, string searchName, string searchPosting, int? page)
        {
            IQueryable<Application> applications = db.Applications;
            ViewBag.Filtering = ""; //Assume not filtering
            if (!String.IsNullOrEmpty(searchName))
            {
                applications = applications
                    .Where(p => p.Applicant.FirstName.ToString().ToUpper().Contains(searchName.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = searchName;
            }

            if (!String.IsNullOrEmpty(searchPosting))
            {
                applications = applications.Where(p => p.Posting.Position.Name.ToString().ToUpper().Contains(searchPosting.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchPosting = searchPosting;
            }

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

            if (sortField == "Position")//Sorting by Applicant Name
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applications = applications
                        .OrderBy(p => p.Posting.Position.Name);
                }
                else
                {
                    applications = applications
                        .OrderByDescending(p => p.Posting.Position.Name);
                }
            }
            else //By default sort by Applicant
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applications = applications
                        .OrderBy(p => p.Applicant.FirstName);
                }
                else
                {
                    applications = applications
                        .OrderByDescending(p => p.Applicant.FirstName);
                }

            }

            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(applications.ToPagedList(pageNumber, pageSize));
        }

        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create(int? id)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.UserID = UserID; //new SelectList(db.Applicants, "ID", "FirstName");
            ViewBag.PostingID = id; //new SelectList(db.Postings, "ID", "Status");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ApplicantID,PostingID")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", application.ApplicantID);
            ViewBag.PostingID = new SelectList(db.Postings, "ID", "Status", application.PostingID);
            return View(application);
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", application.ApplicantID);
            ViewBag.PostingID = new SelectList(db.Postings, "ID", "Status", application.PostingID);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplicantID,PostingID,Comments")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", application.ApplicantID);
            ViewBag.PostingID = new SelectList(db.Postings, "ID", "Status", application.PostingID);
            return View(application);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
