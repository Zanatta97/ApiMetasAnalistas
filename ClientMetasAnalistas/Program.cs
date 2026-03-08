using ClientMetasAnalistas.UI;
using ClientMetasAnalistas.Utilidades;

namespace ClientMetasAnalistas
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Utils.ImprimirTitulo("Controle de Atingimento");
            Utils.ImprimirAviso(new string[] { "Bem-vindo ao sistema de controle de atingimento de metas para analistas!" }, "Controle de Atingimento", false);

            int escolha = -1;

            var analystUI = new AnalystUI();

            do
            {
                escolha = Menus.CriarMenuNavegavel("Menu Principal", Menus.OpcoesMenuPrincipal);

                switch (escolha)
                {
                    case 0:
                        await analystUI.GetAllAnalystTargetData();
                        break;
                    case 1:
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, "Atingimento Geral", true);
                        break;
                    case 2:
                        Utils.ImprimirAviso(new string[] { "Sistema Finalizado!" }, "Controle de Atingimento", false);
                        break;
                }

            } while (escolha != 2);

            Utils.Break("Obrigado por usar o sistema de controle de atingimento de metas para analistas!");
        }

        public static async Task Run()
        {
            Utils.ImprimirTitulo("Controle de Atingimento");
            Utils.ImprimirAviso(new string[] { "Bem-vindo ao sistema de controle de atingimento de metas para analistas!" }, "Controle de Atingimento", false);

            int escolha = -1;

            var analystUI = new AnalystUI();

            do
            {
                escolha = Menus.CriarMenuNavegavel("Menu Principal", Menus.OpcoesMenuPrincipal);

                switch (escolha)
                {
                    case 0:
                        await analystUI.GetAllAnalystTargetData();
                        break;
                    case 1:
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, "Atingimento Geral", true);
                        break;
                    case 2:
                        Utils.ImprimirAviso(new string[] { "Sistema Finalizado!" }, "Controle de Atingimento", false);
                        break;
                }

            } while (escolha != 2);

            Utils.Break("Obrigado por usar o sistema de controle de atingimento de metas para analistas!");
        }
    }
}
