using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AMXAssociacao
{
    class Sistema
    {
        private static Associacao associacao = new Associacao("AMXassociacao");
        public static void Main(string[] args)
        {
            Config();
            MenuPrincipal();
        }
        private static void MenuPrincipal()
        {
            int escolha = -1;
            do
            {
                Cabecalho();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("--------------------------Menu Principal-------------------------");
                Console.ResetColor();
                Console.WriteLine(
                    "1 - Gerenciar Habilidades\r\n" +
                    "2 - Gerenciar Associados\r\n" +
                    "3 - Gerenciar Demandas\r\n" +
                    "4 - Relatórios\r\n" +
                    "0 - sair\r\n"
                );
                escolha = LerNumero("Escolha uma opção: ");
                Console.WriteLine();
                switch (escolha)
                {
                    case 1:
                        MenuHabilidades();
                        break;
                    case 2:
                        MenuAssociados();
                        break;
                    case 3:
                        MenuDemandas();
                        break;
                    case 4:
                        MenuRelatorios();
                        break;
                    case 0:
                        Console.WriteLine("Saindo...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida, por favor tente novamente.");
                        break;
                }
                if (escolha < 0 || escolha > 4) Pausa();
            } while (escolha != 0);
        }

        private static void Cabecalho()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Bem vindo a AMX Associação V0.1");
            Console.ResetColor();
        }
        private static void MenuHabilidades()
        {
            int escolha = -1;
            do
            {
                try
                {
                    Cabecalho();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("---------------------Gerenciando Habilidades—--------------------");
                    Console.ResetColor();
                    Console.WriteLine(
                        "1 - Listar Habilidades Reconhecidas\r\n" +
                        "2 - Cadastrar Habilidades\r\n" +
                        "3 - Excluir Habilidade\r\n" +
                        "0 - Voltar\r\n"
                    );
                    escolha = LerNumero("Escolha uma opção: ");
                    Console.WriteLine();
                    switch (escolha)
                    {
                        case 1:
                            ListarHabilidades();
                            break;
                        case 2:
                            CadastrarHabilidade();
                            break;
                        case 3:
                            ExcluirHabilidade();
                            break;
                        case 0: // Voltar
                            break;
                        default:
                            Console.WriteLine("Opção inválida, por favor tente novamente.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    ImprimirErro("\n" + e.Message);
                }
                if (escolha != 0) Pausa();
            } while (escolha != 0);
        }

        private static void MenuAssociados()
        {
            int escolha = -1;
            do
            {
                try
                {
                    Cabecalho();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("----------------------Gerenciando Associados—--------------------");
                    Console.ResetColor();
                    Console.WriteLine(
                        "1 - Listar Associados\r\n" +
                        "2 - Cadastrar Associado\r\n" +
                        "3 - Pesquisar Associado\r\n" +
                        "4 - Adicionar habilidade\r\n" +
                        "5 - Remover habilidade\r\n" +
                        "0 - voltar\r\n");
                    escolha = LerNumero("Escolha uma opção: ");
                    Console.WriteLine();
                    switch (escolha)
                    {
                        case 1:
                            ListarAssociados();
                            break;
                        case 2:
                            CadastrarAssociado();
                            break;
                        case 3:
                            Associado associado = PesquisarAssociado();
                            Console.WriteLine("\n" + associado.Relatorio());
                            break;
                        case 4:
                            AdicionarHabilidadeAssociado();
                            break;
                        case 5:
                            RemoverHabilidadeAssociado();
                            break;
                        case 0: // Voltar
                            break;
                        default:
                            Console.WriteLine("Opção inválida, por favor tente novamente.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    ImprimirErro(e.Message);
                }
                if (escolha != 0) Pausa();
            } while (escolha != 0);
        }
        private static void MenuDemandas()
        {
            int escolha = -1;
            do
            {
                try
                {
                    Cabecalho();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("---—-------------------Gerenciando Demandas—---------------------");
                    Console.ResetColor();
                    Console.WriteLine(
                        "1 - Listar todas as demandas\r\n" +
                        "2 - Cadastrar demanda\r\n" +
                        "3 - Pesquisar Demanda\r\n" +
                        "4 - Incluir requisito\r\n" +
                        "5 - Excluir requisito\r\n" +
                        "6 - Atribuir prestador\r\n" +
                        "7 - Encerrar demanda\r\n" +
                        "0 - Voltar\r\n"
                    );
                    escolha = LerNumero("Escolha uma opção: ");
                    Console.WriteLine();
                    switch (escolha)
                    {
                        case 1:
                            Console.WriteLine(associacao.ListarDemandas());
                            break;
                        case 2:
                            CadastrarDemanda();
                            break;
                        case 3:
                            Demanda demanda = PesquisarDemanda();
                            Console.WriteLine(demanda.Relatorio());
                            break;
                        case 4:
                            IncluirRequisito();
                            break;
                        case 5:
                            ExcluirRequisito();
                            break;
                        case 6:
                            AtribuirPrestador();
                            break;
                        case 7:
                            EncerrarDemanda();
                            break;
                        case 0: // Voltar
                            break;
                        default:
                            Console.WriteLine("Opção inválida, por favor tente novamente.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    ImprimirErro(e.Message);
                }
                if (escolha != 0) Pausa();
            } while (escolha != 0);
        }

        private static Habilidade LerHabilidade(int numero = -1)
        {
            string mensagem = "Descricao da habilidade" + (numero == -1 ? ": " : $" #{numero:D2}: ");
            Console.Write(mensagem);
            string descricaoHabilidade = Console.ReadLine();
            return new Habilidade(descricaoHabilidade);
        }

        private static void ExcluirHabilidade()
        {
            ListarHabilidades();
            associacao.ExcluirHabilidade(LerHabilidade());
            Console.WriteLine();
            ListarHabilidades();
        }

        private static void ListarHabilidades()
        {
            Console.Write("Habilidades Reconhecidas: ");
            Console.WriteLine(associacao.ListarHabilidades() + "\n");
        }

        private static void CadastrarHabilidade()
        {
            associacao.CadastrarHabilidade(LerHabilidade());
            Console.WriteLine();
            ListarHabilidades();
        }

        private static void RemoverHabilidadeAssociado()
        {
            Associado associado = PesquisarAssociado();
            Habilidade habilidade = LerHabilidade();
            if (!associacao.HabilidadeEhReconhecida(habilidade))
            {
                ImprimirErro($"A habilidade \"{habilidade}\" não é reconhecida.");
                return;
            }
            associado.RemoverHabilidade(habilidade);
            Console.WriteLine("\n" + associado.Relatorio());
        }

        private static Associado PesquisarAssociado()
        {
            Console.Write("ID do Associado: ");
            int idAssociado = int.Parse(Console.ReadLine());
            return associacao.PesquisarAssociado(idAssociado);
        }

        private static void AdicionarHabilidadeAssociado()
        {
            ListarHabilidades();
            Associado associado = PesquisarAssociado();
            Habilidade habilidade = LerHabilidade();
            if (!associacao.HabilidadeEhReconhecida(habilidade))
            {
                ImprimirErro($"A habilidade \"{habilidade}\" não é reconhecida.");
                return;
            }
            associado.AdicionarHabilidade(habilidade);
            Console.WriteLine("\n" + associado.Relatorio());
        }

        private static void ListarAssociados()
        {
            Console.WriteLine(associacao.ListarAssociados());
        }

        private static Habilidade LerRequisito()
        {
            Console.Write("Descricao do requisito: ");
            string descricaoHabilidade = Console.ReadLine();
            return new Habilidade(descricaoHabilidade);
        }

        private static Demanda PesquisarDemanda()
        {
            int idDemanda;
            Console.Write("ID da Demanda: ");
            idDemanda = int.Parse(Console.ReadLine());
            return associacao.PesquisarDemanda(idDemanda);
        }

        private static void IncluirRequisito()
        {
            ListarHabilidades();
            Demanda demanda = PesquisarDemanda();
            Habilidade requisito = LerRequisito();
            if (!associacao.HabilidadeEhReconhecida(requisito))
            {
                ImprimirErro($"O requisito \"{requisito}\" não é reconhecida.");
                return;
            }
            demanda.AdicionarRequisito(requisito);
            Console.WriteLine("\n" + demanda.Relatorio());
        }

        public static void ExcluirRequisito()
        {
            Demanda demanda = PesquisarDemanda();
            Habilidade requisito = LerRequisito();
            if (!associacao.HabilidadeEhReconhecida(requisito))
            {
                ImprimirErro($"O requisito \"{requisito}\" não é reconhecida.");
                return;
            }
            demanda.RemoverRequisito(requisito);
            Console.WriteLine("\n" + demanda.Relatorio());
        }

        private static void AtribuirPrestador()
        {
            Demanda demanda = PesquisarDemanda();
            Associado associado = PesquisarAssociado();
            associacao.AtribuirPrestador(demanda, associado);
            Console.WriteLine("\n" + demanda.Relatorio());
        }

        private static void EncerrarDemanda()
        {
            Demanda demanda = PesquisarDemanda();
            associacao.EncerrarDemanda(demanda);
            Console.WriteLine("\n" + demanda.Relatorio());
        }

        private static void MenuRelatorios()
        {
            int escolha = -1;
            do
            {
                try
                {
                    Cabecalho();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("—----------------------Gerenciando Demandas—---------------------");
                    Console.ResetColor();
                    Console.WriteLine(
                        " 1 - Demandas pendentes\r\n" +
                        " 2 - Associados que não podem registrar demandas\r\n" +
                        " 3 - Demandas ainda não alocadas\r\n" +
                        " 4 - Demandas que podem ser atendidas por um associado\r\n" +
                        " 5 - Dez (10) demandas que geram mais créditos\r\n" +
                        " 6 - Associados hábeis para atender uma demanda\r\n" +
                        " 7 - Média de diferença de tempo atendimento/prazo das demandas\r\n" +
                        " 8 - Dez (10) associados com maior número de créditos\r\n" +
                        " 9 - Demandas solucionáveis por um conjunto de habilidades\r\n" +
                        "10 - Demandas que demoraram mais que a média de tempo\r\n" +
                        "11 - Associados que possuem demandas não atendidas\r\n" +
                        "12 - Serviços prestados por associado\r\n" +
                        " 0 - Voltar\r\n"
                    );
                    escolha = LerNumero("Escolha uma opção: ");
                    Console.WriteLine();
                    switch (escolha)
                    {
                        case 1:
                            Console.WriteLine(associacao.DemandasPendentes());
                            break;
                        case 2:
                            Console.WriteLine(associacao.RelatorioAssociados(assoc => assoc.PodeSolicitar() == false));
                            break;
                        case 3:
                            Console.WriteLine(associacao.RelatorioDemandas(demanda => demanda.VerificarPrestador(null)));
                            break;
                        case 4:
                            DemandasSolucionaveisPorAssociado();
                            break;
                        case 5:
                            Console.WriteLine(associacao.ListarTop10DemandasMaiorCredito());
                            break;
                        case 6:
                            AssociadosHabeisParaDemanda();
                            break;
                        case 7:
                            double media = associacao.MediaDiferencaTempoAtendimento();
                            Console.WriteLine($"Média: {media:F2} (dias)");
                            break;
                        case 8:
                            Console.WriteLine(associacao.RelatorioAssociadosMaiorSaldo());
                            break;
                        case 9:
                            DemandasSolucionaveisPorHabilidades();
                            break;
                        case 10:
                            Console.WriteLine(associacao.DemandasTempoAcimaMedia());
                            break;
                        case 11:
                            Console.WriteLine(associacao.RelatorioAssociados(assoc => assoc.Demandas().Any(d => d.EstahAberta())));
                            break;
                        case 12:
                            ServicosPrestadosPorAssociado();
                            break;
                        case 0: // Voltar
                            break;
                        default:
                            Console.WriteLine("Opção inválida, por favor tente novamente.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    ImprimirErro(e.Message);
                }
                if (escolha != 0) Pausa();
            } while (escolha != 0);
        }

        private static void CadastrarAssociado()
        {
            Console.Write("Informe o nome: ");
            string nome = Console.ReadLine();
            Associado associado = new Associado(nome);
            associacao.CadastrarAssociado(associado);
            Console.WriteLine("\n" + associado);
        }

        private static void CadastrarDemanda()
        {
            string descricao; int tempoPrevisto, prazoPrevisto, idSolicitante;
            Console.Write("Descrição: ");
            descricao = Console.ReadLine();
            Console.Write("Tempo previsto em minutos (múltiplo de 30): ");
            tempoPrevisto = int.Parse(Console.ReadLine());
            Console.Write("Prazo previsto (em dias): ");
            prazoPrevisto = int.Parse(Console.ReadLine());

            // Pesquisa o associado antes de criar a demanda porque se não encontrar, lança exceção
            Console.Write("ID do associado soliciante: ");
            idSolicitante = int.Parse(Console.ReadLine());
            Associado solicitante = associacao.PesquisarAssociado(idSolicitante);

            Demanda demanda = new Demanda(descricao, tempoPrevisto, prazoPrevisto);
            associacao.CadastrarDemanda(demanda, solicitante);
            Console.WriteLine("\n" + demanda.Relatorio());
        }

        private static void ImprimirErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

        private static void Pausa()
        {
            Console.Write("\nDigite ENTER para continuar");
            Console.ReadLine();
        }

        private static void Config()
        {
            string nomeArquivo = "associados.txt";
            try
            {
                associacao.Config(nomeArquivo);
                return;
            }
            catch (FileNotFoundException fnf)
            {
                ImprimirErro($"Arquivo {nomeArquivo} não encontrado.");
            }
            catch (Exception ex)
            {
                ImprimirErro(ex.Message);
            }
            Environment.Exit(-1);
        }

        private static void DemandasSolucionaveisPorHabilidades()
        {
            int totalHabilidades, contador;

            ListarHabilidades();
            Console.Write("Total de habilidades a pesquisar: ");
            totalHabilidades = int.Parse(Console.ReadLine());

            if (totalHabilidades <= 0) throw new("Número inválido!");
            contador = 1;
            HashSet<Habilidade> habilidades = new();
            do
            {
                Habilidade h = LerHabilidade(contador);
                if (!associacao.HabilidadeEhReconhecida(h))
                {
                    ImprimirErro("A habilidade informada não está cadastrada.");
                    continue;
                }
                if (!habilidades.Add(h))
                {
                    Console.WriteLine("Habilidade já inserida! Informe outra.");
                    continue;
                }
                contador++;
            } while (contador <= totalHabilidades);
            Console.WriteLine("\n" + associacao.RelatorioDemandas(d => d.VerificarRequisitos(habilidades)));
        }

        private static int LerNumero(string mensagem)
        {
            int valor;
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out valor))
            {
                return valor;
            }
            return int.MinValue;
        }

        private static void DemandasSolucionaveisPorAssociado()
        {
            Associado associado = PesquisarAssociado();
            Console.WriteLine($"\nASSOCIADO: {associado}\n\nDEMANDAS QUE PODEM SER ATENDIDAS:");
            string relatorio = associacao.RelatorioDemandas(demanda => associado.VerificarRequisitos(demanda));
            Console.WriteLine(relatorio);
        }

        private static void AssociadosHabeisParaDemanda()
        {
            Demanda demanda = PesquisarDemanda();
            Console.WriteLine("\n" + demanda.Relatorio());
            Console.WriteLine("ASSOCIADOS HÁBEIS PARA A DEMANDA:");
            Console.WriteLine(associacao.RelatorioAssociadosHabeis(demanda));
        }

        private static void ServicosPrestadosPorAssociado()
        {
            Associado associado = PesquisarAssociado();
            Console.WriteLine("\n" + associacao.RelatorioServicosPrestados(associado));
        }
    }
}
