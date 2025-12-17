using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;
using WebAppMVC.Services;

namespace WebAppMVC.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: Attendance
        public IActionResult Index()
        {
            var attendances = _attendanceService.GetAll();
            return View(attendances);
        }

        // GET: Attendance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Attendence attendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _attendanceService.Add(attendance);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(attendance);
        }

        // GET: Attendance/Edit/{participantsId}
        public IActionResult Edit(string participantsId)
        {
            var attendance = _attendanceService.GetByParticipantsId(participantsId);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendance/Edit/{participantsId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string participantsId, Attendence attendance)
        {
            if (participantsId != attendance.Participants_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _attendanceService.Update(attendance);
                return RedirectToAction(nameof(Index));
            }

            return View(attendance);
        }

        // GET: Attendance/Delete/{participantsId}
        public IActionResult Delete(string participantsId)
        {
            var attendance = _attendanceService.GetByParticipantsId(participantsId);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendance/Delete/{participantsId}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string participantsId)
        {
            _attendanceService.Delete(participantsId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendance/Details/{participantsId}
        public IActionResult Details(string participantsId)
        {
            var attendance = _attendanceService.GetByParticipantsId(participantsId);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }
    }
}
