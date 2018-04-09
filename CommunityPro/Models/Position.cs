using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class Position
    {
        public Position()
        {
            this.Postings = new HashSet<Posting>();
        }

        [Display(Name = "Position")]
        public string NameCode
        {
            get
            {
                return JobCode + " " + Name;
            }
        }
        public int ID { get; set; }

        public string SummaryShort
        {
            get
            {
                return string.Join(" ", Summary.Split(' ').Take(10)) + "...";
            }
        }

        [Display(Name = "Job Code")]
        [Required(ErrorMessage = "Please enter the Job Code.")]
        [Index("IX_Unique_JobCode", IsUnique = true)]
        [Range(1, 9999999, ErrorMessage = "That's too long!!")]
        public int JobCode { get; set; }

        [Display(Name = "Position Name")]
        [Required(ErrorMessage = "Position Name is required.")]
        [StringLength(100, ErrorMessage = "Name of the position can not be more than 100 character.")]
        public string Name { get; set; }

        [Display(Name = "Summary")]
        [Required(ErrorMessage = "Summary is required.")]
        [StringLength(999999, ErrorMessage = "That's too long Summary to put in.")]
        public string Summary { get; set; }


        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Posting> Postings { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; }
    }
}