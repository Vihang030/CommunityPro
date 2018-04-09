using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class Skill
    {

        public int ID { get; set; }

        [Display(Name = "Skill Name")]
        [Required(ErrorMessage = "Please enter the Skill Name.")]
        [StringLength(50, ErrorMessage = "Skill name can not be more than 50 characters.")]
        [Index("IX_Unique_Skill", IsUnique = true)]
        public string SkillName { get; set; }

        public virtual ICollection<Applicant> Applicants { get; set; }
        public virtual ICollection<Posting> Postings { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}