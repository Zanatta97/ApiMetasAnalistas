using ClientMetasAnalistas.Utilidades;

namespace ClientMetasAnalistas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Utils.ImprimirTitulo("Controle de Atingimento");
            Utils.ImprimirAviso(new string[] { "Bem-vindo ao sistema de controle de atingimento de metas para analistas!" }, "Controle de Atingimento", false);

            int escolha = -1;

            

            do
            {
                escolha = Menus.CriarMenuNavegavel("Menu Principal", Menus.OpcoesMenuPrincipal);

                switch (escolha)
                {
                    case 0:
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, "Atingimento Geral", true);
                        break;
                    case 1:
                        Utils.ImprimirAviso(new string[] { "Opção em desenvolvimento" }, "Atingimento Geral", true);
                        break;
                    case 2:
                        Utils.ImprimirAviso(new string[] { "Sistema Finalizado!" }, "Controle de Atingimento", false);
                        break;
                }

            } while (escolha != 2);

        }
    }
}
