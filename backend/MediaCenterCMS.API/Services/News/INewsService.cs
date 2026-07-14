using MediaCenterCMS.API.DTOs.News;

namespace MediaCenterCMS.API.Services.News;

public interface INewsService
{
    Task<NewsResponse> CreateAsync(
        CreateNewsRequest request,
        int userId);
}