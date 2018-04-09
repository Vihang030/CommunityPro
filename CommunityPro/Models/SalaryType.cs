using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class SalaryType
    {
        public SalaryType()
        {
            this.Postings = new HashSet<Posting>();
        }

        public int ID { get; set; }

        [Display(Name = "Salary Type")]
        [Required(ErrorMessage = "Please enter the Salary Type.")]
        [StringLength(25, ErrorMessage = "Salary type can not contain more than 25 character.")]
        public string Salarytype { get; set; }

        public virtual ICollection<Posting> Postings { get; set; }
    }
}