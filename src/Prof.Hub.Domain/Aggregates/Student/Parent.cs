using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student;
public class Parent : AuditableEntity
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private Parent(Name name, Email email, PhoneNumber phoneNumber)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public static Parent Create(Name name, Email email, PhoneNumber phoneNumber)
    {
        return new Parent(name, email, phoneNumber);
    }

    public void Update(Name name, Email email, PhoneNumber phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}

