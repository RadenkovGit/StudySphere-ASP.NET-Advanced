using Microsoft.AspNetCore.Identity;

namespace StudySphere.Web.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();

    public ICollection<StudyTask> StudyTasks { get; set; } = new HashSet<StudyTask>();

    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}
