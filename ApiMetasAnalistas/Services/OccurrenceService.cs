using ApiMetasAnalistas.Enums;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class OccurrenceService : IOccurrenceService
    {
        private readonly IUnityOfWork _repository;

        public OccurrenceService(IUnityOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Occurrence>> GetAllAsync()
        {
            return await _repository.OccurrenceRepository.GetAllAsync();
        }

        public async Task<Occurrence?> GetAsync(int id)
        {
            return await _repository.OccurrenceRepository.GetAsync(o => o.Id == id);
        }

        public async Task<Occurrence?> GetReadOnlyAsync(int id)
        {
            return await _repository.OccurrenceRepository.GetReadOnlyAsync(o => o.Id == id);
        }

        public async Task<Occurrence> AddAsync(Occurrence occurrence)
        {
            ArgumentNullException.ThrowIfNull(occurrence);

            if (occurrence.AnalistaId <= 0)
            {
                throw new ArgumentException($"Valor {occurrence.AnalistaId} inválido para o Id do analista", nameof(occurrence.AnalistaId));
            }
            if (string.IsNullOrEmpty(occurrence.Descricao))
            {
                throw new ArgumentException("A descrição da ocorrência é obrigatória", nameof(occurrence.Descricao));
            }
            if (occurrence.DataInicio == default)
            {
                throw new ArgumentException($"Valor {occurrence.DataInicio} inválido para a data da ocorrência", nameof(occurrence.DataInicio));
            }
            if (occurrence.DataFim == default)
            {
                throw new ArgumentException($"Valor {occurrence.DataFim} inválido para a data da ocorrência", nameof(occurrence.DataFim));
            }
            if (!System.Enum.IsDefined(typeof(TipoOcorrencia), occurrence.Tipo))
            {
                throw new ArgumentException($"Valor {occurrence.Tipo} inválido para a data da ocorrência", nameof(occurrence.Tipo));
            }

            try
            {
                _repository.OccurrenceRepository.Add(occurrence);
                await _repository.Commit();
                return occurrence;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao adicionar a ocorrência", e);
            }
        }

        public async Task<Occurrence> UpdateAsync(int id, Occurrence occurrence)
        {
            ArgumentNullException.ThrowIfNull(occurrence);

            var existingOccurrence = await _repository.OccurrenceRepository.GetAsync(o => o.Id == id);

            if (existingOccurrence == null)
            {
                throw new KeyNotFoundException($"Ocorrência com id {id} não encontrada");
            }

            try
            {
                existingOccurrence.Tipo = occurrence.Tipo;
                existingOccurrence.Descricao = occurrence.Descricao;
                existingOccurrence.AnalistaId = occurrence.AnalistaId;
                existingOccurrence.DataInicio = occurrence.DataInicio;
                existingOccurrence.DataFim = occurrence.DataFim;

                _repository.OccurrenceRepository.Update(existingOccurrence);
                await _repository.Commit();
                return existingOccurrence;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao atualizar a ocorrência", e);
            }
        } 

        public async Task DeleteAsync(int id)
        {
            var existingOccurrence = await _repository.OccurrenceRepository.GetAsync(o => o.Id == id);

            if (existingOccurrence == null)
            {
                throw new KeyNotFoundException($"Ocorrência com id {id} não encontrada");
            }

            try
            {
                _repository.OccurrenceRepository.Delete(existingOccurrence);
                await _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao excluir a ocorrência", e);
            }
        }

        public async Task<IEnumerable<Occurrence>> GetByAnalystAsync(int analystId)
        {
            return await _repository.OccurrenceRepository.GetByAnalystAsync(analystId);
        }

        public async Task<IEnumerable<Occurrence>> GetByAnalystPeriodAsync(int analystId, DateTime startDate, DateTime endDate)
        {
            return await _repository.OccurrenceRepository.GetByAnalystPeriodAsync(analystId, startDate, endDate);
        }

        public async Task<IEnumerable<Occurrence>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _repository.OccurrenceRepository.GetByPeriodAsync(startDate, endDate);
        }

        public async Task<bool> HasOcurrencesAsync(int id, DateTime occurrenceDate)
        {
            return await _repository.OccurrenceRepository.HasOcurrencesAsync(id, occurrenceDate);
        }

    }
}
