using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class AnalystService : IAnalystService
    {
        //private readonly IAnalystRepository _repository;
        private readonly IUnityOfWork _repository;

        public AnalystService(IUnityOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Analyst>> GetAllAsync()
        {
            return await _repository.AnalystRepository.GetAllAsync();
        }

        public async Task<Analyst?> GetAsync(int id)
        {
            return await _repository.AnalystRepository.GetAsync(a => a.Id == id);
        }

        /// <summary>
        /// Retorna o analista sem Tracking.
        /// Melhora o desempenho quando não há necessidade de alteração do objeto.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Analyst?> GetReadOnlyAsync(int id)
        {
            return await _repository.AnalystRepository.GetReadOnlyAsync(a => a.Id == id);
        }

        public async Task<Analyst?> GetByUserNameAsync(string userName)
        {
            return await _repository.AnalystRepository.GetByUserNameAsync(userName);
        }

        public async Task<Analyst> AddAsync(Analyst analyst)
        {
            ArgumentNullException.ThrowIfNull(analyst);

            if (string.IsNullOrEmpty(analyst.Nome))
            {
                throw new ArgumentException("O nome do analista é obrigatório", nameof(analyst.Nome));
            }

            if (string.IsNullOrEmpty(analyst.Usuario))
            {
                throw new ArgumentException("O nome de usuário do analista é obrigatório", nameof(analyst.Usuario));
            }

            if (analyst.MetaDiaria <= 0)
            {
                throw new ArgumentException($"Valor {analyst.MetaDiaria} inválido para a Meta do analista", nameof(analyst.MetaDiaria));
            }

            if (await _repository.AnalystRepository.GetByUserNameAsync(analyst.Usuario) != null)
            {
                throw new InvalidOperationException($"O nome de usuário {analyst.Usuario} já existe");
            }

            try
            {
                _repository.AnalystRepository.Add(analyst);
                await _repository.Commit();
                return analyst;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao adicionar analista", e);
            }
        }

        public async Task<Analyst> UpdateAsync(int id, Analyst analyst)
        {
            //Outra forma de fazer isto seria utilizando o EntityState.Modified,
            //mas isso pode levar a problemas de segurança, como ataques de overposting,
            //onde um cliente mal-intencionado pode enviar dados adicionais que não deveriam ser atualizados.
            //Além disso, o uso do EntityState.Modified pode resultar em atualizações acidentais de campos que não foram
            //intencionalmente modificados, especialmente se o modelo tiver muitos campos ou relacionamentos complexos.
            //Portanto, é recomendado buscar o registro existente no banco de dados,
            //atualizar apenas os campos necessários e depois salvar as alterações para garantir um controle
            //mais preciso sobre o processo de atualização.
            /*if (id != analyst.Id)
                return BadRequest("ID do analista não corresponde ao ID fornecido na URL");

            _context.Entry(analyst).State = EntityState.Modified;
            _context.SaveChanges();*/

            ArgumentNullException.ThrowIfNull(analyst);

            var existingAnalyst = await GetAsync(id);

            if (existingAnalyst == null)
            {
                throw new KeyNotFoundException($"Analista com ID {id} não encontrado");
            }

            try
            {
                existingAnalyst.Nome = analyst.Nome;
                existingAnalyst.Usuario = analyst.Usuario;
                existingAnalyst.RegiaoId = analyst.RegiaoId;
                existingAnalyst.MetaDiaria = analyst.MetaDiaria;

                _repository.AnalystRepository.Update(existingAnalyst);
                await _repository.Commit();
                return existingAnalyst;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao alterar analista", e);
            }
        }

        public async Task DeleteAsync(int id)
        {

            var existingAnalyst = await GetAsync(id);

            if (existingAnalyst == null)
            {
                throw new KeyNotFoundException($"Analista com ID {id} não encontrado");
            }

            if (await _repository.AnalystRepository.HasOccurrencesAsync(existingAnalyst.Id))
            {
                throw new InvalidOperationException("Não é possível excluir o analista porque ele tem ocorrências associadas");
            }

            if (await _repository.AnalystRepository.HasTicketsAsync(existingAnalyst.Id))
            {
                throw new InvalidOperationException("Não é possível excluir o analista porque ele tem chamados associados");
            }

            try
            {
                _repository.AnalystRepository.Delete(existingAnalyst);
                await _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao excluir analista", e);
            }

        }


        public async Task<int> GetTargetForPeriodAsync(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var analyst = await GetReadOnlyAsync(id);

                if (analyst is null)
                    throw new KeyNotFoundException($"Analista com ID {id} não encontrado");

                var totalDays = 0;

                for (int i = 0; startDate.Date.AddDays(i) <= endDate.Date; i++)
                {
                    var currentDate = startDate.Date.AddDays(i);

                    var isHoliday = await _repository.AnalystRepository.IsHolidayAsync(analyst, currentDate);

                    var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;

                    var hasOccurrence = await _repository.AnalystRepository.HasOccurrenceAsync(analyst.Id, currentDate);

                    if (!isHoliday && !isWeekend && !hasOccurrence)
                        totalDays++;
                }

                var targetForPeriod = analyst.MetaDiaria * totalDays;

                return targetForPeriod;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AnalystResultDTO>> GetTargetResultsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var analysts = await _repository.AnalystRepository.GetAllAsync();

                if (!analysts.Any())
                    throw new KeyNotFoundException("Nenhum analista encontrado");

                var targetResults = new List<AnalystResultDTO>();

                foreach (var analyst in analysts)
                {
                    var totalTarget = await GetTargetForPeriodAsync(analyst.Id, startDate, endDate);
                    var ticketsFechados = await _repository.AnalystRepository.TicketCountAsync(analyst.Id, startDate, endDate);

                    targetResults.Add(new AnalystResultDTO
                    {
                        AnalistaId = analyst.Id,
                        NomeAnalista = analyst.Nome,
                        RegiaoId = analyst.RegiaoId,
                        TotalDiasUteis = await AnalystTotalDaysAsync(startDate, endDate, analyst),
                        MetaDiaria = analyst.MetaDiaria,
                        TotalMetaPeriodo = totalTarget,
                        TicketsFechados = ticketsFechados,
                        PercentualMetaAlcancada = totalTarget > 0 ? (decimal)ticketsFechados / totalTarget * 100m : 0m

                    });

                }

                return targetResults;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AnalystResultDTO> GetAnalystTargetResultsAsync(DateTime startDate, DateTime endDate, Analyst analyst)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(analyst);

                var totalTarget = await GetTargetForPeriodAsync(analyst.Id, startDate, endDate);
                var ticketsFechados = await _repository.AnalystRepository.TicketCountAsync(analyst.Id, startDate, endDate);

                var targetResults = new AnalystResultDTO
                {
                    AnalistaId = analyst.Id,
                    NomeAnalista = analyst.Nome,
                    RegiaoId = analyst.RegiaoId,
                    TotalDiasUteis = await AnalystTotalDaysAsync(startDate, endDate, analyst),
                    MetaDiaria = analyst.MetaDiaria,
                    TotalMetaPeriodo = totalTarget,
                    TicketsFechados = ticketsFechados,
                    PercentualMetaAlcancada = totalTarget > 0 ? (decimal)ticketsFechados / totalTarget * 100m : 0m

                };

                return targetResults;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> AnalystTotalDaysAsync(DateTime startDate, DateTime endDate, Analyst analyst)
        {
            var totalDays = 0;

            for (int i = 0; startDate.Date.AddDays(i) <= endDate.Date; i++)
            {
                var currentDate = startDate.Date.AddDays(i);

                var isHoliday = await _repository.AnalystRepository.IsHolidayAsync(analyst, currentDate);

                var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;

                if (!isHoliday && !isWeekend)
                    totalDays++;
            }
            return totalDays;
        }

    }
}
