using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Services;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.UI
{
    public class RegionUI
    {
        private readonly IRegionService _service = new RegionService();

        public async Task<int> SelectRegion()
        {
            Utils.TelaLoading("Buscando regiões", "Cadastro");
            var regions = await _service.GetAllRegionsAsync();
            int selectedRegionId = 0;
            bool retry = true;

            while (retry)
            {
                if (regions == null || !regions.Any())
                {
                    Console.WriteLine("Nenhuma região encontrada.");
                    var escolha = Menus.MenuSimNao("Deseja tentar novamente?", "Selecionar Região");

                    if (escolha)
                    {
                        Utils.TelaLoading("Buscando Regiões", "Cadastro");
                        regions = await _service.GetAllRegionsAsync();
                    }
                    else
                        retry = false;
                }
                else
                    retry = false;
            }

            if (regions != null && regions.Any())
            {
                bool escolher;

                escolher = Menus.MenuSimNao("Deseja buscar a região pelo ID?", "Selecionar Região");

                if (escolher)
                {
                    selectedRegionId = Utils.ValidaInt("Por Favor, informe o ID da Região desejada:", "Selecionar Região");
                }
                else
                {
                    selectedRegionId = Menus.CriarMenuNavegavel("Selecionar Região", FormatListRegionsArray(regions));
                    selectedRegionId = regions.ElementAt(selectedRegionId).Id;
                }

                if (!regions.Any(a => a.Id.Equals(selectedRegionId)) || selectedRegionId == 0)
                {
                    Utils.ImprimirAviso(new string[] { "Região não encontrada.", "Selecione a Região desejada na lista a seguir:" }, "Selecionar Região", true);
                    selectedRegionId = Menus.CriarMenuNavegavel("Selecionar Região", FormatListRegionsArray(regions));
                    selectedRegionId = regions.ElementAt(selectedRegionId).Id;
                }
            }

            return selectedRegionId;
        }

        public static string[] FormatListRegionsArray(IEnumerable<RegionDTO> regions)
        {
            List<string> lista = new List<string>();

            foreach (var region in regions)
            {
                lista.Add(Utils.RetornaStringFormatada($"{"[" + region.Id + "]",-5} | {region.Nome,-25}", 50, ' ', 0, ' '));
            }

            return lista.ToArray();
        }
    }
}
