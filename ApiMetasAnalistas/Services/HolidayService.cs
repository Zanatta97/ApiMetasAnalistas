using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _repository;

        public HolidayService(IHolidayRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Holiday> GetAll()
        {
            return _repository.GetAll();
        }

        public Holiday? Get(int id)
        {
            return _repository.Get(id);
        }

        public Holiday? GetReadOnly(int id)
        {
            return _repository.GetReadOnly(id);
        }

        public IEnumerable<Holiday> GetByDate(DateTime data)
        {
            return _repository.GetByDate(data);
        }

        public IEnumerable<Holiday> GetByRegion(int regionId, DateTime data)
        {
            return _repository.GetByRegion(regionId, data);
        }

        public Holiday Add(Holiday holiday)
        {
            ArgumentNullException.ThrowIfNull(holiday);
            if (string.IsNullOrEmpty(holiday.Descricao))
            {
                throw new ArgumentException("A descrição do feriado é obrigatória", nameof(holiday.Descricao));
            }
            if (holiday.Data == default)
            {
                throw new ArgumentException("A data do feriado é obrigatória", nameof(holiday.Data));
            }
            if (holiday.RegiaoId <= 0)
            {
                throw new ArgumentException("A data do feriado é obrigatória", nameof(holiday.RegiaoId));
            }

            try
            {
                _repository.Add(holiday);
                return (holiday);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao adicionar o feriado", e);
            }
        }

        public Holiday Update(int id, Holiday holiday)
        {
            ArgumentNullException.ThrowIfNull(holiday);

            var existingHoliday = _repository.Get(id);

            if (existingHoliday == null)
            {
                throw new KeyNotFoundException($"Feriado com id {id} não encontrado");
            }

            try
            {
                existingHoliday.Data = holiday.Data;
                existingHoliday.Descricao = holiday.Descricao;
                existingHoliday.RegiaoId = holiday.RegiaoId;

                _repository.Update(existingHoliday);
                return existingHoliday;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao atualizar o feriado", e);
            }
        }

        public void Delete(int id)
        {
            var existingHoliday = _repository.Get(id);

            if (existingHoliday == null)
            {
                throw new KeyNotFoundException($"Feriado com id {id} não encontrado");
            }

            try
            {
                _repository.Delete(existingHoliday);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao deletar o feriado", e);
            }
        }
    }
}
