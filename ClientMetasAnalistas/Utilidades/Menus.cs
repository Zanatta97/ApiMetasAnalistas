using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.Utilidades
{
    public static class Menus
    {
        private static readonly int TamanhoMenus = 60;
        private static readonly string TextoPadrao = $"Escolha a opção desejada:";

        public static string[] OpcoesMenuPrincipal =
        {
            "[1] Visualizar Atingimento Geral",
            "[2] Visualizar Atingimento Analista",
            "[3] Cadastrar/Alterar Analista",
            "[4] Alterar Meta Analista",
            "[5] Listar Ocorrencias Analista no Período",
            "[6] Registrar Ocorrencia Analista",
            "[7] Listar Feriados",
            "[8] Registrar Feriado",
            "[0] Finalizar o programa"
        };
        

        // MENU NAVEGÁVEL COM SETAS - Roubado da IA como um Teste mas como eu curti eu alterei e ficou.
        public static int CriarMenuNavegavel(string titulo, string[] opcoes, string texto = "", string textofinal = "")
        {
            ConsoleKey key;
            int opcaoSelecionada = 0;
            int larguraMenu = TamanhoMenus;
            if (opcoes.Max(s => s.Length) > larguraMenu)
                larguraMenu = opcoes.Max(s => s.Length); ;

            larguraMenu += 4;

            if (texto == "")
                texto = TextoPadrao;

            int centro = (Console.WindowWidth - larguraMenu) / 2;

            do
            {
                Console.Clear();
                Utils.ImprimirTitulo(titulo);
                Console.SetCursorPosition(centro, Console.CursorTop);
                Console.WriteLine(Utils.CentralizarTexto($"{texto}", larguraMenu));
                Console.SetCursorPosition(centro, Console.CursorTop);
                Console.WriteLine("+" + new string('-', larguraMenu - 2) + "+");

                //Retorna as opções no formado certinho
                for (int i = 0; i < opcoes.Length; i++)
                {
                    Console.SetCursorPosition(centro, Console.CursorTop);

                    if (i == opcaoSelecionada)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("| " + opcoes[i].PadRight(larguraMenu - 4) + " |");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("| " + opcoes[i].PadRight(larguraMenu - 4) + " |");
                    }
                }

                Console.SetCursorPosition(centro, Console.CursorTop);
                Console.WriteLine("+" + new string('-', larguraMenu - 2) + "+");

                if (textofinal != "")
                {
                    if (textofinal.Length < larguraMenu)
                        Console.WriteLine(Utils.CentralizarTexto(textofinal, larguraMenu));
                    else
                        Console.WriteLine(Utils.CentralizarTexto(textofinal, larguraMenu));
                }

                string instrucao = Utils.CentralizarTexto("*Use as setas para navegar e ENTER para selecionar*");
                Console.WriteLine(instrucao);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    opcaoSelecionada--;
                    if (opcaoSelecionada < 0) opcaoSelecionada = opcoes.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    opcaoSelecionada++;
                    if (opcaoSelecionada >= opcoes.Length) opcaoSelecionada = 0;
                }

            } while (key != ConsoleKey.Enter);

            return opcaoSelecionada;
        }

        public static int CriarMenuNavegavel(string titulo, string[] opcoes, string[] texto, string[] textofinal = null)
        {
            ConsoleKey key;
            int opcaoSelecionada = 0;
            int larguraMenu = TamanhoMenus;
            int larguraTexto = TamanhoMenus;
            int larguraFinal = TamanhoMenus;

            if (opcoes.Max(s => s.Length) > larguraMenu)
                larguraMenu = opcoes.Max(s => s.Length);

            if (texto.Max(s => s.Length) > larguraTexto)
                larguraTexto = texto.Max(s => s.Length);

            if (textofinal != null)
            {

                if (textofinal.Max(s => s.Length) > larguraFinal)
                    larguraFinal = textofinal.Max(s => s.Length);
            }

            larguraMenu += 4;
            larguraTexto += 4;
            larguraFinal += 4;

            if (texto == null)
                texto[0] = TextoPadrao;


            int centroMenu = (Console.WindowWidth - larguraMenu) / 2;
            int centroTexto = (Console.WindowWidth - larguraTexto) / 2;
            int centroFinal = (Console.WindowWidth - larguraFinal) / 2;

            do
            {
                Console.Clear();
                Utils.ImprimirTitulo(titulo);

                foreach (string linha in texto)
                {
                    Console.SetCursorPosition(centroTexto, Console.CursorTop);
                    Console.WriteLine("| " + linha.PadRight(larguraTexto - 4) + " |");
                }

                Console.SetCursorPosition(centroMenu, Console.CursorTop);
                Console.WriteLine("+" + new string('-', larguraMenu - 2) + "+");

                //Retorna as opções no formado certinho
                for (int i = 0; i < opcoes.Length; i++)
                {
                    Console.SetCursorPosition(centroMenu, Console.CursorTop);

                    if (i == opcaoSelecionada)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("| " + opcoes[i].PadRight(larguraMenu - 4) + " |");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("| " + opcoes[i].PadRight(larguraMenu - 4) + " |");
                    }
                }

                Console.SetCursorPosition(centroMenu, Console.CursorTop);
                Console.WriteLine("+" + new string('-', larguraMenu - 2) + "+");

                if (textofinal != null)
                {
                    foreach (string linha in textofinal)
                    {
                        Console.SetCursorPosition(centroFinal, Console.CursorTop);
                        Console.WriteLine("| " + linha.PadRight(larguraFinal - 4) + " |");
                    }
                }

                string instrucao = Utils.CentralizarTexto("*Use as setas para navegar e ENTER para selecionar*");
                Console.WriteLine(instrucao);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    opcaoSelecionada--;
                    if (opcaoSelecionada < 0) opcaoSelecionada = opcoes.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    opcaoSelecionada++;
                    if (opcaoSelecionada >= opcoes.Length) opcaoSelecionada = 0;
                }

            } while (key != ConsoleKey.Enter);

            return opcaoSelecionada;
        }

        public static bool MenuSimNao(string textoPergunta, string titulo)
        {
            int opcaoSelecionada;
            string[] opcoes =
            {
                "SIM",
                "NÃO"
            };

            opcaoSelecionada = CriarMenuNavegavel(titulo, opcoes, textoPergunta);

            if (opcaoSelecionada == 0)
                return true;
            else
                return false;
        }

        public static bool MenuSimNao(string[] textoPergunta, string titulo)
        {
            int opcaoSelecionada;
            string[] opcoes =
            {
                "SIM",
                "NÃO"
            };

            opcaoSelecionada = CriarMenuNavegavel(titulo, opcoes, textoPergunta);

            if (opcaoSelecionada == 0)
                return true;
            else
                return false;
        }

        public static int MenuCadastro(string opcao)
        {
            string[] escolhas =
            {
                $"[1] Inserir {opcao}",
                $"[2] Alterar {opcao} existente",
                $"[0] Voltar ao menu principal"
            };

            int escolha;
            do
            {
                escolha = CriarMenuNavegavel($"Menu Cadastro {opcao}", escolhas, TextoPadrao);

                if (escolha < 0 || escolha > escolhas.Length)
                {
                    Utils.ImprimirAviso(new string[] { "Opção selecionada inválida!" }, $"Menu Cadastro {opcao}", true);
                    escolha = -1;
                }

            } while (escolha == -1);

            if (escolha == escolhas.Length)
                return 0;
            else
                return escolha + 1;
        }
    }
}