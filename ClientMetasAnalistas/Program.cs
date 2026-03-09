using ClientMetasAnalistas.UI;
using ClientMetasAnalistas.Utilidades;

namespace ClientMetasAnalistas
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string tituloPrinipal = "Controle de Atingimento";
            Utils.ImprimirTitulo(tituloPrinipal);
            Utils.ImprimirAviso(new string[] { "Bem-vindo ao sistema de controle de atingimento de metas para analistas!" }, "Controle de Atingimento", false);

            int escolha = -1;

            var analystUI = new AnalystUI();

            do
            {
                escolha = Menus.CriarMenuNavegavel("Menu Principal", Menus.OpcoesMenuPrincipal);

                /*
                    "[1] Visualizar Atingimento Geral", - OK
                    "[2] Visualizar Atingimento Analista", - OK
                    "[3] Cadastrar/Alterar Analista", - OK
                    "[4] Alterar Meta Analista", - Testar
                    "[5] Listar Ocorrencias Analista no Período",
                    "[6] Registrar Ocorrencia Analista",
                    "[7] Listar Feriados",
                    "[8] Registrar Feriado",
                    "[0] Finalizar o programa"
                 */

                switch (escolha)
                {
                    case 0: //[1] Visualizar Atingimento Geral
                        await analystUI.GetAllAnalystTargetData();
                        break;
                    case 1: //[2] Visualizar Atingimento Analista
                        await analystUI.GetAnalystTargetData();
                        break;
                    case 2: //[3] Cadastrar/Alterar Analista
                        await analystUI.InsertUpdateAnalyst();
                        break;
                    case 3: //[4] Alterar Meta Analista
                        await analystUI.UpdateAnalystTarget();
                        break;
                    case 4: //[5] Listar Ocorrencias Analista no Período
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, tituloPrinipal, true);
                        break;
                    case 5: //[6] Registrar Ocorrencia Analista
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, tituloPrinipal, true);
                        break;
                    case 6: //[7] Listar Feriados
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, tituloPrinipal, true);
                        break;
                    case 7: //[8] Registrar Feriado
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, tituloPrinipal, true);
                        break;
                    case 8: //[0] Finalizar o programa
                        Utils.ImprimirAviso(new string[] { "Sistema Finalizado!" }, tituloPrinipal, false);
                        break;
                }

            } while (escolha != 8);

            Utils.Break("Obrigado por usar o sistema de controle de atingimento de metas para analistas!");
        }
    }
}
