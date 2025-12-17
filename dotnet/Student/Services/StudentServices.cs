using WebAppMVC.Models;
namespace WebAppMVC.Services
{
    public class StudentService : IStudentService
    {
        private readonly List<Student> _students;
        public StudentService()
        {
            _students = new List<Student>
{
new Student { Id = 1, Name = "Alice", Email = "alice@example.com", Age = 20 },
new Student { Id = 2, Name = "Bob", Email = "bob@example.com", Age = 22 }
};
        }
        public List<Student> GetAllStudents()
        {
            return _students;
        }
        public Student GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }
        public void AddStudent(Student student)
        {
            student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            _students.Add(student);
        }
        public void UpdateStudent(Student student)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.Age = student.Age;
            }
        }
        public void DeleteStudent(int id)
        {
            _students.RemoveAll(s => s.Id == id);
        }
    }
}