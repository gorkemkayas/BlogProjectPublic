using BlogProject.Application.DTOs;
using System.Threading.Tasks;

namespace BlogProject.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetStatisticsAsync();
    }
}