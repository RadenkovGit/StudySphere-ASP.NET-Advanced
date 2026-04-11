using StudySphere.Web.Models.Lessons;
using StudySphere.Web.Models.Resources;
using StudySphere.Web.Models.Reviews;

namespace StudySphere.Web.Models.Courses;

public class CourseDetailsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string InstructorName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public int DurationWeeks { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsEnrolled { get; set; }

    public double AverageRating { get; set; }

    public IEnumerable<LessonListItemViewModel> Lessons { get; set; } = new List<LessonListItemViewModel>();

    public IEnumerable<ResourceListItemViewModel> Resources { get; set; } = new List<ResourceListItemViewModel>();

    public IEnumerable<ReviewListItemViewModel> Reviews { get; set; } = new List<ReviewListItemViewModel>();

    public CreateReviewInputModel NewReview { get; set; } = new();
}
