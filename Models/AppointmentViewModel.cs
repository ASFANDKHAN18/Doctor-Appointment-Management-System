using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hospitalMS.Models
{
    public class AppointmentViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^03\d{9}$",
     ErrorMessage = "Enter a valid phone number (03XXXXXXXXX).")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Please select gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please select appointment date.")]
        public string AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select appointment time.")]
        public string AppointmentTime { get; set; }

        public int DoctorId { get; set; }
    }
}