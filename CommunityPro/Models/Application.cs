using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class Application
    {

        public Application()
        {

            this.Files = new HashSet<aFile>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You musst select Applicant.")]
        public int ApplicantID { get; set; }

        [Required(ErrorMessage = "You musst select Posting.")]
        public int PostingID { get; set; }

        [Display(Name = "Comments")]
        [Required(ErrorMessage = "You cannot leave comments blank.")]
        [StringLength(100, ErrorMessage = "comments cannot be more than 100 characters long.")]
        public string Comments { get; set; }

        public virtual Posting Posting { get; set; }

        public virtual Applicant Applicant { get; set; }

        public virtual ICollection<aFile> Files { get; set; }

    }
}