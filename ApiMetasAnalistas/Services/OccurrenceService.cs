using ApiMetasAnalistas.Enum;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class OccurrenceService : IOccurrenceService
    {
        private readonly IOccurrenceRepository _repository;

        public OccurrenceService(IOccurrenceRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Occurrence> GetAll()
        {
            return _repository.GetAll();
        }

        public Occurrence? Get(int id)
        {
            return _repository.Get(id);
        }

        public Occurrence? GetReadOnly(int id)
        {
            return _repository.GetReadOnly(id);
        }

        public Occurrence Add(Occurrence occurrence)
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
                _repository.Add(occurrence);
                return occurrence;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao adicionar a ocorrência", e);
            }
        }

        public Occurrence Update(int id, Occurrence occurrence)
        {
            ArgumentNullException.ThrowIfNull(occurrence);

            var existingOccurrence = _repository.Get(id);

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

                _repository.Update(existingOccurrence);
                return existingOccurrence;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao atualizar a ocorrência", e);
            }
        } 

        public void Delete(int id)
        {
            var existingOccurrence = _repository.Get(id);

            if (existingOccurrence == null)
            {
                throw new KeyNotFoundException($"Ocorrência com id {id} não encontrada");
            }

            try
            {
                _repository.Delete(existingOccurrence);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao excluir a ocorrência", e);
            }
        }

        public IEnumerable<Occurrence> GetByAnalyst(int analystId)
        {
            return _repository.GetByAnalyst(analystId);
        }

        public IEnumerable<Occurrence> GetByAnalystPeriod(int analystId, DateTime startDate, DateTime endDate)
        {
            return _repository.GetByAnalystPeriod(analystId, startDate, endDate);
        }

        public IEnumerable<Occurrence> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            return _repository.GetByPeriod(startDate, endDate);
        }

        public bool HasOcurrences(int id, DateTime occurrenceDate)
        {
            return _repository.HasOcurrences(id, occurrenceDate);
        }

    }
}
