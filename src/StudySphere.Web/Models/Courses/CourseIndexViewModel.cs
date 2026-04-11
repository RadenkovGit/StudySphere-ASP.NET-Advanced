using StudySphere.Web.Models.Paging;

namespace StudySphere.Web.Models.Courses;

public class CourseIndexViewModel
{
    public string? SearchTerm { get; set; }

    public int? CategoryId { get; set; }

    public IEnumerable<CourseCategoryOptionViewModel> Categories { get; set; } = new List<CourseCategoryOptionViewModel>();

    public PagedResult<CourseCardViewModel> PagedCourses { get; set; } = new();
}
