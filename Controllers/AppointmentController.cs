using hospitalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hospitalMS.Controllers
{
    public class AppointmentController : Controller
    {
        HospitalDBEntities db = new HospitalDBEntities();
        public ActionResult Index(int doctorid)
        {
            var doctor = db.Doctors.Find(doctorid);
            ViewBag.Doctor = doctor;

            AppointmentViewModel model = new AppointmentViewModel();
            model.DoctorId = doctorid;

            ViewBag.TimeSlots = GetTimeSlots(doctor.AvailableTime);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AppointmentViewModel model)
        {
            var doctor = db.Doctors.Find(model.DoctorId);
            ViewBag.Doctor = doctor;
            ViewBag.TimeSlots = GetTimeSlots(doctor.AvailableTime);

            // Pehle normal validation
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Ab parse karo
            DateTime selectedDate = DateTime.Parse(model.AppointmentDate);
            TimeSpan selectedTime = TimeSpan.Parse(model.AppointmentTime);

            // Doctor available hai ya nahi
            if (!IsDoctorAvailableOnDay(doctor.AvailableDays, selectedDate))
            {
                ModelState.AddModelError("AppointmentDate",
                    "Doctor is not available on the selected day.");

                return View("Index", model);
            }

            bool alreadyBooked = db.Appointments.Any(x =>
                x.DoctorId == model.DoctorId &&
                x.AppointmentDate == selectedDate &&
                x.AppointmentTime == selectedTime);

            if (alreadyBooked)
            {
                ModelState.AddModelError("AppointmentTime",
                    "This time slot is already booked.");

                return View("Index", model);
            }

            Patient p = new Patient
            {
                Name = model.Name,
                Phone = model.Phone,
                Age = model.Age.Value,
                Gender = model.Gender,
                CreatedAt = DateTime.Now
            };

            db.Patients.Add(p);
            db.SaveChanges();

            Appointment a = new Appointment
            {
                PatientId = p.Id,
                DoctorId = model.DoctorId,
                AppointmentDate = selectedDate,
                AppointmentTime = selectedTime,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            db.Appointments.Add(a);
            db.SaveChanges();

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {

            return View();
        }


        private bool IsDoctorAvailableOnDay(string availableDays, DateTime appointmentDate)
        {
            string[] weekDays =
            {
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday"
    };

            var parts = availableDays.Split('-');

            if (parts.Length != 2)
                return false;

            string startDay = parts[0].Trim();
            string endDay = parts[1].Trim();

            int startIndex = Array.IndexOf(weekDays, startDay);
            int endIndex = Array.IndexOf(weekDays, endDay);
            int selectedIndex = Array.IndexOf(weekDays, appointmentDate.DayOfWeek.ToString());

            if (startIndex == -1 || endIndex == -1 || selectedIndex == -1)
                return false;

            if (startIndex <= endIndex)
            {
                return selectedIndex >= startIndex && selectedIndex <= endIndex;
            }

            return selectedIndex >= startIndex || selectedIndex <= endIndex;
        }

        private List<SelectListItem> GetTimeSlots(string availableTime)
        {
            List<SelectListItem> slots = new List<SelectListItem>();

            var parts = availableTime.Split('-');

            if (parts.Length != 2)
                return slots;

            DateTime start = DateTime.Parse(parts[0].Trim());
            DateTime end = DateTime.Parse(parts[1].Trim());

            while (start <= end)
            {
                slots.Add(new SelectListItem
                {
                    Text = start.ToString("hh:mm tt"),
                    Value = start.ToString("HH:mm")
                });

                start = start.AddMinutes(30);
            }

            return slots;
        }

    }

}