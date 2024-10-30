using Microsoft.EntityFrameworkCore;
using Prof.Hub.Application.Interfaces.Repositories;
using Prof.Hub.Domain.Aggregates.Student;
using Prof.Hub.Infrastructure.PostgresSql;

namespace Prof.Hub.Infrastructure.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> GetByIdAsync(Guid id)
    {
        return await _context.Set<Student>().FindAsync(id);
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Set<Student>().ToListAsync();
    }

    public async Task<Student> AddAsync(Student student)
    {
        await _context.Set<Student>().AddAsync(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Set<Student>().Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var student = await GetByIdAsync(id);
        if (student is not null)
        {
            _context.Set<Student>().Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
