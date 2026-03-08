using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Services;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace ClientMetasAnalistas.UI
{
    internal class AnalystUI
    {
        private readonly IAnalystService _service = new AnalystService();

        public async Task GetAllAnalystTargetData()
        {
            var startDate = Utils.ValidaData("Resultados Período", "Informe a data inicial (dd/MM/yyyy): ");
            var endDate = Utils.ValidaData("Resultados Período", "Informe a data final (dd/MM/yyyy): ");

            var response = await _service.GetAllAnalystTargetDataAsync(startDate, endDate);

            if (response == null || response.Count == 0)
            {
                Utils.ImprimirAviso(["Nenhum dado encontrado para o período informado."], "Resultados Completos", true);
                Utils.Break();
                return;
            }

            Utils.ImprimirTitulo("Resultados Período");
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"RESULTADOS - {startDate:dd/MM/yyyy} até {endDate:dd/MM/yyyy}", 90, ' ', 1);
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"{"NOME ANALISTA",-30} | {"DIAS",-10} | {"META",-10} | {"FECHADOS",-10} | {"ATINGIMENTO",-15}" , 90, ' ', 0);
            Utils.Imprimir("-", 90, '-', 0);

            foreach (var item in response)
            {
                if (item.PercentualMetaAlcancada >= 90m)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                } 
                else if (item.PercentualMetaAlcancada >= 70m && item.PercentualMetaAlcancada < 90m)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Utils.Imprimir($"{item.NomeAnalista,-30} | {item.TotalDiasUteis,-10} | {item.TotalMetaPeriodo,-10} | {item.TicketsFechados,-10} | {item.PercentualMetaAlcancada,-15:F2}", 90, ' ', 0);
                
                Console.ResetColor();
                Utils.Imprimir("-", 90, '-', 0);
            }

            Utils.Break();

        }
    }
}
