using Microsoft.EntityFrameworkCore;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Domain.Aggregates.Student;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Infrastructure.PostgresSql;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Infrastructure.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Student>> GetByIdAsync(StudentId id, CancellationToken cancellationToken = default)
    {
        var student = await _context.Set<Student>()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (student == null)
            return Result.NotFound($"Estudante com ID {id.Value} não encontrado.");

        return student;
    }

    public async Task<Result<Student>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var student = await _context.Set<Student>()
            .FirstOrDefaultAsync(s => s.Profile.Email.Value == email, cancellationToken);

        if (student == null)
            return Result.NotFound($"Estudante com email {email} não encontrado.");

        return student;
    }

    public async Task<bool> ExistsAsync(StudentId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Student>()
            .AnyAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Result<List<Student>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var students = await _context.Set<Student>()
            .ToListAsync(cancellationToken);

        return students;
    }

    public async Task<Result<List<Student>>> GetByIdsAsync(IEnumerable<StudentId> ids, CancellationToken cancellationToken = default)
    {
        var students = await _context.Set<Student>()
            .Where(s => ids.Contains(s.Id))
            .ToListAsync(cancellationToken);

        if (!students.Any())
            return Result.NotFound("Nenhum estudante encontrado com os IDs fornecidos.");

        return students;
    }

    public async Task AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        await _context.Set<Student>().AddAsync(student, cancellationToken);
    }

    public void Update(Student student)
    {
        _context.Set<Student>().Update(student);
    }

    public void Remove(Student student)
    {
        _context.Set<Student>().Remove(student);
    }
}
