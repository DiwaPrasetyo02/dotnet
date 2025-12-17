using WebAppMVC.Models;

namespace WebAppMVC.Services
{
    public interface IAttendanceService
    {
        List<Attendence> GetAll();
        Attendence GetByParticipantsId(string participantsId);
        void Add(Attendence attendance);
        void Update(Attendence attendance);
        void Delete(string participantsId);
    }
}
