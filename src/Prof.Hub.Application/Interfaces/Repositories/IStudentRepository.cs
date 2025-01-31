using Prof.Hub.Domain.Aggregates.Student;

namespace Prof.Hub.Application.Interfaces.Repositories;
public interface IStudentRepository
{
    Task<Student> GetByIdAsync(Guid id);
    Task<List<Student>> GetAllAsync();
    Task<Student> AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Guid id);
}
