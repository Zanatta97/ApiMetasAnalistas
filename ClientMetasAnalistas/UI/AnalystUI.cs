using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Services;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Reflection;
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

            Utils.TelaLoading("Buscando dados", "Resultado Período");
            var response = await _service.GetAllAnalystTargetDataAsync(startDate, endDate);

            if (response == null || !response.Any())
            {
                Utils.ImprimirAviso(["Nenhum dado encontrado para o período informado."], "Resultados Completos", true);
                Utils.Break();
                return;
            }

            Utils.ImprimirTitulo("Resultados Período");
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"RESULTADOS - {startDate:dd/MM/yyyy} até {endDate:dd/MM/yyyy}", 90, ' ', 1);
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"{"NOME ANALISTA",-30} | {"DIAS",-10} | {"META",-10} | {"FECHADOS",-10} | {"ATINGIMENTO",-15}", 90, ' ', 0);
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

        public async Task GetAnalystTargetData()
        {
            AnalystDTO selectedAnalyst = await GetAnalystMenu();
            var startDate = Utils.ValidaData("Resultados Período", "Informe a data inicial (dd/MM/yyyy): ");
            var endDate = Utils.ValidaData("Resultados Período", "Informe a data final (dd/MM/yyyy): ");

            Utils.TelaLoading("Buscando analistas", "Resultados Período");
            var response = await _service.GetAnalystTargetDataAsync(selectedAnalyst, startDate, endDate);

            if (response == null)
            {
                Utils.ImprimirAviso(["Nenhum dado encontrado para o período informado."], "Resultados Analista", true);
                Utils.Break();
                return;
            }

            Utils.ImprimirTitulo("Resultados Período Analista");
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"{response.NomeAnalista.ToUpper()}", 90, ' ', 1);
            Utils.Imprimir($"{startDate:dd/MM/yyyy} até {endDate:dd/MM/yyyy}", 90, ' ', 1);
            Utils.Imprimir("==", 90, '=', 1);
            Utils.Imprimir($"{"DIAS TRABALHADOS",30} | {response.TotalDiasUteis,-20}", 90, ' ', 0);
            Utils.Imprimir($"{"META DIÁRIA",30} | {response.MetaDiaria,-20}", 90, ' ', 0);
            Utils.Imprimir($"{"META TOTAL",30} | {response.TotalMetaPeriodo,-20}", 90, ' ', 0);
            Utils.Imprimir($"{"CHAMADOS ENCERRADOS",30} | {response.TicketsFechados,-20}", 90, ' ', 0);

            if (response.PercentualMetaAlcancada >= 90m)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (response.PercentualMetaAlcancada >= 70m && response.PercentualMetaAlcancada < 90m)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Utils.Imprimir($"{"ATINGIMENTO",30} | {response.PercentualMetaAlcancada,-20}", 90, ' ', 0);
            Console.ResetColor();
            Utils.Imprimir("-", 90, '-', 0);
            Utils.Break();

        }

        public async Task<AnalystDTO> InsertAnalyst()
        {
            var analystDTO = new AnalystDTO();
            
            analystDTO.Nome = Utils.ValidaString("Informe o nome do Analista: ", "Cadastro de Analista", "Nome do Analista");

            analystDTO.Usuario = await SelectUsername();
            analystDTO.RegiaoId = await SelectRegion();
            analystDTO.MetaDiaria = Utils.ValidaInt("Informe a Meta Diária do Analista: ", "Cadastro de Analista");

            Utils.TelaLoading("Interindo analista", "Cadastro de Analista");
            var result = await _service.InsertAnalystAsync(analystDTO);
            if (result != null)
            {
                Utils.ImprimirAviso(new string[] { $"Analista '{result.Nome}' cadastrado com sucesso!" }, "Cadastro de Analista", false);
                return result;
            }
            else
            {
                Utils.ImprimirAviso(new string[] { "Erro ao cadastrar o analista. Tente novamente." }, "Cadastro de Analista", true);
                return null;

            }
        }

        public async Task<bool> UsernameExists(string username)
        {
            Utils.TelaLoading("Validando Nome de Usuário", "Cadastro de Analista");
            return await _service.UsernameExistsAsync(username);
        }

        public async Task<string> SelectUsername()
        {
            var usernameValido = false;

            string username;

            do
            {
                username = Utils.ValidaString("Informe o usuário do Analista: ", "Cadastro de Analista", "Usuário do Analista");

                if (await UsernameExists(username))
                {
                    Utils.ImprimirAviso([$"O usuário '{username}' já existe. Por favor, escolha outro."], "Cadastro de Analista", true);
                }
                else
                {
                    usernameValido = true;
                }

            } while (!usernameValido);

            return username;
        }

        public async Task<int> SelectRegion()
        {
            var regionUI = new RegionUI();

            int regionId = await regionUI.SelectRegion();

            return regionId;
        }

        public async Task InsertUpdateAnalyst()
        {
            string titulo = "Cadastro Analistas";
            int escolhaCadastro = Menus.MenuCadastro("Analista");

            if (escolhaCadastro == 3) //Voltar ao menu principal
            {
                Console.Clear();
                Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
            }
            else if (escolhaCadastro == 1) //Cadastrar novo
            {
                AnalystDTO analyst = await InsertAnalyst();

                Utils.ImprimirAviso(new string[] { $"Analista {analyst.Nome} cadastrado com sucesso!" }, titulo);
            }
            else //Alterar
            {
                bool cancelou = false;
                while (!cancelou)
                {
                    Utils.TelaLoading("Buscando analistas", titulo);
                    IEnumerable<AnalystDTO> analysts = await _service.GetAllAnalysts();

                    List<string> analystsString = new List<string>();

                    if (!analysts.Any())
                    {
                        Utils.ImprimirAviso(new string[] { "Nenhum analista encontrado!" }, titulo);
                        return;
                    }

                    foreach (var analyst in analysts)
                    {
                        analystsString.Add($"[{analyst.Id}] - Login: {analyst.Usuario} - Nome: {analyst.Nome}");
                    }

                    analystsString.Add("Voltar ao Menu Principal");

                    AnalystDTO? selectedAnalyst = null;

                    bool escolher;

                    escolher = Menus.MenuSimNao("Deseja buscar o Analista pelo Nome de Usuário?", titulo);

                    if (escolher)
                    {
                        string login = Utils.ValidaString("Por Favor, informe o login do analista a ser alterado:", titulo, "Usuário");
                        selectedAnalyst = analysts.FirstOrDefault(a => a.Usuario.Equals(login, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        int escolha = Menus.CriarMenuNavegavel(titulo, analystsString.ToArray(), "Selecione o analista que deseja alterar:");

                        if (escolha + 1 == analystsString.Count)
                        {
                            Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                            return;
                        }

                        selectedAnalyst = analysts.ElementAt(escolha);
                    }

                    if (selectedAnalyst != null)
                    {
                        await UpdateAnalyst(selectedAnalyst);
                        cancelou = !Menus.MenuSimNao("Deseja alterar outro analista?", titulo);
                    }
                    else
                    {
                        Utils.ImprimirAviso(new string[] { "Login não encontrado! Deseja tentar novamente?" }, titulo, true);
                        bool opcao = Menus.MenuSimNao("Login não encontrado! Deseja tentar novamente?", titulo);
                        if (opcao)
                        {
                            continue;
                        }
                        else
                        {
                            Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                            cancelou = true;
                        }
                    }
                }
            }
        }

        public async Task UpdateAnalyst(AnalystDTO analyst)
        {
            string titulo = "Cadastro Analista";
            bool finalizou = false;
            bool alterou = false;
            while (!finalizou)
            {
                PropertyInfo[] propriedades = analyst.GetType().GetProperties();

                int escolhaAlterar;
                Console.Clear();

                do
                {
                    Utils.ImprimirAviso(new string[] { $"Alterando Analista {analyst.Nome}" }, titulo);

                    List<string> listaNomesPropriedades = new List<string>();
                    for (int i = 0; i < propriedades.Length; i++)
                    {
                        var prop = propriedades[i];
                        var valorAtual = prop.GetValue(analyst);
                        if (prop.Name.Equals("Id"))
                        {
                            listaNomesPropriedades.Add($"[{i}] {prop.Name + "*Definido Automaticamente*",-35} | {valorAtual,-25}");
                        }
                        else if (prop.Name.Equals("Senha"))
                        {
                            listaNomesPropriedades.Add($"[{i}] {prop.Name,-35} | {new string('*', valorAtual.ToString().Length),-25}");
                        }
                        else
                        {
                            listaNomesPropriedades.Add($"[{i}] {prop.Name,-35} | {valorAtual,-25}");
                        }
                    }
                    listaNomesPropriedades.Add($"[0] Para cancelar ");


                    escolhaAlterar = Menus.CriarMenuNavegavel(titulo, listaNomesPropriedades.ToArray(), "Escolha a opção desejada:");

                    if (escolhaAlterar == propriedades.Length)
                    {
                        if (alterou)
                        {
                            Utils.ImprimirAviso(new string[] { "Alterações salvas com sucesso!" }, titulo);
                        }
                        else
                        {
                            Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                        }
                        finalizou = true;
                    }

                } while (escolhaAlterar == -1);

                if (finalizou)
                {
                    continue;
                }

                PropertyInfo propSelecionada = propriedades[escolhaAlterar];

                string novoValor;

                if (propSelecionada.Name.Equals("Usuario"))
                {
                    novoValor = await SelectUsername();
                }
                else if (propSelecionada.Name.Equals("Id"))
                {
                    Utils.ImprimirAviso(new string[] { "Id não pode ser alterado!" }, titulo, true);
                    continue;
                }
                else
                {
                    Console.Write($"Digite o novo valor para '{propSelecionada.Name}': ");
                    novoValor = Utils.ValidaString($"Digite o novo valor para '{propSelecionada.Name}': ", titulo, propSelecionada.Name);
                }

                try
                {
                    object novoValorConvertido = Convert.ChangeType(novoValor, propSelecionada.PropertyType);
                    propSelecionada.SetValue(analyst, novoValorConvertido);

                    if (propSelecionada.Name.Equals("Senha"))
                        Utils.ImprimirAviso(new string[] { $"Senha alterada com sucesso!" }, titulo);
                    else
                        Utils.ImprimirAviso(new string[] { $"Valor de '{propSelecionada.Name}' alterado para {novoValor}" }, titulo);
                    alterou = true;
                    finalizou = !Menus.MenuSimNao("Deseja alterar mais alguma propriedade deste analista?", titulo);
                }
                catch
                {
                    Utils.ImprimirAviso(new string[] { $"O valor digitado não é válido para o tipo {propSelecionada.PropertyType.Name}." }, titulo, true);
                    continue;
                }
            }

            AnalystDTO? updatedAnalyst = null;

            if (alterou)
            {
                try
                {
                    Utils.TelaLoading("Atualizando analista", titulo);
                    updatedAnalyst = await _service.UpdateAnalyst(analyst);

                    if (updatedAnalyst != null)
                    {
                        Utils.ImprimirAviso(new string[] { $"Analista '{updatedAnalyst.Nome}' atualizado com sucesso!" }, titulo, false);
                    }
                    else
                    {
                        Utils.ImprimirAviso(new string[] { "Erro ao atualizar o analista. Tente novamente." }, titulo, true);
                    }
                }
                catch (Exception e)
                {
                    Utils.ImprimirAviso(new string[] { "Erro ao atualizar o analista. Tente novamente." }, titulo, true);
                    Console.WriteLine($"Detalhes do erro: {e.Message}");
                }
            }
        }

        public async Task<AnalystDTO?> GetAnalystMenu()
        {
            string titulo = "Seleção de Analista";
            Utils.TelaLoading("Buscando analistas", titulo);
            IEnumerable<AnalystDTO> analysts = await _service.GetAllAnalysts();

            List<string> analystsString = new List<string>();

            if (!analysts.Any())
            {
                Utils.ImprimirAviso(new string[] { "Nenhum analista encontrado!" }, titulo);
                return null;
            }

            foreach (var analyst in analysts)
            {
                analystsString.Add($"[{analyst.Id}] - Login: {analyst.Usuario} - Nome: {analyst.Nome}");
            }

            analystsString.Add("Voltar ao Menu Principal");

            AnalystDTO selectedAnalyst = null;

            bool escolher;

            escolher = Menus.MenuSimNao("Deseja buscar o Analista pelo Nome de Usuário?", titulo);

            if (escolher)
            {
                string login = Utils.ValidaString("Por Favor, informe o login do analista a ser alterado:", titulo, "Usuário");
                selectedAnalyst = analysts.FirstOrDefault(a => a.Usuario.Equals(login, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                int escolha = Menus.CriarMenuNavegavel(titulo, analystsString.ToArray(), "Selecione o analista que deseja alterar:");

                if (escolha + 1 == analystsString.Count)
                {
                    Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                    return null;
                }

                selectedAnalyst = analysts.ElementAt(escolha);
            }

            return selectedAnalyst;
        }

        public async Task<AnalystDTO?> UpdateAnalystTarget()
        {
            string titulo = "Atualizar Meta Analista";
            bool cancelou = false;
            AnalystDTO selectedAnalyst = null;
            while (!cancelou)
            {
                Utils.TelaLoading("Buscando analistas", titulo);
                IEnumerable<AnalystDTO> analysts = await _service.GetAllAnalysts();

                List<string> analystsString = new List<string>();

                if (!analysts.Any())
                {
                    Utils.ImprimirAviso(new string[] { "Nenhum analista encontrado!" }, titulo);
                    return null;
                }

                foreach (var analyst in analysts)
                {
                    analystsString.Add($"[{analyst.Id}] - Login: {analyst.Usuario} - Nome: {analyst.Nome}");
                }

                analystsString.Add("Voltar ao Menu Principal");

                bool escolher;

                escolher = Menus.MenuSimNao("Deseja buscar o Analista pelo Nome de Usuário?", titulo);

                if (escolher)
                {
                    string login = Utils.ValidaString("Por Favor, informe o login do analista a ser alterado:", titulo, "Usuário");
                    selectedAnalyst = analysts.FirstOrDefault(a => a.Usuario.Equals(login, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    int escolha = Menus.CriarMenuNavegavel(titulo, analystsString.ToArray(), "Selecione o analista que deseja alterar:");

                    if (escolha + 1 == analystsString.Count)
                    {
                        Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                        return null;
                    }

                    selectedAnalyst = analysts.ElementAt(escolha);
                }

                if (selectedAnalyst != null)
                {
                    selectedAnalyst.MetaDiaria = Utils.ValidaInt($"Informe a nova Meta Diária para o analista {selectedAnalyst.Nome}:", titulo);
                    await UpdateAnalyst(selectedAnalyst);
                    cancelou = !Menus.MenuSimNao("Deseja alterar outro analista?", titulo);
                }
                else
                {
                    Utils.ImprimirAviso(new string[] { "Login não encontrado! Deseja tentar novamente?" }, titulo, true);
                    bool opcao = Menus.MenuSimNao("Login não encontrado! Deseja tentar novamente?", titulo);
                    if (opcao)
                    {
                        continue;
                    }
                    else
                    {
                        Utils.ImprimirAviso(new string[] { "Operação Cancelada!" }, titulo);
                        cancelou = true;
                        return null;
                    }
                }
            }

            return selectedAnalyst;
        }
    }
}
