﻿using Prof.Hub.Domain.Common;

namespace Prof.Hub.Domain.Entities
{
    public class Student : AuditableEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
      
        public bool IsActive { get; set; } = true;
    }
}
