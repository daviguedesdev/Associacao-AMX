using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMXAssociacao
{
    class Associacao
    {
        private string _nome;
        private LinkedList<Associado> _associados;
        private HashSet<Habilidade> _habilidadesReconhecidas;

        public Associacao(string nome) // construtor da classe
        {
            _nome = nome;
            _associados = new LinkedList<Associado>();
            _habilidadesReconhecidas = new HashSet<Habilidade>();
        }

        /// <summary>
        /// se a habilidade existir retorna false, se não adciona ela no inicio da lista
        /// </summary>
        /// <param name="habilidade"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se a habildidade for nula.</exception>
        public bool CadastrarHabilidade(Habilidade habilidade)
        {
            if (habilidade == null)
            {
                throw new ArgumentException("A habilidade não pode ser nula.");
            }
            return _habilidadesReconhecidas.Add(habilidade);
        }

        /// <summary>
        /// retorna true se removeu e false se não encontrou
        /// </summary>
        /// <param name="habilidade"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se a habildidade for nula ou .</exception>
        /// <exception cref="InvalidOperationException">Se a habildidade estiver vinculada a algum(a) morador/demanda.</exception>
        public bool ExcluirHabilidade(Habilidade habilidade)
        {
            if (habilidade == null)
            {
                throw new ArgumentException("A habilidade não pode ser nula.");
            }

            // Tentar remover primeiro porque a habilidade pode não estar cadastrada (retorna falso)
            if (!_habilidadesReconhecidas.Remove(habilidade))
            {
                return false;
            }

            // Se a habilidade estiver em uso, adiciona novamente no conjunto e lança exceção
            foreach (Associado assoc in _associados)
            {
                string usos = assoc.UsosDaHabilidade(habilidade);
                if (usos.Length > 0)
                {
                    _habilidadesReconhecidas.Add(habilidade);
                    throw new InvalidOperationException($"Habilidade em uso por\r\n{usos}...");
                }
            }
            return true;
        }

        /// <summary>
        /// retorna true se a habilidade recebida está cadastrada
        /// </summary>
        /// <param name="habilidade"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se a habildidade for nula.</exception>
        public bool HabilidadeEhReconhecida(Habilidade habilidade)
        {
            if (habilidade == null)
            {
                throw new ArgumentException("A habilidade não pode ser nula.");
            }

            return _habilidadesReconhecidas.Contains(habilidade);
        }

        /// <summary>
        /// Chama a lista de habilidades reconhecidas e converte para uma unica string com separado por "\n"
        /// </summary>
        /// <returns></returns>
        public string ListarHabilidades()
        {
            string msg = string.Join(", ", _habilidadesReconhecidas);
            return msg.Length > 0 ? msg : "Não há habilidades cadastradas.";
        }

        /// <summary>
        /// Chama a lista de associados e converte para uma unica string com separado por "\n"
        /// </summary>
        /// <returns></returns>
        public string ListarAssociados()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Associado assoc in _associados)
            {
                sb.AppendLine(assoc.Relatorio());
            }

            return sb.Length > 0 ? sb.ToString() : "Não há associados cadastrados.";
        }

        /// <summary>
        /// Se ja estiver cadastrado retorna false, se não adciona o associado no inicio da lista e retorna true indicando sucesso no cadastro.
        /// </summary>
        /// <param name="associado"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se o associado for nulo.</exception>
        public bool CadastrarAssociado(Associado associado)
        {
            if (associado == null)
            {
                throw new ArgumentException("O associado não pode ser nulo.");
            }

            if (_associados.Contains(associado))
            {
                return false;
            }

            _associados.AddLast(associado); // Inserir no fim para ficar na ordem de cadastro
            return true;
        }

        // procura na lista _associados um associado e retorna o associado se encontrar, se não encontrar retorna null
        public Associado PesquisarAssociado(int id)
        {
            foreach (Associado assoc in _associados)
            {
                if (assoc.GetHashCode() == id)
                {
                    return assoc;
                }
            }

            throw new InvalidOperationException($"Associado MAT#{id:D4} não localizado.");
        }

        //procura associados na lista _associados que possam prestar a Demanda informada, retorna uma lista com os associados que atendem o criterio
        public string PesquisarPrestadores(Demanda demanda)
        {
            LinkedList<Associado> prestadores = new LinkedList<Associado>();

            foreach (Associado assoc in _associados)
            {
                if (assoc.VerificarRequisitos(demanda))
                {
                    prestadores.AddLast(assoc);
                }

            }
            return string.Join('\n', prestadores);
        }

        // Solicita o serviço e debita os créditos do associado, depois inclui na lista de demandas sem prestador
        public bool CadastrarDemanda(Demanda d, Associado solicitante)
        {
            solicitante.SolicitarServico(d);
            return true;
        }

        //o primeiro verifica se a demanda está aberta, se não estiver retorna false
        //o segundo if verifica se a demanda ja foi atendida, se ja foi não muda o prestador, então retorna false
        //o returno é o resultado do método AtribuirPrestador da classe demanda.
        public bool AtribuirPrestador(Demanda demanda, Associado prestador)
        {
            if (demanda == null)
            {
                throw new ArgumentException("Demanda não pode ser nula.");
            }
            if (prestador == null)
            {
                throw new ArgumentException("Prestador não pode ser nulo.");
            }
            return demanda.AtribuirPrestador(prestador);
        }

        //percorre as listas de demandas, verifica se o id é igual ao que foi passado, se encontrar retorna a demanda encontrada, se terminar de percorrer e não achar retorna null
        public Demanda PesquisarDemanda(int id)
        {
            Demanda servico;
            foreach (Associado a in _associados)
            {
                servico = a.PesquisarDemanda(id);
                if (servico != null)
                {
                    return servico;
                }
            }
            throw new InvalidOperationException($"Demanda ID#{id:D4} não localizada.");
        }

        // verifica se a demanda d está registrada na lisa _demandas, se não estiver retorna false, se estiver define que ela foi atendida, remove a demanda da lista e retorna true
        public bool EncerrarDemanda(Demanda d)
        {
            return d.EncerrarDemanda();
        }

        // Cria uma nova lista resultados, percorre as demandas existentes e pra cada uma verifica se a demanda está aberta
        // se sim adciona à lista de resultados e depois retorna uma string com as informações dessa lista
        public String DemandasPendentes()
        {
            LinkedList<Demanda> resultados = new LinkedList<Demanda>();
            foreach (Associado a in _associados)
            {
                LinkedList<Demanda> demandasAssociado = a.DemandasPendentes();
                foreach (Demanda d in demandasAssociado)
                {
                    resultados.AddLast(d);
                }
            }
            return resultados.Count > 0 ? string.Join("\n", resultados) : "Não há demandas pendentes.";
        }

        // Cria uma nova lista resultados, percorre os associados existentes e pra cada um verifica se o prestador realizou alguma de suas demandas
        // se sim adciona à lista de resultados e depois retorna uma string com as informações dessa lista
        public String RelatorioServicosPrestados(Associado prestador)
        {
            LinkedList<Demanda> resultados = new LinkedList<Demanda>();
            foreach (Associado a in _associados)
            {
                LinkedList<Demanda> demandasAssociado = a.RelatorioServicosPrestados(prestador);
                foreach (Demanda d in demandasAssociado)
                {
                    resultados.AddLast(d);
                }
            }
            return resultados.Count > 0 ? string.Join("\n", resultados) : $"O associado {prestador} não prestou serviços.";
        }

        public void Config(string nomeArquivo)
        {
            GerarHabilidades();
            GerarClientes(nomeArquivo);
            GerarDemandas(2000);
        }

        private void GerarHabilidades()
        {
            string[] habilidades = ["H1", "H2", "H3", "H4", "H5", "H6", "H7"];
            foreach (string h in habilidades)
            {
                _habilidadesReconhecidas.Add(new Habilidade(h));
            }
        }

        private void GerarClientes(string nomeArquivo)
        {
            List<string> nomes = LerArquivo(nomeArquivo);
            Random random = new Random();
            int total = nomes.Count;
            int q2 = (int)(total * 0.30);           // 30% de associados com 2 habilidades
            int q3 = (int)(total * 0.20);           // 20% de associados com 3 habilidades
            int q4 = (int)(total * 0.20);           // 20% de associados com 4 habilidades
            int q5 = (int)(total * 0.20);           // 20% de associados com 5 habilidades
            int q7 = total - (q2 + q3 + q4 + q5);   // 10% de associados com 7 habilidades
            List<(int, int)> distribuicoes = [(q2, 2), (q3, 3), (q4, 4), (q5, 5), (q7, 7)];

            int indice = 0;
            foreach ((int quantidade, int habCount) in distribuicoes)
            {
                for (int i = 0; i < quantidade; i++)
                {
                    List<Habilidade> habilidades =
                                _habilidadesReconhecidas
                                .OrderBy(x => random.Next())
                                .Take(habCount)
                                .ToList();

                    Associado novo = new Associado(nomes[indice++]);
                    foreach (Habilidade h in habilidades)
                    {
                        novo.AdicionarHabilidade(h);
                    }
                    _associados.AddLast(novo);
                }
            }
        }

        private List<string> LerArquivo(string nomeArquivo)
        {
            List<string> nomes = File.ReadAllLines(nomeArquivo).ToList();
            if (nomes.Count == 0)
            {
                throw new InvalidDataException($"Arquivo {nomeArquivo} está vazio ou corrompido.");
            }
            return nomes;
        }

        private void GerarDemandas(int total)
        {
            Random random = new Random();
            int d1 = (int)(total * 0.30);           // 30% de demandas com 1 requisitos
            int d2 = (int)(total * 0.20);           // 20% de demandas com 2 requisitos
            int d3 = (int)(total * 0.20);           // 20% de demandas com 3 requisitos
            int d4 = (int)(total * 0.20);           // 20% de demandas com 4 requisitos
            int d5 = total - (d1 + d2 + d3 + d4);   // 10% de demandas com 5 requisitos
            List<(int, int)> distribuicoes = new() { (d1, 1), (d2, 1), (d3, 3), (d4, 2), (d5, 1) };

            int idDemanda = 1;
            foreach ((int quantDemandas, int quantRequisitos) in distribuicoes)
            {
                for (int i = 0; i < quantDemandas; i++)
                {
                    List<Habilidade> requisitos =
                        _habilidadesReconhecidas
                        .OrderBy(x => random.Next())
                        .Take(quantRequisitos)
                        .ToList();

                    int duracaoEmMinutos = GerarTempoDemanda(quantRequisitos, random);
                    int prazoEmDias = GerarPrazoDemanda(quantRequisitos);
                    Demanda novaDemanda = new Demanda($"Demanda {idDemanda++}", duracaoEmMinutos, prazoEmDias);
                    foreach (Habilidade r in requisitos)
                    {
                        novaDemanda.AdicionarRequisito(r);
                    }

                    IEnumerable<Associado> associadosComCreditos = _associados.Where(assoc => assoc.PodeSolicitar());
                    Associado solicitante = associadosComCreditos.ElementAt(random.Next(associadosComCreditos.Count()));
                    solicitante.SolicitarServico(novaDemanda);

                    IEnumerable<Associado> associadosHabilitados = _associados.Where(assoc => assoc.VerificarRequisitos(novaDemanda));
                    Associado prestador = associadosHabilitados.ElementAt(random.Next(associadosHabilitados.Count()));
                    novaDemanda.AtribuirPrestador(prestador);

                    EncerrarDemanda(novaDemanda);
                }
            }
        }

        private int GerarTempoDemanda(int quantRequisitos, Random rnd)
        {
            int duracaoEmMinutos = quantRequisitos switch
            {
                <= 2 => rnd.Next(30, 3 * 60 + 1),        // 30 min a 3h
                <= 4 => rnd.Next(3 * 60, 8 * 60 + 1),    // 3h a 8h
                _ => rnd.Next(8 * 60, 16 * 60 + 1)       // 8h a 16h
            };
            return duracaoEmMinutos - duracaoEmMinutos % 30;    // Precisa ser múltiplo de 30 minutos
        }

        private int GerarPrazoDemanda(int quantRequisitos)
        {
            return quantRequisitos switch
            {
                <= 2 => 7,  //  7 dias
                <= 4 => 10, // 10 dias
                _ => 14     // 14 dias
            };
        }

        /// <summary>
        /// Gera um relatório com os dez (10) associados com maior saldos.
        /// </summary>
        /// <returns>String com o relatório solicitado</returns>
        public string RelatorioAssociadosMaiorSaldo()
        {
            try
            {
                return _associados
                        .OrderByDescending(a => a.Saldo())
                        .Take(10)
                        .Select(a => a.ToString() + $", {a.Saldo():F2} créditos")
                        .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Não foi possível gerar o relatório. Não há associados cadastrados.");
            }
        }

        /// <summary>
        /// Gera um relatório com das demandas dos associados atendem ao predicado informado.
        /// </summary>
        /// <param name="condicao">Predicado aplicá-vel a uma demanda.</param>
        /// <returns>
        /// <c>string</c> com as demandas soluncionáveis.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Caso o predicado recebido seja nulo.
        /// </exception>
        public string RelatorioDemandas(Predicate<Demanda> condicao)
        {
            ArgumentNullException.ThrowIfNull(condicao, "A condição (predicado) não pode ser nula!");

            try
            {
                return _associados
                        .SelectMany(assoc => assoc.Demandas())
                        .Where(demanda => condicao.Invoke(demanda))
                        .Select(demanda => demanda.ToString())
                        .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException)
            {
                return "Demanda(s) não encontrada(s) com a condição informada.";
            }
        }

        /// <summary>
        /// Gera um relatório com os associados que atendem ao predicado informado.
        /// </summary>
        /// <param name="condicao">Predicado aplicá-vel a um associado.</param>
        /// <returns>
        /// <c>string</c> com os associados que atendem o predicado.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Caso o predicado recebido seja nulo.
        /// </exception>
        public string RelatorioAssociados(Predicate<Associado> condicao)
        {
            ArgumentNullException.ThrowIfNull(condicao, "A condição (predicado) não pode ser nula!");

            try
            {
                return _associados
                    .Where(assoc => condicao.Invoke(assoc))
                    .Select(assoc => assoc.Relatorio())
                    .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException ioe)
            {
                return "Associado(s) não encontrado(s) com a condição informada.";
            }
        }

        /// <summary>
        /// Lista todas as demandas dos asosciados ordenadas pelo ID.
        /// </summary>
        /// <returns>
        /// <c>string</c> com todas as demandas cadastradas.
        /// </returns>
        public string ListarDemandas()
        {
            try
            {
                return _associados
                       .SelectMany(assoc => assoc.Demandas())
                       .OrderBy(demanda => demanda.GetHashCode())
                       .Select(demanda => demanda.ToString())
                       .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException ioe)
            {
                return "Não há demandas cadastradas.";
            }
        }

        /// <summary>
        /// Gera um relatório dos associados que podem atender uma demanda específica.
        /// </summary>
        /// <returns>
        /// <c>string</c> com todas os associados que podem atender uma demanda.
        /// </returns>
        public string RelatorioAssociadosHabeis(Demanda demanda)
        {
            try
            {
                return _associados
                    .Where(assoc => assoc.VerificarRequisitos(demanda))
                    .OrderByDescending(a => a.Saldo())
                    .Select(assoc => assoc.ToString())
                    .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException ioe)
            {
                return "Não há associados hábeis para a atender a demanda.";
            }
        }

        /// <summary>
        /// Calcula a diferença de tempo, em dias, entre o prazo previsto e o tempo de atendimento.
        /// </summary>
        /// <returns>Valor real (double) com a diferença entre o prazo previsot e o temo de atendimento.</returns>
        public double MediaDiferencaTempoAtendimento()
        {
            try
            {
                return _associados
                    .SelectMany(assoc => assoc.Demandas())
                    .Average(demanda => demanda.DiferencaTempoAtendimento());
            }
            catch (InvalidOperationException ioe)
            {
                return 0;   // Não há demandas cadastradas.
            }
        }

        /// <summary>
        /// Gera um relatório com as 10 demandas que oferecem a maior quantidade de créditos,
        /// ordenada de forma decrescente pela quantidade de créditos.
        /// </summary>
        /// <returns>String com as 10 demandas que geram a maior quantidade de créditos.</returns>
        public string ListarTop10DemandasMaiorCredito()
        {
            try
            {
                Comparer<Demanda> comparador = Comparer<Demanda>.Create(
                    (d1, d2) => d1.TempoPrevisto() - d2.TempoPrevisto()
                );
                return _associados
                       .SelectMany(assoc => assoc.Demandas())
                       .OrderDescending(comparador)
                       .Take(10)
                       .Select(d => $"{d.Descricao()} ID#{d.GetHashCode():D4}, duração: {d.TempoPrevisto()} minutos")
                       .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException ioe)
            {
                return "Não há demandas cadastradas.";
            }
        }

        /// <summary>
        /// Gera um relatório as demandas que demoraram mais do que a média do tempo de atendimento
        /// para serem atendidas.
        /// </summary>
        /// <returns>String com as demandas que demoraram mais que a média do tempo de atendimenteo.</returns>
        public string DemandasTempoAcimaMedia()
        {
            try
            {
                double media = _associados
                    .SelectMany(assoc => assoc.Demandas())
                    .Average(demanda => demanda.TempoPrevisto());

                return _associados
                    .SelectMany(assoc => assoc.Demandas())
                    .Where(demanda => demanda.TempoPrevisto() > media)
                    .Select(Demanda => Demanda.ToString())
                    .Aggregate((str1, str2) => str1 + "\n" + str2);
            }
            catch (InvalidOperationException ioe)
            {
                return "Não há demandas cadastradas.";
            }
        }
    }
}
