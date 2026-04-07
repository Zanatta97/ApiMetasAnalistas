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

        public async Task<IEnumerable<Holiday>> GetAllAsync()
        {
            return await _repository.HolidayRepository.GetAllAsync();
        }

        public async Task<Holiday?> GetAsync(int id)
        {
            return await _repository.HolidayRepository.GetAsync(h => h.Id == id);
        }

        public async Task<Holiday?> GetReadOnlyAsync(int id)
        {
            return await _repository.HolidayRepository.GetReadOnlyAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Holiday>> GetByDateAsync(DateTime data)
        {
            return await _repository.HolidayRepository.GetByDateAsync(data);
        }

        public async Task<IEnumerable<Holiday>> GetByRegionAsync(int regionId, DateTime data)
        {
            return await _repository.HolidayRepository.GetByRegionAsync(regionId, data);
        }

        public async Task<IEnumerable<Holiday>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _repository.HolidayRepository.GetByPeriodAsync(startDate, endDate);
        }

        public async Task<Holiday> AddAsync(Holiday holiday)
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
                await _repository.Commit();
                return (holiday);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao adicionar o feriado", e);
            }
        }

        public async Task<Holiday> UpdateAsync(int id, Holiday holiday)
        {
            ArgumentNullException.ThrowIfNull(holiday);

            var existingHoliday = await _repository.HolidayRepository.GetAsync(h => h.Id == id);

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
                await _repository.Commit();
                return existingHoliday;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao atualizar o feriado", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingHoliday = await _repository.HolidayRepository.GetAsync(h => h.Id == id);

            if (existingHoliday == null)
            {
                throw new KeyNotFoundException($"Feriado com id {id} não encontrado");
            }

            try
            {
                _repository.HolidayRepository.Delete(existingHoliday);
                await _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao deletar o feriado", e);
            }
        }
    }
}
