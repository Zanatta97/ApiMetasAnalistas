using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IAnalystService
    {
        public IEnumerable<Analyst> GetAll();
        public Analyst? Get(int id);
        public Analyst? GetReadOnly(int id);
        public Analyst? GetByUserName(string userName);
        public Analyst Add(Analyst analyst);
        public Analyst Update(int id, Analyst analyst);
        public void Delete(int id);
        public int GetTargetForPeriod(int id, DateTime startDate, DateTime endDate);
        public List<AnalystResultDTO> GetTargetResults(DateTime startDate, DateTime endDate);
    }
}
