using System.ComponentModel.DataAnnotations;
using System.Web;

namespace hospitalMS.ViewModels
{
    public class DoctorVM
    {
        // Doctor Id (Edit ke liye)
        public int Id { get; set; }

        // User Table
        [Required(ErrorMessage = "Doctor Name is required")]
        public string Name { get; set; }

     
        public string Email { get; set; }

    
        public string Password { get; set; }

        // Doctor Table
        [Required(ErrorMessage = "Department is required")]
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Consultation Fee is required")]
        public decimal? Fee { get; set; }

        public bool? IsAvailable { get; set; }

        [Required(ErrorMessage = "Available Days are required")]
        public string AvailableDays { get; set; }

        [Required(ErrorMessage = "Available Time is required")]
        public string AvailableTime { get; set; }

        // Existing Image Path
        public string Image { get; set; }

        // New Image Upload
        public HttpPostedFileBase ImageFile { get; set; }
    }
}