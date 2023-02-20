using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;

namespace Quinto_projeto
{
    internal class Program
    {
        static string delimitadorInicio;
        static string delimitadorFim;
        static string tagNome;
        static string tagDataNascimento;
        static string tagNomeRua;
        static string tagNumeroCasa;
        public struct DadosCadastraisStruct
        {
            public string Nome;
            public DateTime DataDeNascimento;
            public string NomeDaRua;
            public UInt32 NumeroDaCasa;
        }
        public enum Resultado_e
        {
            Sucesso = 0,
            Sair = 1,
            Excecao = 2
        }
        public static void MostraMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
            Console.WriteLine("Pressione qualquer tecla para continuar");
            Console.ReadKey();
            Console.Clear();
        }
        public static Resultado_e PegaString(ref string minhaString, string mensagem)
        {
            Resultado_e retorno;
            Console.WriteLine(mensagem);
            string temp = Console.ReadLine();
            if (temp == "s" || temp == "S")
                retorno = Resultado_e.Sair;
            else
            {
                minhaString = temp;
                retorno = Resultado_e.Sucesso;
            }
            Console.Clear();
            return retorno;
        }
        public static Resultado_e PegaData(ref DateTime data, string mensagem)
        {
            Resultado_e retorno;
            do
            {

                try
                {
                    Console.WriteLine(mensagem);
                    string temp = Console.ReadLine();
                    if (temp == "s" || temp == "S")
                        retorno = Resultado_e.Sair;
                    else
                    {
                        data = Convert.ToDateTime(temp);
                        retorno = Resultado_e.Sucesso;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCECAO: " + e.Message);
                    Console.WriteLine("Pressione qualquer tecla para continuar");
                    Console.ReadKey();
                    Console.Clear();
                    retorno = Resultado_e.Excecao;
                }

            } while (retorno == Resultado_e.Excecao);
            Console.Clear();
            return retorno;
        }
        public static Resultado_e PegaUInt32(ref UInt32 numeroUInt32, string mensagem)
        {
            Resultado_e retorno;
            do
            {

                try
                {
                    Console.WriteLine(mensagem);
                    string temp = Console.ReadLine();
                    if (temp == "s" || temp == "S")
                        retorno = Resultado_e.Sair;
                    else
                    {
                        numeroUInt32 = Convert.ToUInt32(temp);
                        retorno = Resultado_e.Sucesso;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCECAO: " + e.Message);
                    Console.WriteLine("Pressione qualquer tecla para continuar");
                    Console.ReadKey();
                    Console.Clear();
                    retorno = Resultado_e.Excecao;
                }

            } while (retorno == Resultado_e.Excecao);
            Console.Clear();
            return retorno;
        }

        public static Resultado_e CadastraUsuario(ref List<DadosCadastraisStruct> ListaDeUsuarios)
        {
            DadosCadastraisStruct cadastroUsuario;
            cadastroUsuario.Nome = "";
            cadastroUsuario.DataDeNascimento = new DateTime();
            cadastroUsuario.NomeDaRua = "";
            cadastroUsuario.NumeroDaCasa = 0;
            if (PegaString(ref cadastroUsuario.Nome, "Digite o nome completo ou digite S para sair") == Resultado_e.Sair)
                return Resultado_e.Sair;
            if (PegaData(ref cadastroUsuario.DataDeNascimento, "Digite a data de nascimento no formato DD/MM/AAAA ou digite S para sair") == Resultado_e.Sair)
                return Resultado_e.Sair;
            if (PegaString(ref cadastroUsuario.NomeDaRua, "Digite o nome da rua ou digite S para sair") == Resultado_e.Sair)
                return Resultado_e.Sair;
            if (PegaUInt32(ref cadastroUsuario.NumeroDaCasa, "Digite o número da casa ou digite S para sair") == Resultado_e.Sair)
                return Resultado_e.Sair;
            ListaDeUsuarios.Add(cadastroUsuario);
            return Resultado_e.Sucesso;
        }

        public static void GravaDados(string caminho, List<DadosCadastraisStruct> ListaDeUsuarios)
        {
            try
            {
                string conteudoArquivo = "";
                foreach (DadosCadastraisStruct cadastro in ListaDeUsuarios)
                {
                    conteudoArquivo += delimitadorInicio + "\r\n";
                    conteudoArquivo += tagNome + cadastro.Nome + "\r\n";
                    conteudoArquivo += tagDataNascimento + cadastro.DataDeNascimento.ToString("dd/MM/yyyy") + "\r\n";
                    conteudoArquivo += tagNomeRua + cadastro.NomeDaRua + "\r\n";
                    conteudoArquivo += tagNumeroCasa + cadastro.NumeroDaCasa + "\r\n";
                    conteudoArquivo += delimitadorFim + "\r\n";
                }
                File.WriteAllText(caminho, conteudoArquivo);
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEÇÃO: " + e.Message);
            }
        }

        public static void CarregaDados(string caminho, ref List<DadosCadastraisStruct> ListaDeUsuarios)
        {
            try
            {
                if (File.Exists(caminho))
                {
                    string[] conteudoArquivo = File.ReadAllLines(caminho);
                    DadosCadastraisStruct dadosCadastrais;
                    dadosCadastrais.Nome = "";
                    dadosCadastrais.DataDeNascimento = new DateTime();
                    dadosCadastrais.NomeDaRua = "";
                    dadosCadastrais.NumeroDaCasa = 0;

                    foreach (string linha in conteudoArquivo)
                    {
                        if (linha.Contains(delimitadorInicio))
                            continue;
                        if (linha.Contains(delimitadorFim))
                            ListaDeUsuarios.Add(dadosCadastrais);
                        if (linha.Contains(tagNome))
                            dadosCadastrais.Nome = linha.Replace(tagNome, "");
                        if (linha.Contains(tagDataNascimento))
                            dadosCadastrais.DataDeNascimento = Convert.ToDateTime(linha.Replace(tagDataNascimento, ""));
                        if (linha.Contains(tagNomeRua))
                            dadosCadastrais.NomeDaRua = linha.Replace(tagNomeRua, "");
                        if (linha.Contains(tagNumeroCasa))
                            dadosCadastrais.NumeroDaCasa = Convert.ToUInt32(linha.Replace(tagNumeroCasa, ""));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEÇÃO: " + e.Message);
            }
        }

        static void Main(string[] args)
        {
            List<DadosCadastraisStruct> ListaDeUsuarios = new List<DadosCadastraisStruct>();
            string opcao = "";
            delimitadorInicio = "##### INICIO #####";
            delimitadorFim = "##### FIM #####";
            tagNome = "NOME: ";
            tagDataNascimento = "DATA_DE_NASCIMENTO: ";
            tagNomeRua = "NOME_DA_RUA: ";
            tagNumeroCasa = "NUMERO_DA_CASA: ";
            string caminhoArquivo = @"baseDeDados.txt";

            CarregaDados(caminhoArquivo, ref ListaDeUsuarios);
            do
            {
                Console.WriteLine("Digite C para cadastrar um novo usuário ou S para sair:");
                opcao = Console.ReadKey(true).KeyChar.ToString().ToLower();
                if (opcao == "c")
                {
                    //Cadastrar um novo usuário
                    if (CadastraUsuario(ref ListaDeUsuarios) == Resultado_e.Sucesso)
                        GravaDados(caminhoArquivo, ListaDeUsuarios);
                }
                else if (opcao == "s")
                {
                    //Sair da aplicação
                    MostraMensagem("Encerrando o programa");
                }
                else
                {
                    //Opção desconhecida
                    MostraMensagem("Opção desconhecida");
                }
            } while (opcao != "s");
        }
    }
}
