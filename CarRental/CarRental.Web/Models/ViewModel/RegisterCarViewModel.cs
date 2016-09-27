using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Models.ViewModel
{
    #region enums
    public enum GroupType
    {
        Economic,
        Middle,
        High,
        Luxury,
        Jeep,
        Minivan
    }

    public enum GearType
    {
        Automatic,
        Manual
    }

    public enum FuelType
    {
        Gasoline,
        Diesel
    }

    public enum Capacity
    {
        [Display(Name = "2")]
        Two,
        [Display(Name = "4")]
        Four,
        [Display(Name = "5")]
        Five,
        [Display(Name = "9")]
        Nine
    }

    public enum Insurance
    {
        Exempt,
        [Display(Name = "Not Exempt")]
        NotExempt
    }

    #endregion

    public class RegisterCarViewModel
    {
        [Required(ErrorMessage = "Title required")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Stock value is required")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Group Type required")]
        [Display(Name = "Group Type")]
        public string GroupType { get; set; }

        [Required(ErrorMessage = "Gear Type required")]
        [Display(Name = "Gear Type")]
        public string GearType { get; set; }

        [Required(ErrorMessage = "Fuel Type required")]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [Required(ErrorMessage = "Car location required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "City required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country required")]
        [Display(Name = "Country")]
        public string County { get; set; }

        [Required(ErrorMessage = "Capacity required")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Price required")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Insurance Type required")]
        [Display(Name = "Insurance Type")]
        public string InsuranceType { get; set; }

        [Required(ErrorMessage = "Description required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image required")]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Rating required")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }
    }
}