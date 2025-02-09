using Prof.Hub.Domain.Aggregates.Student;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.Interfaces.Repositories;
public interface IStudentRepository
{
    Task<Result<Student>> GetByIdAsync(StudentId id, CancellationToken cancellationToken = default);
    Task<Result<Student>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(StudentId id, CancellationToken cancellationToken = default);
    Task<Result<List<Student>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<List<Student>>> GetByIdsAsync(IEnumerable<StudentId> ids, CancellationToken cancellationToken = default);
    Task AddAsync(Student student, CancellationToken cancellationToken = default);
    void Update(Student student);
    void Remove(Student student);
}
