using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class Posting : IValidatableObject
    {
        public Posting()
        {
            this.Applications = new HashSet<Application>();
        }


        public int ID { get; set; }

        [Display(Name = "Position")]
        [Required(ErrorMessage = "Position is required.")]
        public int PositionID { get; set; }

        [Display(Name = "FTE")]
        [Required(ErrorMessage = "Invalid FTE amount")]
        [Range(0, 1.5, ErrorMessage = "You must specify the valid amount")]
        public decimal FTEType { get; set; }

        [Display(Name = "# of Positions")]
        [Required(ErrorMessage = "You must show how many positions are available")]
        [Range(1, 9999, ErrorMessage = "The number you have entered is invalid")]
        public int NumberOpen { get; set; }

        [Display(Name = "Status")]
        [StringLength(9, ErrorMessage = "Status cannot be more than 9.")]
        public string Status { get; set; }

        [Display(Name = "Opening Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Closing Date")]
        [Required(ErrorMessage = "You must put the closing date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [Display(Name = "Details")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(999999, ErrorMessage = "That's too long Description to put in.")]
        public string Details { get; set; }

        [Range(1, 999999999, ErrorMessage = "Salary can not be more than 999999999 character.")]
        [DisplayFormat(DataFormatString = "${0:#,#}", ApplyFormatInEditMode = false)]
        public int Salary { get; set; }

        [Display(Name = "Salary Type")]
        public int SalaryTypeID { get; set; }

        public virtual SalaryType SalaryType { get; set; }
        public virtual Position Position { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ClosingDate < DateTime.Today)
                yield return new ValidationResult("The closing date can not be in the past.", new[] { "ClosingDate" });
            if (StartDate < DateTime.Today)
                yield return new ValidationResult("The start date can not be in the past.", new[] { "StartDate" });
            if (StartDate.GetValueOrDefault() > ClosingDate)
                yield return new ValidationResult("The start date can not be after the closing date.", new[] { "StartDate" });

        }
    }
}