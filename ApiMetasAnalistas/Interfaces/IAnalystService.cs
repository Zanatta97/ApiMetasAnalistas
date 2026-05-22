using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IAnalystService
    {
        public Task<IEnumerable<Analyst>> GetAllAsync();
        public Task<Analyst?> GetAsync(int id);
        public Task<Analyst?> GetReadOnlyAsync(int id);
        public Task<Analyst?> GetByUserNameAsync(string userName);
        public Task<Analyst> AddAsync(Analyst analyst);
        public Task<Analyst> UpdateAsync(int id, Analyst analyst);
        public Task DeleteAsync(int id);
        public Task<int> GetTargetForPeriodAsync(int id, DateTime startDate, DateTime endDate);
        public Task<List<AnalystResultDTO>> GetTargetResultsAsync(DateTime startDate, DateTime endDate);
        public Task<AnalystResultDTO> GetAnalystTargetResultsAsync(DateTime startDate, DateTime endDate, Analyst analyst);
    }
}
