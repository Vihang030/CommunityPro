using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class aFile
    {


        public int ID { get; set; }

        [Display(Name = "File Name")]
        [Required(ErrorMessage = "You cannot leave the File name blank.")]
        [StringLength(256, ErrorMessage = "File name cannot be more than 256 characters long.")]
        public string FileName { get; set; }

        [Display(Name = "Description of File")]
        [StringLength(500, ErrorMessage = "You can not enter more than 500 characters in Description")]
        public string Description { get; set; }



        public int ApplicantID { get; set; }
        public virtual Applicant Applicant { get; set; }


        public int ApplicationID { get; set; }
        public virtual Application Application { get; set; }

        public virtual FileContent FileContent { get; set; }


    }
}