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
using System.IO;

namespace CommunityPro.Controllers
{
    public class ApplicantsController : Controller
    {
        private CommunityProEntities db = new CommunityProEntities();

        // GET: Applicants
        public ActionResult Index(string sortDirection, string sortField,
            string actionButton, string searchName, string searchPhone, int? page)
        {
            IQueryable<Applicant> applicants = db.Applicants.Include(a => a.Skills);
            ViewBag.Filtering = ""; //Assume not filtering

            if (!String.IsNullOrEmpty(searchName))
            {
                applicants = applicants
                    .Where(p => p.FirstName.ToUpper().Contains(searchName.ToUpper())
                        || p.LastName.ToUpper().Contains(searchName.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = searchName;
            }

            if (!String.IsNullOrEmpty(searchPhone))
            {
                applicants = applicants.Where(p => p.Phone.ToString().ToUpper().Contains(searchPhone.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchPhone = searchPhone;
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
            //Now we know which field and direction to sort by, but a Switch is hard to use for 2 criteria
            //so we will use an if() structure instead.
            if (sortField == "Phone")//Sorting by Phone Number
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applicants = applicants
                        .OrderBy(p => p.Phone);
                }
                else
                {
                    applicants = applicants
                        .OrderByDescending(p => p.Phone);
                }
            }
            //so we will use an if() structure instead.
            if (sortField == "EMail")//Sorting by Phone Number
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applicants = applicants
                        .OrderBy(p => p.EMail);
                }
                else
                {
                    applicants = applicants
                        .OrderByDescending(p => p.EMail);
                }
            }
            else //By default sort by Applicant
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    applicants = applicants
                        .OrderBy(p => p.FirstName)
                        .ThenBy(p => p.LastName);
                }
                else
                {
                    applicants = applicants
                        .OrderByDescending(p => p.FirstName)
                        .ThenByDescending(p => p.LastName);
                }

            }

            //Set sort for next time
            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(applicants.ToPagedList(pageNumber, pageSize));
        }

        // GET: Applicants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants
                  .Include(p => p.Skills)
                  .Include(a => a.Files)
                  .Include(a => a.ApplicantImage)
                  .Where(a => a.ID == id).SingleOrDefault();
            if (applicant == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedSkillData(applicant);
            return View(applicant);
        }


        //Loading all the Skills in List using method
        private void PopulateAssignedSkillData(Applicant applicant)
        {
            var allSkills = db.Skills;
            var pSkills = new HashSet<int>(applicant.Skills.Select(b => b.ID));
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

        // GET: Applicants/Create
        public ActionResult Create()
        {
            var applicant = new Applicant();
            applicant.Skills = new List<Skill>();
            PopulateAssignedSkillData(applicant);

            ViewBag.ID = new SelectList(db.ApplicantImages, "ApplicantImageID", "imageMimeType");
            ViewBag.QualificationID = new SelectList(db.Qualifications, "ID", "DegreeName");
            return View();
        }

        // POST: Applicants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Phone,EMail,QualificationID")] Applicant applicant
             , IEnumerable<HttpPostedFileBase> theFiles, string[] selectedOptions)
        {
            if (selectedOptions != null)
            {
                applicant.Skills = new List<Skill>();
                foreach (var cond in selectedOptions)
                {
                    var condToAdd = db.Skills.Find(int.Parse(cond));
                    applicant.Skills.Add(condToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                AddPicture(ref applicant, Request.Files["thePicture"]);
                AddDocuments(ref applicant, theFiles);
                db.Applicants.Add(applicant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.ApplicantImages, "ApplicantImageID", "imageMimeType", applicant.ID);
            ViewBag.QualificationID = new SelectList(db.Qualifications, "ID", "DegreeName", applicant.QualificationID);
            PopulateAssignedSkillData(applicant);
            return View(applicant);
        }

        //Creating a method for picture upload
        private void AddPicture(ref Applicant applicant, HttpPostedFileBase f)
        {
            if (f != null)
            {
                string mimeType = f.ContentType;
                int fileLength = f.ContentLength;
                if ((mimeType.Contains("image") && fileLength > 0))//Looks like we have a file!!!
                {
                    //Save the full size image
                    Stream fileStream = f.InputStream;
                    byte[] fullData = new byte[fileLength];
                    fileStream.Read(fullData, 0, fileLength);
                    //This is used for both Create and Edit so must decide
                    if (applicant.ApplicantImage == null)//Create New 
                    {
                        ApplicantImage fullImage = new ApplicantImage
                        {
                            imageContent = fullData,
                            imageMimeType = mimeType
                        };
                        applicant.ApplicantImage = fullImage;
                    }
                    else //Update the current image
                    {
                        applicant.ApplicantImage.imageContent = fullData;
                        applicant.ApplicantImage.imageMimeType = mimeType;
                    }
                }
            }

        }


        //Creating a method for file upload
        private void AddDocuments(ref Applicant applicant, IEnumerable<HttpPostedFileBase> docs)
        {
            foreach (var f in docs)
            {
                if (f != null)
                {
                    string mimeType = f.ContentType;
                    string fileName = Path.GetFileName(f.FileName);
                    int fileLength = f.ContentLength;
                    //Note: you could filter for mime types if you only want to allow
                    //certain types of files.  I am allowing everything.
                    if (!(fileName == "" || fileLength == 0))//Looks like we have a file!!!
                    {
                        Stream fileStream = f.InputStream;
                        byte[] fileData = new byte[fileLength];
                        fileStream.Read(fileData, 0, fileLength);

                        aFile newFile = new aFile
                        {
                            FileContent = new FileContent
                            {
                                Content = fileData,
                                MimeType = mimeType
                            },
                            FileName = fileName
                        };
                        applicant.Files.Add(newFile);
                    };
                }
            }
        }

        // GET: Applicants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants
                 .Include(p => p.Skills)
                 .Include(a => a.Files)
                 .Include(a => a.ApplicantImage)
                 .Where(a => a.ID == id).SingleOrDefault();
            if (applicant == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.ApplicantImages, "ApplicantImageID", "imageMimeType", applicant.ID);
            ViewBag.QualificationID = new SelectList(db.Qualifications, "ID", "DegreeName", applicant.QualificationID);

            PopulateAssignedSkillData(applicant);
            return View(applicant);
        }

        // POST: Applicants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName,Phone,EMail,QualificationID")] Applicant applicant,
             string chkRemoveImage, IEnumerable<HttpPostedFileBase> theFiles, int? id, string[] selectedOptions)
        {
            Applicant applicantToUpdate = db.Applicants
                .Include(p => p.Skills)
                .Include(a => a.Files)
                .Include(a => a.ApplicantImage)
                .Where(a => a.ID == id).SingleOrDefault();

            var patientToUpdate = db.Applicants
                .Include(p => p.Skills)
                .Where(i => i.ID == id)
                .SingleOrDefault();

            UpdatePatientConditions(selectedOptions, patientToUpdate);

            if (chkRemoveImage != null)
            {
                applicantToUpdate.ApplicantImage = null;
            }
            else
            {
                AddPicture(ref applicantToUpdate, Request.Files["thePicture"]);
            }
            AddDocuments(ref applicantToUpdate, theFiles);

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.ApplicantImages, "ApplicantImageID", "imageMimeType", applicant.ID);
            ViewBag.QualificationID = new SelectList(db.Qualifications, "ID", "DegreeName", applicant.QualificationID);

            PopulateAssignedSkillData(applicant);
            return View(applicant);
        }

        //Update method for updating skills if the changes are made
        private void UpdatePatientConditions(string[] selectedOptions, Applicant patientToUpdate)
        {
            if (selectedOptions == null)
            {
                patientToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedConditionsHS = new HashSet<string>(selectedOptions);
            var patientConds = new HashSet<int>
                (patientToUpdate.Skills.Select(c => c.ID));//IDs of the currently selected conditions
            foreach (var cond in db.Skills)
            {
                if (selectedConditionsHS.Contains(cond.ID.ToString()))
                {
                    if (!patientConds.Contains(cond.ID))
                    {
                        patientToUpdate.Skills.Add(cond);
                    }
                }
                else
                {
                    if (patientConds.Contains(cond.ID))
                    {
                        patientToUpdate.Skills.Remove(cond);
                    }
                }
            }
        }

        // GET: Applicants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: Applicants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Applicant applicant = db.Applicants.Find(id);
            db.Applicants.Remove(applicant);
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

        //making an uploaded file eligible to be downloaded
        public FileContentResult Download(int id)
        {
            var theFile = db.Files.Include(f => f.FileContent).Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent.Content, theFile.FileContent.MimeType, theFile.FileName);
        }
    }
}
