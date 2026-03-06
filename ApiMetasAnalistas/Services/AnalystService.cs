using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class AnalystService : IAnalystService
    {
        private readonly IAnalystRepository _repository;

        public AnalystService(IAnalystRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Analyst> GetAll()
        {
            return _repository.GetAll();
        }

        public Analyst? Get(int id)
        {
            return _repository.Get(id);
        }

        /// <summary>
        /// Retorna o analista sem Tracking.
        /// Melhora o desempenho quando não há necessidade de alteração do objeto.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Analyst? GetReadOnly(int id)
        {
            return _repository.GetReadOnly(id);
        }

        public Analyst? GetByUserName(string userName)
        {
            return _repository.GetByUserName(userName);
        }

        public Analyst Add(Analyst analyst)
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

            if (_repository.GetByUserName(analyst.Usuario) != null)
            {
                throw new InvalidOperationException($"O nome de usuário {analyst.Usuario} já existe");
            }

            try
            {
                _repository.Add(analyst);
                return analyst;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao adicionar analista", ex);
            }
        }

        public Analyst Update(int id, Analyst analyst)
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

            var existingAnalyst = Get(id);

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

                _repository.Update(existingAnalyst);
                return existingAnalyst;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao alterar analista", ex);
            }
        }

        public void Delete(int id)
        {

            var existingAnalyst = Get(id);

            if (existingAnalyst == null)
            {
                throw new KeyNotFoundException($"Analista com ID {id} não encontrado");
            }

            if (_repository.HasOcurrences(existingAnalyst.Id))
            {
                throw new InvalidOperationException("Não é possível excluir o analista porque ele tem ocorrências associadas");
            }

            if (_repository.HasTickets(existingAnalyst.Id))
            {
                throw new InvalidOperationException("Não é possível excluir o analista porque ele tem chamados associados");
            }

            try
            {
                _repository.Delete(existingAnalyst);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao excluir analista", e);
            }

        }


        public int GetTargetForPeriod(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var analyst = GetReadOnly(id);

                if (analyst is null)
                    throw new KeyNotFoundException($"Analista com ID {id} não encontrado");

                var totalDays = 0;

                for (int i = 0; startDate.Date.AddDays(i) <= endDate.Date; i++)
                {
                    var currentDate = startDate.Date.AddDays(i);

                    var isHoliday = _repository.IsHoliday(analyst, currentDate);

                    var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;

                    if (!isHoliday && !isWeekend)
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

        public List<AnalystResultDTO> GetTargetResults(DateTime startDate, DateTime endDate)
        {
            try
            {
                var analysts = _repository.GetAll();

                if (!analysts.Any())
                    throw new KeyNotFoundException("Nenhum analista encontrado");

                var targetResults = new List<AnalystResultDTO>();

                foreach (var analyst in analysts)
                {
                    var totalDays = GetTargetForPeriod(analyst.Id, startDate, endDate);

                    var ticketsFechados = _repository.TicketCount(analyst.Id, startDate, endDate);

                    targetResults.Add(new AnalystResultDTO
                    {
                        AnalistaId = analyst.Id,
                        NomeAnalista = analyst.Nome,
                        RegiaoId = analyst.RegiaoId,
                        TotalDiasUteis = totalDays,
                        MetaDiaria = analyst.MetaDiaria,
                        TotalMetaPeriodo = analyst.MetaDiaria * totalDays,
                        TicketsFechados = ticketsFechados,
                        PercentualMetaAlcancada = totalDays > 0 ? (decimal)ticketsFechados / (analyst.MetaDiaria * totalDays) * 100m : 0m

                    });

                }

                return targetResults;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
