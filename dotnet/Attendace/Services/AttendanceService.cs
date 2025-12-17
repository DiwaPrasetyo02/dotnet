using WebAppMVC.Models;

namespace WebAppMVC.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly List<Attendence> _attendances;

        public AttendanceService()
        {
            _attendances = new List<Attendence>
            {
                // Cuman untuk dummy data
                new Attendence
                {
                    Participants_Id = "P001",
                    Date = DateTime.Today,
                    Status = AttendanceStatus.Hadir,
                    Notes = "Tepat waktu"
                },
                new Attendence
                {
                    Participants_Id = "P002",
                    Date = DateTime.Today,
                    Status = AttendanceStatus.Izin,
                    Notes = "Sakit"
                }
            };
        }

        public List<Attendence> GetAll()
        {
            return _attendances;
        }

        public Attendence GetByParticipantsId(string participantsId)
        {
            return _attendances.FirstOrDefault(a => a.Participants_Id == participantsId);
        }

        public void Add(Attendence attendance)
        {
            // optional: cegah duplikat Participants_Id
            if (_attendances.Any(a => a.Participants_Id == attendance.Participants_Id))
                throw new InvalidOperationException("Participants_Id sudah ada.");

            _attendances.Add(attendance);
        }

        public void Update(Attendence attendance)
        {
            var existing = _attendances.FirstOrDefault(a => a.Participants_Id == attendance.Participants_Id);
            if (existing != null)
            {
                existing.Date = attendance.Date;
                existing.Status = attendance.Status;
                existing.Notes = attendance.Notes;
            }
        }

        public void Delete(string participantsId)
        {
            _attendances.RemoveAll(a => a.Participants_Id == participantsId);
        }
    }
}
