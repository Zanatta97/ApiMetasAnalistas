using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Enum;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Services;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.UI
{
    public class OccurrenceUI
    {
        private readonly IOccurrenceService _service = new OccurrenceService();

        public async Task ListOccurrencesByAnalyst(int idAnalyst)
        {
            string titulo = $"Ocorrências Analista";
            var analystUI = new AnalystUI();

            var analyst = await analystUI.GetAnalystMenu();

            if (analyst == null || analyst.Id != idAnalyst)
            {
                Utils.ImprimirAviso(new[] { "Analista não encontrado." }, titulo, true);
                return;
            }

            Utils.TelaLoading("Carregando ocorrências", titulo);
            var occurrences = await _service.GetOccurrencesByAnalyst(idAnalyst);

            if (!occurrences.Any())
            {
                Utils.ImprimirAviso(new[] { "Nenhuma ocorrência encontrada para este analista." }, titulo);
                return;
            }


            Utils.ImprimirTitulo(titulo);
            Utils.Imprimir("==", 120, '=', 1);
            Utils.Imprimir($"OCORRÊNCIAS - {analyst.Nome.ToUpper()}", 120, ' ', 1);
            Utils.Imprimir("==", 120, '=', 1);
            Utils.Imprimir($"{"ID",-5} | {"TIPO",-15} | {"DATA INÍCIO",-12} | {"DATA FIM",-12} | {"DESCRIÇÃO",-40}", 120, ' ', 0);
            Utils.Imprimir("-", 120, '-', 0);

            foreach (var occurrence in occurrences)
            {
                string tipoTexto = ObterDescricaoTipo(occurrence.Tipo);
                string dataInicio = occurrence.DataInicio.ToString("dd/MM/yyyy");
                string dataFim = occurrence.DataFim.ToString("dd/MM/yyyy");
                Utils.Imprimir($"{occurrence.Id,-5} | {tipoTexto,-15} | {dataInicio, -12} | {dataFim,-12} | {occurrence.Descricao,-40}", 120, ' ', 0);
                Utils.Imprimir("-", 120, '-', 0);
            }

            Utils.Break();
        }

        public async Task ListOccurrencesByAnalystPeriod()
        {
            string titulo = "Ocorrências por Período";
            var analystUI = new AnalystUI();

            var analyst = await analystUI.GetAnalystMenu();

            if (analyst == null)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            var startDate = Utils.ValidaData(titulo, "Informe a data inicial (dd/MM/yyyy): ");
            var endDate = Utils.ValidaData(titulo, "Informe a data final (dd/MM/yyyy): ");

            if (startDate > endDate)
            {
                Utils.ImprimirAviso(new[] { "Data inicial não pode ser maior que data final!" }, titulo, true);
                return;
            }

            Utils.TelaLoading("Carregando ocorrências", titulo);
            var occurrences = await _service.GetByAnalystAndPeriod(analyst.Id, startDate, endDate);

            if (!occurrences.Any())
            {
                Utils.ImprimirAviso(new[] { $"Nenhuma ocorrência encontrada para o período de {startDate:dd/MM/yyyy} até {endDate:dd/MM/yyyy}." }, titulo);
                return;
            }

            Utils.ImprimirTitulo(titulo);
            Utils.Imprimir("==", 120, '=', 1);
            Utils.Imprimir($"OCORRÊNCIAS - {analyst.Nome.ToUpper()}", 120, ' ', 1);
            Utils.Imprimir($"{startDate:dd/MM/yyyy} até {endDate:dd/MM/yyyy}", 120, ' ', 1);
            Utils.Imprimir("==", 120, '=', 1);
            Utils.Imprimir($"{"ID",-5} | {"TIPO",-15} | {"DATA INÍCIO",-12} | {"DATA FIM",-12} | {"DESCRIÇÃO",-40}", 120, ' ', 0);
            Utils.Imprimir("-", 120, '-', 0);

            foreach (var occurrence in occurrences)
            {
                string tipoTexto = ObterDescricaoTipo(occurrence.Tipo);
                string dataInicio = occurrence.DataInicio.ToString("dd/MM/yyyy");
                string dataFim = occurrence.DataFim.ToString("dd/MM/yyyy");
                Utils.Imprimir($"{occurrence.Id,-5} | {tipoTexto,-15} | {dataInicio,-12} | {dataFim,-12} | {occurrence.Descricao,-40}", 120, ' ', 0);
                Utils.Imprimir("-", 120, '-', 0);
            }

            Utils.Break();
        }

        public async Task InsertOccurrence()
        {
            string titulo = "Registrar Ocorrência";
            var analystUI = new AnalystUI();

            var analyst = await analystUI.GetAnalystMenu();

            if (analyst == null)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            var occurrenceDTO = new OccurrenceDTO
            {
                AnalistaId = analyst.Id
            };

            // Selecionar tipo de ocorrência
            occurrenceDTO.Tipo = SelectOccurrenceType(titulo);

            // Validar descrição
            occurrenceDTO.Descricao = Utils.ValidaString("Informe a descrição da ocorrência: ", titulo, "Descrição");

            // Validar datas
            occurrenceDTO.DataInicio = Utils.ValidaData(titulo, "Informe a data de início (dd/MM/yyyy): ");
            occurrenceDTO.DataFim = Utils.ValidaData(titulo, "Informe a data de fim (dd/MM/yyyy): ");

            if (occurrenceDTO.DataInicio > occurrenceDTO.DataFim)
            {
                Utils.ImprimirAviso(new[] { "Data de início não pode ser maior que data de fim!" }, titulo, true);
                return;
            }

            Utils.TelaLoading("Registrando ocorrência", titulo);
            var result = await _service.InsertOccurrenceAsync(occurrenceDTO);

            if (result != null && result.Id > 0)
            {
                Utils.ImprimirAviso(new[] { $"Ocorrência '{ObterDescricaoTipo(result.Tipo)}' registrada com sucesso para o analista {analyst.Nome}!" }, titulo, false);
            }
            else
            {
                Utils.ImprimirAviso(new[] { "Erro ao registrar a ocorrência. Tente novamente." }, titulo, true);
            }
        }

        public async Task InsertUpdateOccurrence()
        {
            string titulo = "Gerenciar Ocorrências";
            int escolhaCadastro = Menus.MenuCadastro("Ocorrência");

            if (escolhaCadastro == 3) // Voltar ao menu principal
            {
                Console.Clear();
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
            }
            else if (escolhaCadastro == 1) // Cadastrar novo
            {
                await InsertOccurrence();
            }
            else // Alterar
            {
                await UpdateOccurrence();
            }
        }

        public async Task UpdateOccurrence()
        {
            string titulo = "Alterar Ocorrência";
            var analystUI = new AnalystUI();

            var analyst = await analystUI.GetAnalystMenu();

            if (analyst == null)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            Utils.TelaLoading("Carregando ocorrências", titulo);
            var occurrences = await _service.GetOccurrencesByAnalyst(analyst.Id);

            if (!occurrences.Any())
            {
                Utils.ImprimirAviso(new[] { "Nenhuma ocorrência encontrada para este analista." }, titulo);
                return;
            }

            List<string> occurrencesString = new List<string>();
            foreach (var occ in occurrences.OrderBy(o => o.DataInicio))
            {
                string tipoTexto = ObterDescricaoTipo(occ.Tipo);
                occurrencesString.Add($"[{occ.Id}] {tipoTexto} - {occ.DataInicio:dd/MM/yyyy} até {occ.DataFim:dd/MM/yyyy} - {occ.Descricao}");
            }

            occurrencesString.Add("Voltar ao Menu Principal");

            int escolha = Menus.CriarMenuNavegavel(titulo, occurrencesString.ToArray(), "Selecione a ocorrência que deseja alterar:");

            if (escolha + 1 == occurrencesString.Count)
            {
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                return;
            }

            var selectedOccurrence = occurrences.OrderBy(o => o.DataInicio).ElementAt(escolha);

            // Atualizar campos
            selectedOccurrence.Tipo = SelectOccurrenceType(titulo);
            selectedOccurrence.Descricao = Utils.ValidaString("Informe a nova descrição: ", titulo, "Descrição");
            selectedOccurrence.DataInicio = Utils.ValidaData(titulo, "Informe a nova data de início (dd/MM/yyyy): ");
            selectedOccurrence.DataFim = Utils.ValidaData(titulo, "Informe a nova data de fim (dd/MM/yyyy): ");

            if (selectedOccurrence.DataInicio > selectedOccurrence.DataFim)
            {
                Utils.ImprimirAviso(new[] { "Data de início não pode ser maior que data de fim!" }, titulo, true);
                return;
            }

            Utils.TelaLoading("Atualizando ocorrência", titulo);
            var result = await _service.UpdateOccurrenceAsync(selectedOccurrence);

            if (result != null && result.Id > 0)
            {
                Utils.ImprimirAviso(new[] { "Ocorrência atualizada com sucesso!" }, titulo, false);
            }
            else
            {
                Utils.ImprimirAviso(new[] { "Erro ao atualizar a ocorrência. Tente novamente." }, titulo, true);
            }
        }

        public async Task DeleteOccurrence()
        {
            string titulo = "Excluir Ocorrência";
            var analystUI = new AnalystUI();

            var analyst = await analystUI.GetAnalystMenu();

            if (analyst == null)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            Utils.TelaLoading("Carregando ocorrências", titulo);
            var occurrences = await _service.GetOccurrencesByAnalyst(analyst.Id);

            if (!occurrences.Any())
            {
                Utils.ImprimirAviso(new[] { "Nenhuma ocorrência encontrada para este analista." }, titulo);
                return;
            }

            List<string> occurrencesString = new List<string>();
            foreach (var occ in occurrences.OrderBy(o => o.DataInicio))
            {
                string tipoTexto = ObterDescricaoTipo(occ.Tipo);
                occurrencesString.Add($"[{occ.Id}] {tipoTexto} - {occ.DataInicio:dd/MM/yyyy} até {occ.DataFim:dd/MM/yyyy} - {occ.Descricao}");
            }

            occurrencesString.Add("Voltar ao Menu Principal");

            int escolha = Menus.CriarMenuNavegavel(titulo, occurrencesString.ToArray(), "Selecione a ocorrência que deseja excluir:");

            if (escolha + 1 == occurrencesString.Count)
            {
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                return;
            }

            var selectedOccurrence = occurrences.OrderBy(o => o.DataInicio).ElementAt(escolha);

            bool confirmar = Menus.MenuSimNao($"Deseja realmente excluir a ocorrência '{ObterDescricaoTipo(selectedOccurrence.Tipo)}' de {selectedOccurrence.DataInicio:dd/MM/yyyy}?", titulo);

            if (!confirmar)
            {
                Utils.ImprimirAviso(new[] { "Operação Cancelada!" }, titulo);
                return;
            }

            Utils.TelaLoading("Excluindo ocorrência", titulo);
            var result = await _service.DeleteOccurrenceAsync(selectedOccurrence.Id);

            if (result)
            {
                Utils.ImprimirAviso(new[] { "Ocorrência excluída com sucesso!" }, titulo, false);
            }
            else
            {
                Utils.ImprimirAviso(new[] { "Erro ao excluir a ocorrência. Tente novamente." }, titulo, true);
            }
        }

        private int SelectOccurrenceType(string titulo)
        {
            // Obter todos os valores do enum
            var tiposOcorrencia = System.Enum.GetValues(typeof(TipoOcorrencia))
                .Cast<TipoOcorrencia>()
                .Select((tipo, index) => $"[{(int)tipo}] {ObterDescricaoTipo(index)}")
                .ToArray();

            int escolha = Menus.CriarMenuNavegavel(titulo, tiposOcorrencia, "Selecione o tipo de ocorrência:");

            return escolha;
        }

        private string ObterDescricaoTipo(int tipo)
        {
            var tipoEnum = (TipoOcorrencia)tipo;

            return tipoEnum.ToString();
        }

    }
}
