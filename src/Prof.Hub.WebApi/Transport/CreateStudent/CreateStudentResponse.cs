﻿namespace Prof.Hub.WebApi.Transport.CreateStudent;

public record CreateStudentResponse(
        Guid Id,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string State,
        string PostalCode,
        bool IsActive
    )
{
    public static CreateStudentResponse FromEntity(Domain.Entities.Student student)
    {
        return new CreateStudentResponse(
            student.Id,
            student.FirstName,
            student.LastName,
            student.DateOfBirth,
            student.Email,
            student.PhoneNumber,
            student.Address,
            student.City,
            student.State,
            student.PostalCode,
            student.IsActive
        );
    }
}

