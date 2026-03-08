using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.Utilidades
{
    public static class Utils
    {
        public const string API_URL = "http://localhost:5037/";
        public static void ImprimirAviso(string[] texto, string titulo, bool erro = false)
        {
            int tamanho = texto.Max(s => s.Length) + 6;
            int esquerda = (Console.WindowWidth - tamanho) / 2;

            ImprimirTitulo(titulo);

            if (erro)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(CentralizarTexto($"*{new string('*', tamanho)}*"));

            foreach (string linha in texto)
            {
                esquerda = (tamanho - linha.Length) / 2;
                int direita = esquerda + (tamanho - linha.Length - esquerda * 2);
                Console.WriteLine(CentralizarTexto($"* {new string(' ', esquerda - 1)}{$"{linha.Replace("\r", "").Replace("\n", "").ToUpper().Trim()}"}{new string(' ', direita - 1)} *"), tamanho);
            }

            Console.WriteLine(CentralizarTexto($"*{new string('*', tamanho)}*"));
            Console.ResetColor();
            Break();
        }

        public static void ImprimirTitulo(string texto)
        {
            Console.Clear();
            Console.Write("\x1b[3J"); // Limpa o buffer do console
            Console.SetCursorPosition(0, 0); // Move o cursor para o início, somente isto não limpa a tela totalmente
            Console.Clear();
            int metade = (Console.WindowWidth - 3 - texto.Length) / 2;
            int tamanho = metade * 2 + texto.Length;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"|{new string('=', tamanho)}|");
            Console.WriteLine($"|{new string(' ', metade)}{texto.ToUpper()}{new string(' ', metade)}|");
            Console.WriteLine($"|{new string('=', tamanho)}|" + "\r\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Imprime o texto no console do tamanho desejado e coloca | no início e fim.
        /// Posição do Texto: 0 => Esquerda 1 => Centro 2 => Direita
        /// </summary>
        /// <param name="texto"> Texto a ser impresso</param>
        /// <param name="larguraTotal">Tamanho que a impressão ficará em tela</param>
        /// <param name="caractere">Caractere que será preenchido nos espaços a mais</param>
        /// <param name="posicaoTexto">0 => Esquerda - 1 => Centro - 2 => Direita</param>
        public static void Imprimir(string texto, int larguraTotal, char caractere, int posicaoTexto = 0)
        {
            string linha;
            int espacoVazio;

            if (posicaoTexto == 0) //Esquerda
            {
                espacoVazio = larguraTotal - texto.Length - 3;
                if (espacoVazio < 0)
                    espacoVazio = 0;
                linha = $"| {texto}{new string(caractere, espacoVazio)}|";
            }
            else if (posicaoTexto == 1) //Centro
            {
                espacoVazio = larguraTotal - texto.Length - 2;
                if (espacoVazio < 0)
                    espacoVazio = 0;
                int traçosEsquerda = espacoVazio / 2;
                int traçosDireita = espacoVazio - traçosEsquerda;
                linha = $"|{new string(caractere, traçosEsquerda)}{texto}{new string(caractere, traçosDireita)}|";
            }
            else //Direita
            {
                espacoVazio = larguraTotal - texto.Length - 3;
                if (espacoVazio < 0)
                    espacoVazio = 0;
                linha = $"|{new string(caractere, espacoVazio)}{texto} |";
            }
            int metade = (Console.WindowWidth - linha.Length) / 2;

            Console.WriteLine($"{new string(' ', metade)}{linha}{new string(' ', metade)}");

            //Console.WriteLine(linha);
        }


        public static string RetornaStringFormatada(string texto, int larguraTotal, char caractere, int posicaoTexto = 0, char inicioFim = '|')
        {
            string linha;
            int espacoVazio;

            if (posicaoTexto == 0) //Esquerda
            {
                espacoVazio = larguraTotal - texto.Length - 3;
                if (espacoVazio < 0)
                    espacoVazio = 0;
                linha = $"{inicioFim} {texto}{new string(caractere, espacoVazio)}{inicioFim}";
            }
            else if (posicaoTexto == 1) //Centro
            {
                espacoVazio = larguraTotal - texto.Length - 2;
                if (espacoVazio < 0)
                    espacoVazio = 0;
                int traçosEsquerda = espacoVazio / 2;
                int traçosDireita = espacoVazio - traçosEsquerda;
                linha = $"{inicioFim}{new string(caractere, traçosEsquerda)}{texto}{new string(caractere, traçosDireita)}{inicioFim}";
            }
            else //Direita
            {
                espacoVazio = larguraTotal - texto.Length - 3;
                if (espacoVazio < 0)
                    espacoVazio = 0;
                linha = $"{inicioFim}{new string(caractere, espacoVazio)}{texto} {inicioFim}";
            }
            return linha;
        }

        //Código roubado da IA para um teste - Mas como eu gostei e não afeta nada ele ficou.
        public static string LerSenha(string titulo = "", string mensagem = "")
        {
            if (titulo.Length > 0)
                ImprimirTitulo(titulo);

            string senha = "";

            if (mensagem.Length > 0)
                Console.WriteLine(mensagem);

            Console.Write("Digite a senha: ");

            while (true)
            {
                // O 'true' aqui é o segredo: ele impede a tecla de aparecer na tela
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                // 1. Se apertar ENTER, encerra a leitura
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine(); // Pula para a próxima linha visualmente
                    break;
                }
                // 2. Tratamento do BACKSPACE (para apagar caracteres)
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (senha.Length > 0)
                    {
                        // Remove o último caractere da string da senha
                        senha = senha.Substring(0, senha.Length - 1);

                        // Remove o asterisco da tela: volta um, imprime espaço, volta um
                        Console.Write("\b \b");
                    }
                }
                // 3. Adiciona caractere à senha (ignorando teclas de controle como F1, ESC, etc)
                else if (!char.IsControl(tecla.KeyChar))
                {
                    senha += tecla.KeyChar;
                    Console.Write("*"); // Imprime o asterisco no lugar da letra
                }
            }

            if (senha.Length == 0)
            {
                if (titulo.Length > 0)
                    ImprimirAviso(new string[] { "SENHA não pode ser vazia! Por favor digite uma senha!" }, titulo, true);
                return LerSenha(titulo);
            }
            return senha;
        }

        public static void Break(string textoFinal = "")
        {
            if (textoFinal.Equals(""))
                textoFinal = "Pressione qualquer tecla para continuar!";

            Console.WriteLine($"{new string(' ', (Console.WindowWidth - textoFinal.Length) / 2)}{textoFinal}{new string(' ', (Console.WindowWidth - textoFinal.Length) / 2)}");
            Console.ReadKey(true);
        }

        //Código roubado de IA para testes.

        // Método auxiliar para centralizar texto puro (usado no menu acima)
        public static string CentralizarTexto(string texto, int largura = 0)
        {
            if (largura == 0)
                largura = Console.WindowWidth;

            if (texto.Length >= largura) return texto;
            int esquerda = (largura - texto.Length) / 2;
            // PadLeft cria o espaço a esquerda, PadRight preenche o resto
            return texto.PadLeft(esquerda + texto.Length).PadRight(largura);
        }

        public static decimal ValidaDecimal(string mensagem, string titulo)
        {
            decimal valor;
            Utils.ImprimirTitulo(titulo);

            Console.Write(mensagem);
            while (!decimal.TryParse(Console.ReadLine(), out valor))
            {
                Utils.ImprimirAviso(new[] { "Valor Selecionado Inválido!" }, titulo, true);
                Console.Write(mensagem);

            }
            return valor;
        }

        public static int ValidaInt(string mensagem, string titulo)
        {
            int valor;
            Utils.ImprimirTitulo(titulo);
            Console.Write(mensagem);
            while (!Int32.TryParse(Console.ReadLine(), out valor))
            {
                Utils.ImprimirAviso(new[] { "Valor Selecionado Inválido!" }, titulo, true);
                Console.Write(mensagem);
            }
            return valor;
        }

        public static string ValidaString(string mensagem, string titulo, string dado)
        {
            string valor;
            bool confirma = false;

            do
            {
                Utils.ImprimirTitulo(titulo);
                Console.Write(mensagem);
                valor = Console.ReadLine();

                if (valor.Equals(""))
                    Utils.ImprimirAviso(new[] { $"{dado} selecionado inválido!" }, titulo, true);
                else
                    confirma = Menus.MenuSimNao($"Você digitou: {valor}. Confirma?", titulo);
            } while (!confirma);

            return valor;
        }

        public static string ValidaDocumento(string mensagem, string titulo, string dado)
        {
            string valor;
            bool confirma = false;

            do
            {
                Utils.ImprimirTitulo(titulo);
                Console.Write(mensagem);
                valor = Console.ReadLine();

                if (!valor.Equals(""))
                {
                    if (dado.Equals("CPF"))
                    {
                        if (valor.Length == 11 && valor.All(char.IsDigit))
                        {
                            confirma = Menus.MenuSimNao($"Você digitou: {valor}. Confirma?", titulo);
                            continue;
                        }
                        else
                            Utils.ImprimirAviso(new[] { $"{dado} informado inválido!" }, titulo, true);
                    }
                    else if (dado.Equals("CNPJ"))
                    {
                        if (valor.Length == 14 && valor.All(char.IsDigit))
                            confirma = Menus.MenuSimNao($"Você digitou: {valor}. Confirma?", titulo);
                        else
                            Utils.ImprimirAviso(new[] { $"{dado} informado inválido!" }, titulo, true);
                    }
                }
                else
                {
                    Utils.ImprimirAviso(new[] { $"Por favor, informe um {dado} válido!" }, titulo, true);
                }

            } while (!confirma);

            return valor;
        }

        public static DateTime ValidaData(string titulo, string mensagem)
        {
            DateTime dataValida;
            bool dataOk = false;
            do
            {
                Utils.ImprimirTitulo(titulo);
                Console.Write(mensagem);
                string data = Console.ReadLine();
                if (DateTime.TryParse(data, out dataValida))
                {
                    dataOk = true;
                }
                else
                {
                    Utils.ImprimirAviso(new string[] { "Data inválida! Por favor, tente novamente." }, titulo, true);
                }
            } while (!dataOk);
            return dataValida;
        }
    }
}