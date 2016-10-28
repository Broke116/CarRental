using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Web.Models.ViewModel
{
    public enum Gender
    {
        Male,
        Female
    }

    public class CustomerViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Firstname required")]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname required")]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [EmailAddress(ErrorMessage = "Must be valid address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile required")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public Guid UniqueKey { get; set; }

        [Required(ErrorMessage = "Citizen number required")]
        [Display(Name = "Citizen number")]
        public int CitizenNumber { get; set; }

        [Required(ErrorMessage = "Passport Number required")]
        [Display(Name = "Passport Number")]
        public int PassportNumber { get; set; }

        [Required(ErrorMessage = "Location required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "City required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country required")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "LicenseOrigin required")]
        [Display(Name = "LicenseOrigin")]
        public string LicenseOrigin { get; set; }

        [Required(ErrorMessage = "BirthDate required")]
        [Display(Name = "BirthDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}