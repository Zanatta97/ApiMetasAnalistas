using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiMetasAnalistas.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IUnityOfWork _repository;

        public HolidayService(IUnityOfWork repository)
        {
            _repository = repository;
        }

        public IEnumerable<Holiday> GetAll()
        {
            return _repository.HolidayRepository.GetAll();
        }

        public Holiday? Get(int id)
        {
            return _repository.HolidayRepository.Get(h => h.Id == id);
        }

        public Holiday? GetReadOnly(int id)
        {
            return _repository.HolidayRepository.GetReadOnly(h => h.Id == id);
        }

        public IEnumerable<Holiday> GetByDate(DateTime data)
        {
            return _repository.HolidayRepository.GetByDate(data);
        }

        public IEnumerable<Holiday> GetByRegion(int regionId, DateTime data)
        {
            return _repository.HolidayRepository.GetByRegion(regionId, data);
        }

        public IEnumerable<Holiday> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            return _repository.HolidayRepository.GetByPeriod(startDate, endDate);
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
                _repository.HolidayRepository.Add(holiday);
                _repository.Commit();
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

            var existingHoliday = _repository.HolidayRepository.Get(h => h.Id == id);

            if (existingHoliday == null)
            {
                throw new KeyNotFoundException($"Feriado com id {id} não encontrado");
            }

            try
            {
                existingHoliday.Data = holiday.Data;
                existingHoliday.Descricao = holiday.Descricao;
                existingHoliday.RegiaoId = holiday.RegiaoId;

                _repository.HolidayRepository.Update(existingHoliday);
                _repository.Commit();
                return existingHoliday;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao atualizar o feriado", e);
            }
        }

        public void Delete(int id)
        {
            var existingHoliday = _repository.HolidayRepository.Get(h => h.Id == id);

            if (existingHoliday == null)
            {
                throw new KeyNotFoundException($"Feriado com id {id} não encontrado");
            }

            try
            {
                _repository.HolidayRepository.Delete(existingHoliday);
                _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao deletar o feriado", e);
            }
        }
    }
}
