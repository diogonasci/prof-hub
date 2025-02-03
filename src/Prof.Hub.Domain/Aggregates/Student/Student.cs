using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;


namespace Prof.Hub.Domain.Aggregates.Student
{
    public class Student : AuditableEntity, IAggregateRoot
    {
        private readonly List<PrivateClass.PrivateClass> _privateClasses = [];
        private readonly List<GroupClass.GroupClass> _groupClasses = [];
        private readonly List<EnrollmentHistory> _enrollmentHistory = [];
        private readonly List<TeacherFavorite> _favoriteTeachers = [];

        public StudentId Id { get; private set; }
        public StudentProfile Profile { get; private set; }
        public Balance Balance { get; private set; }
        public School School { get; private set; }

        public IReadOnlyList<PrivateClass.PrivateClass> PrivateClasses => _privateClasses.AsReadOnly();
        public IReadOnlyList<GroupClass.GroupClass> GroupClasses => _groupClasses.AsReadOnly();
        public IReadOnlyList<EnrollmentHistory> EnrollmentHistory => _enrollmentHistory.AsReadOnly();
        public IReadOnlyList<TeacherFavorite> FavoriteTeachers => _favoriteTeachers.AsReadOnly();


        private Student()
        {
        }

        public static Result<Student> Create(
            string name,
            string email,
            string phoneNumber
        )
        {
            var profileResult = StudentProfile.Create(name, email, phoneNumber);

            if (!profileResult.IsSuccess)
            {
                var errors = new List<ValidationError>();

                if (profileResult.ValidationErrors.Any()) errors.AddRange(profileResult.ValidationErrors);

                return Result.Invalid(errors);
            }

            var student = new Student
            {
                Id = StudentId.Create(),
                Profile = profileResult.Value,
            };

            return Result.Success(student);
        }

        public void UpdateProfile(StudentProfile profile)
        {
            Profile = profile;
            AddDomainEvent(new StudentProfileUpdatedEvent(Id, profile));
        }

        public Result<Student> AddToFavorites(TeacherId teacherId)
        {
            if (_favoriteTeachers.Any(f => f.TeacherId == teacherId))
                return Result.Invalid(new ValidationError("Professor já está nos favoritos."));

            _favoriteTeachers.Add(new TeacherFavorite(Id, teacherId));
            AddDomainEvent(new TeacherAddedToFavoritesEvent(Id, teacherId));

            return Result.Success();
        }
    }
}
