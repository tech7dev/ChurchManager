using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class EducationService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    // Sunday School
    public async Task<PagedResult<SundaySchoolClassDto>?> GetSundaySchoolClassesAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<SundaySchoolClassDto>(await client.GetAsync($"api/v1/sundayschool/classes?page={page}&pageSize={pageSize}"));
    }

    // Growth School
    public async Task<PagedResult<GrowthSchoolCourseDto>?> GetGrowthSchoolCoursesAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<GrowthSchoolCourseDto>(await client.GetAsync($"api/v1/growthschool/courses?page={page}&pageSize={pageSize}"));
    }
}

public class SundaySchoolClassDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Level { get; set; } = string.Empty;
    public string? TeacherName { get; set; }
    public int EnrollmentCount { get; set; }
    public bool IsActive { get; set; }
}

public class GrowthSchoolCourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Level { get; set; } = string.Empty;
    public string? InstructorName { get; set; }
    public int? DurationWeeks { get; set; }
    public int EnrollmentCount { get; set; }
    public bool IsActive { get; set; }
}
