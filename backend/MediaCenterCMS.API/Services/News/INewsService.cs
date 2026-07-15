using MediaCenterCMS.API.DTOs.News;

namespace MediaCenterCMS.API.Services.News;

public interface INewsService
{
    Task<NewsResponse> CreateAsync(
        CreateNewsRequest request,
        int userId);

    Task<IEnumerable<NewsResponse>> GetAllAsync();
    Task<NewsResponse?> GetByIdAsync(int id);
    Task<NewsResponse?> UpdateAsync(
    int id,
    UpdateNewsRequest request,
    int userId);
}