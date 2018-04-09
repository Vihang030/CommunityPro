using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class Qualification
    {

        public int ID { get; set; }

        [Display(Name = "Degree")]
        [Required(ErrorMessage = "You cannot leave the degree blank.")]
        [StringLength(25, ErrorMessage = "Degree name cannot be more than 25 characters long.")]
        public string DegreeName { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Posting> Postings { get; set; }
        public virtual ICollection<Applicant> Applicants { get; set; }
    }
}