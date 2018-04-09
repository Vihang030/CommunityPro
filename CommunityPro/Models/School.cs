using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class School
    {

        public int ID { get; set; }

        [Display(Name = "School")]
        [Required(ErrorMessage = "School Name is required.")]
        [StringLength(255, ErrorMessage = "Name of the School can not be more than 255 character.")]
        public string Name { get; set; }

        public virtual ICollection<Posting> Postings { get; set; }

    }
}