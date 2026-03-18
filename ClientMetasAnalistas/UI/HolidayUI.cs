using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Services;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMetasAnalistas.UI
{
    public class HolidayUI
    {
        private readonly IHolidayService _service = new HolidayService();

        public async Task ListHolidays()
        {
            string titulo = "Listar Feriados";
            var regionUI = new RegionUI();
            IEnumerable<HolidayDTO> holidays = new List<HolidayDTO>();
            DateTime dataInicio = default, dataFim = default;

            bool filtrarPorRegiao = Menus.MenuSimNao("Deseja filtrar por região?", titulo);

            int? regionId = null;
            if (filtrarPorRegiao)
            {
                regionId = await regionUI.SelectRegion();
                if (regionId == 0)
                {
                    Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                    return;
                }
            }

            bool filtrarPorPeriodo = Menus.MenuSimNao("Deseja filtrar por período?", titulo);

            if (filtrarPorPeriodo)
            {
                dataInicio = Utils.ValidaData(titulo, "Informe a data de início (dd/MM/yyyy): ");
                dataFim = Utils.ValidaData(titulo, "Informe a data de fim (dd/MM/yyyy): ");
                
                if (dataFim < dataInicio)
                {
                    Utils.ImprimirAviso(new[] { "Data de fim não pode ser anterior à data de início." }, titulo);
                    return;
                }
                Utils.TelaLoading("Carregando feriados", titulo);
                holidays = await _service.GetHolidaysByPeriodAsync(dataInicio, dataFim);
            }
            else
            {
                Utils.TelaLoading("Carregando feriados", titulo);
                holidays = await _service.GetAllHolidaysAsync();
            }

            if (!holidays.Any())
            {
                Utils.ImprimirAviso(new[] { "Nenhum feriado encontrado." }, titulo);
                return;
            }

            // Filtrar por região se necessário
            if (regionId.HasValue)
            {
                holidays = holidays.Where(h => h.RegiaoId == regionId.Value).ToList();
                
                if (!holidays.Any())
                {
                    Utils.ImprimirAviso(new[] { "Nenhum feriado encontrado para esta região." }, titulo);
                    return;
                }
            }

            var regionService = new RegionService();
            var regions = await regionService.GetAllRegionsAsync();

            Utils.ImprimirTitulo(titulo);
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir("FERIADOS CADASTRADOS", 90, ' ', 1);

            if (dataInicio != default && dataFim != default)
            {
                Utils.Imprimir($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}", 90, ' ', 1);
            }
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"{"ID",-5} | {"DATA",-12} | {"REGIÃO",-10} | {"DESCRIÇÃO",-50}", 90, ' ', 0);
            Utils.Imprimir("-", 90, '-', 0);

            foreach (var holiday in holidays.OrderBy(h => h.Data))
            {
                string data = holiday.Data.ToString("dd/MM/yyyy");
                string regiaoNome = regions.FirstOrDefault(r => r.Id == holiday.RegiaoId)?.Nome ?? "Nacional";
                Utils.Imprimir($"{holiday.Id,-5} | {data,-12} | {regiaoNome,-10} | {holiday.Descricao,-50}", 90, ' ', 0);
                Utils.Imprimir("-", 90, '-', 0);
            }

            Utils.Break();
        }

        public async Task InsertHoliday()
        {
            string titulo = "Registrar Feriado";
            var regionUI = new RegionUI();

            var holidayDTO = new HolidayDTO();

            // Validar descrição
            holidayDTO.Descricao = Utils.ValidaString("Informe a descrição do feriado: ", titulo, "Descrição");

            // Validar data
            holidayDTO.Data = Utils.ValidaData(titulo, "Informe a data do feriado (dd/MM/yyyy): ");

            // Selecionar região
            holidayDTO.RegiaoId = await regionUI.SelectRegion();

            if (holidayDTO.RegiaoId == 0)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            Utils.TelaLoading("Registrando feriado", titulo);
            var result = await _service.InsertHolidayAsync(holidayDTO);

            if (result != null && result.Id > 0)
            {
                Utils.ImprimirAviso(new[] { $"Feriado '{result.Descricao}' registrado com sucesso!" }, titulo, false);
            }
            else
            {
                Utils.ImprimirAviso(new[] { "Erro ao registrar o feriado. Tente novamente." }, titulo, true);
            }
        }

        public async Task InsertUpdateHoliday()
        {
            string titulo = "Gerenciar Feriados";
            int escolhaCadastro = Menus.MenuCadastro("Feriado");

            if (escolhaCadastro == 3) // Voltar ao menu principal
            {
                Console.Clear();
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
            }
            else if (escolhaCadastro == 1) // Cadastrar novo
            {
                await InsertHoliday();
            }
            else // Alterar
            {
                await UpdateHoliday();
            }
        }

        public async Task UpdateHoliday()
        {
            string titulo = "Alterar Feriado";

            Utils.TelaLoading("Carregando feriados", titulo);
            var holidays = await _service.GetAllHolidaysAsync();

            if (!holidays.Any())
            {
                Utils.ImprimirAviso(new[] { "Nenhum feriado encontrado." }, titulo);
                return;
            }

            List<string> holidaysString = new List<string>();
            foreach (var holiday in holidays.OrderBy(h => h.Data))
            {
                holidaysString.Add($"[{holiday.Id}] {holiday.Data:dd/MM/yyyy} - Região {holiday.RegiaoId} - {holiday.Descricao}");
            }

            holidaysString.Add("Voltar ao Menu Principal");

            int escolha = Menus.CriarMenuNavegavel(titulo, holidaysString.ToArray(), "Selecione o feriado que deseja alterar:");

            if (escolha + 1 == holidaysString.Count)
            {
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                return;
            }

            var selectedHoliday = holidays.OrderBy(h => h.Data).ElementAt(escolha);

            // Atualizar campos
            selectedHoliday.Descricao = Utils.ValidaString("Informe a nova descrição: ", titulo, "Descrição");
            selectedHoliday.Data = Utils.ValidaData(titulo, "Informe a nova data (dd/MM/yyyy): ");

            var regionUI = new RegionUI();
            selectedHoliday.RegiaoId = await regionUI.SelectRegion();

            if (selectedHoliday.RegiaoId == 0)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            Utils.TelaLoading("Atualizando feriado", titulo);
            var result = await _service.UpdateHolidayAsync(selectedHoliday);

            if (result != null && result.Id > 0)
            {
                Utils.ImprimirAviso(new[] { "Feriado atualizado com sucesso!" }, titulo, false);
            }
            else
            {
                Utils.ImprimirAviso(new[] { "Erro ao atualizar o feriado. Tente novamente." }, titulo, true);
            }
        }

        public async Task DeleteHoliday()
        {
            string titulo = "Excluir Feriado";

            Utils.TelaLoading("Carregando feriados", titulo);
            var holidays = await _service.GetAllHolidaysAsync();

            if (!holidays.Any())
            {
                Utils.ImprimirAviso(new[] { "Nenhum feriado encontrado." }, titulo);
                return;
            }

            List<string> holidaysString = new List<string>();
            foreach (var holiday in holidays.OrderBy(h => h.Data))
            {
                holidaysString.Add($"[{holiday.Id}] {holiday.Data:dd/MM/yyyy} - Região {holiday.RegiaoId} - {holiday.Descricao}");
            }

            holidaysString.Add("Voltar ao Menu Principal");

            int escolha = Menus.CriarMenuNavegavel(titulo, holidaysString.ToArray(), "Selecione o feriado que deseja excluir:");

            if (escolha + 1 == holidaysString.Count)
            {
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                return;
            }

            var selectedHoliday = holidays.OrderBy(h => h.Data).ElementAt(escolha);

            bool confirmar = Menus.MenuSimNao($"Deseja realmente excluir o feriado '{selectedHoliday.Descricao}' de {selectedHoliday.Data:dd/MM/yyyy}?", titulo);

            if (!confirmar)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            Utils.TelaLoading("Excluindo feriado", titulo);
            var result = await _service.DeleteHolidayAsync(selectedHoliday.Id);

            if (result)
            {
                Utils.ImprimirAviso(new[] { "Feriado excluído com sucesso!" }, titulo, false);
            }
            else
            {
                Utils.ImprimirAviso(new[] { "Erro ao excluir o feriado. Tente novamente." }, titulo, true);
            }
        }
    }
}