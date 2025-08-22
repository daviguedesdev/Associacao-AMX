using System.Dynamic;
using System.Text;

namespace AMXAssociacao
{
    /// <summary>
    /// Classe Cemanda para a AMXAssociacao de moradores. Uma demanda é
    /// responsável por gerenciar os dados referentes à prestação de
    /// serviços por moradores da associação.
    /// </summary>
    class Demanda
    {
        /// <summary>
        /// Constante: base de tempo para duração da demanda.
        /// O tempoPrevisto deve ser um múltiplo dessa constante.
        /// </summary>
        private const int FracaoTempoEmMinutos = 30;

        /// <summary>
        /// Estático: Número identificador para a próxima demanda a ser criada.
        /// </summary>
        private static int s_proximoID = 1;

        /// <summary>
        /// Número identificador da demanda.
        /// </summary>
        private int _id;

        /// <summary>
        /// Descrição textual da demanda.
        /// </summary>
        private string _descricao;

        /// <summary>
        /// Conjunto de Habilidades necessárias para execução da demanda.
        /// </summary>
        private HashSet<Habilidade> _requisitos;

        /// <summary>
        /// Tempo previsto para execução da demanda em minutos.
        /// </summary>
        private int _tempoPrevisto;

        /// <summary>
        /// Morador responsável pela execução da demanda em minutos.
        /// </summary>
        private Associado? _prestador;

        /// <summary>
        /// Estado da demanda: aberta (true) ou fechada (false).
        /// </summary>
        private bool _aberta;

        /// <summary>
        /// Prazo previsto para conclusão (em dias).
        /// </summary>
        private int _prazoPrevisto;

        /// <summary>
        /// Data de criação da demanda.
        /// </summary>
        private DateOnly? _dataCriacao;

        /// <summary>
        /// Data de encerramento/conclusão da demanda.
        /// </summary>
        private DateOnly? _dataConclusao;

        /// <summary>
        /// Construtor padrão. Cria uma demanda a partir da descriao, tempo previsto e prazo.
        /// </summary>
        /// <param name="decricao">Descrição da demanda.</param>
        /// <param name="tempoPrevisto">Tempo previsto de duração (múltiplo de FracaoTempoEmMinutos).</param>
        /// <param name="prazoPrevisto">Prazo previsto para conclusão em dias.</param>
        public Demanda(string descricao, int tempoPrevisto, int prazoPrevisto)
        {
            ValidarDados(descricao, tempoPrevisto, prazoPrevisto);
            _id = s_proximoID++;
            _descricao = descricao.Trim().ToUpper();
            _requisitos = new HashSet<Habilidade>();
            _tempoPrevisto = tempoPrevisto;
            _prestador = null;
            _aberta = true;
            _prazoPrevisto = prazoPrevisto;
            _dataCriacao = DateOnly.FromDateTime(DateTime.Today);
            _dataConclusao = null;
        }

        /// <summary>
        /// Valida os dados recebidos pelo construtor da classe.
        /// </summary>
        /// <param name="decricao">Descrição da demanda.</param>
        /// <param name="tempoPrevisto">Tempo previsto de duração (múltiplo de FracaoTempoEmMinutos).</param>
        /// <param name="prazoPrevisto">Prazo previsto para conclusão em dias.</param>
        /// <exception cref="ArgumentException">
        ///  * Se a descricao for nula ou string vazia;
        ///  * Se o tempoPrevisto for menor ou igual a zero, ou não for múltiplo de FracaoTempoEmMinutos;
        ///  * Se o prazoPrevisto for menor ou igual a zero.
        /// </exception>
        private void ValidarDados(string descricao, int tempoPrevisto, int prazoPrevisto
        )
        {
            if (descricao == null || descricao.Trim().Length == 0)
            {
                throw new ArgumentException("Descrição inválida.");
            }
            if (tempoPrevisto <= 0 || tempoPrevisto % FracaoTempoEmMinutos != 0)
            {
                throw new ArgumentException(
                    "Tempo previsto (em minutos) para execução inválido."
                );
            }
            if (prazoPrevisto <= 0)
            {
                throw new ArgumentException(
                    "Prazo previsto (em dias) para conclusão inválido."
                );
            }
        }

        /// <summary>
        /// Adiciona uma habilide ao conjunto de requisitos para executar a demanda.
        /// </summary>
        /// <param name="habilidade">Habilidade a incluir no conjunto de requisito.</param>
        /// <returns>Verdadeiro caso a inclusão seja bem sucessida, falso caso contrário.</returns>
        /// <exception cref="ArgumentException">Se a habilidade for nula.</exception>
        public bool AdicionarRequisito(Habilidade habilidade)
        {
            if (habilidade == null)
            {
                throw new ArgumentException("Habilidade não pode ser nula.");
            }
            if (!EstahAberta())
            {
                throw new ArgumentException("A demanda está fechada.");
            }
            if (_prestador != null && !_prestador.PossuiHabilidade(habilidade))
            {
                throw new ArgumentException($"O prestador associado à demanda não possui a habilidade {habilidade}.");
            }
            _requisitos.Add(habilidade);
            return true;
        }

        /// <summary>
        /// Remove uma habilide do conjunto de requisitos para executar a demanda.
        /// </summary>
        /// <param name="habilidade">Habilidade a remover do conjunto de requisitos.</param>
        /// <returns> Verdadeiro caso a remoção seja bem sucessida, falso caso contrário.</returns>
        /// <exception cref="ArgumentException">Se a habilidade for nula.</exception>
        public bool RemoverRequisito(Habilidade habilidade)
        {
            if (habilidade == null)
            {
                throw new ArgumentException("Habilidade não pode ser nula.");
            }
            if (!EstahAberta())
            {
                throw new ArgumentException("A demanda está fechada.");
            }
            return _requisitos.Remove(habilidade);
        }

        /// <summary>
        /// Verifica se a habilidade recebida é um requisito da demanda.
        /// </summary>
        /// <param name="habilidade"Requisitos a ser verificado.</param>
        /// <returns>Verdadeiro se a habilidades faz parte dos requisitos da demanda, falso caso contrário.</returns>
        public bool PossuiRequisito(Habilidade habilidade)
        {
            return _requisitos.Contains(habilidade);
        }

        /// <summary>
        /// Verifica se o conjunto de habilidade recebido é suficiente para
        /// realizar a demanda. Isto é, se o conjunto de requisitos da demanda
        /// é um subconjunto das habilidades recebidas.
        /// </summary>
        /// <param name="habilidades">Conjunto de requisitos a ser verificado.</param>
        /// <returns>Verdadeiro se o conjunto de habilidades atende aos requisitos da demanda, falso caso contrário.</returns>
        /// <exception cref="ArgumentException">Se outrosRequisitos for nulo.</exception>
        public bool VerificarRequisitos(HashSet<Habilidade> habilidades)
        {
            if (habilidades == null)
            {
                throw new ArgumentException("Habilidades não podem ser nulas.");
            }
            if (_requisitos.Count == 0)
            {
                throw new InvalidOperationException($"A {_descricao} ID#{_id:D4} não possui requisitos cadastrados.");
            }
            int encontrados = 0;
            foreach (Habilidade r in _requisitos)
            {
                if (habilidades.Contains(r)) encontrados++;
            }
            return encontrados != 0 && encontrados == _requisitos.Count;
        }

        /// <summary>
        /// Atribui um responsável por executar a demanda.
        /// </summary>
        /// <param name="prestador">Um Associado cadastrado no sistema.</param>
        /// <returns>Veradeiro se o prestador atender todos os requisitos da demanda, falso caso contrário.</returns>
        /// <exception cref="ArgumentException">
        /// * Se o prestador for nulo.
        /// * Se a demanda estiver fechada.
        /// * Se o prestador não possuir os requisitos necessários.
        /// </exception>
        public bool AtribuirPrestador(Associado prestador)
        {
            if (prestador == null)
            {
                throw new ArgumentException("Prestador não pode ser nulo.");
            }
            if (!EstahAberta())
            {
                throw new ArgumentException($"A demanda está fechada.");
            }
            if (!prestador.VerificarRequisitos(this))
            {
                throw new ArgumentException($"O prestador {prestador} não possui os requisitos necessários.");
            }
            _prestador = prestador;
            return true;
        }

        /// <summary>
        /// Encerra a demanda atual.
        /// </summary>
        /// <returns>Verdadeiro se a demanda for encerrada, falso caso não seja possível encerrar.</returns>
        /// <exception cref="ArgumentException">
        /// Caso tente encerrar com status de atendida, mas não tenha cadastrado previamente o prestador do serviço.
        /// </exception>
        public bool EncerrarDemanda()
        {
            if (!_aberta)
            {
                return false;
            }
            if (_prestador == null)
            {
                throw new ArgumentException("Demanda atendida precisa ter um prestador.");
            }
            _prestador.ReceberServico(_tempoPrevisto);
            _aberta = false;
            _dataConclusao = DateOnly.FromDateTime(DateTime.Today);
            return _aberta;
        }

        /// <summary>
        /// Verificar se o associado recebido como parâmetro é o responsável pela demanda.
        /// </summary>
        /// <param name="prestador">Associado que se deseja saber se é o responsável pela demanda.</param>
        /// <returns>Verdadeiro o Associado recebido for igual ao responsável pela demandas.</returns>
        /// <remarks>Caso o parâmetro recebido e o prestador sejam nulos, o método retorna verdadeiro.</remarks>
        public bool VerificarPrestador(Associado? prestador)
        {
            return _prestador == prestador || _prestador != null && _prestador.Equals(prestador);
        }

        /// <summary>
        /// Getter para a descricao da demanda para uso em filtros e relatórios.
        /// </summary>
        /// <returns>String com a descrição da demanda.</returns>
        public string Descricao()
        {
            return _descricao;
        }

        /// <summary>
        /// Getter para o tempo previsto (duração em minutos) da demanda para uso em filtros e relatórios.
        /// </summary>
        /// <returns>O tempo previsto (em dias) para conclusão da demanda.</returns>
        public int TempoPrevisto()
        {
            return _tempoPrevisto;
        }

        /// <summary>
        /// Getter para o estado (aberta | fechada) da demanda para uso em filtros e relatórios.
        /// </summary>
        /// <returns>True caso a demanda esteja aberta, false caso contrário.</returns>
        public bool EstahAberta()
        {
            return _aberta;
        }

        /// <summary>
        /// Tempo gasto para atendimento (em dias) da demanda para uso em filtros e relatórios.
        /// </summary>
        /// <returns>Dias de diferença entre as datas de conclusão/encerramento
        /// e de criação da demanda, ou -1 caso a demanda ainda esteja aberta.</returns>
        public int TempoAtendimento()
        {
            if (_aberta)
            {
                return 0;
            }
            return _dataConclusao.Value.DayNumber - _dataCriacao.Value.DayNumber;
        }

        /// <summary>
        /// Calcula a diferença de tempo, em dias, entre o prazo previsto e o tempo de atendimento.
        /// </summary>
        /// <returns>Valor real (double) com a diferença entre o prazo previsot e o temo de atendimento.</returns>
        public double DiferencaTempoAtendimento()
        {
            return (double) TempoAtendimento() - _prazoPrevisto;
        }

        /// <summary>
        /// HashCode do objeto obtido a partir do atributo _id.
        /// </summary>
        /// <returns>HashCode (_id) do objeto.</returns>
        public override int GetHashCode()
        {
            return _id;
        }

        /// <summary>
        /// Determina se o objeto especificado é igual ao objeto atual.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado com o objeto atual.</param>
        /// <returns>Verdadeiro se o objeto especificado for igual ao objeto atual, falso caso contrário.</returns>
        public override bool Equals(object? obj)
        {
            Demanda outraDemanda = obj as Demanda;
            if (obj == null || outraDemanda == null)
            {
                return false;
            }
            return _id == outraDemanda._id && _descricao.Equals(outraDemanda);
        }

        /// <summary>
        /// Representação simplificada de uma demanda em forma de string.
        /// </summary>
        /// <returns>String com (alguns) dados do objeto.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_descricao} (ID#{_id:D4}) - ");
            sb.Append(EstahAberta() ? "Aberta" : "Fechada");
            sb.Append(_prestador == null ? " - Não atribuída" : $" - Prestador {_prestador}");
            if(_requisitos.Count > 0)
            {
                sb.Append(" - Requisitos: ");
                sb.Append(String.Join(", ", _requisitos));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Representação detalhada de uma demanda em forma de string.
        /// </summary>
        /// <returns>String com (alguns) dados do objeto.</returns>
        public string Relatorio()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{_descricao} (ID#{_id:D4}) - {(EstahAberta() ? "Aberta" : "Fechada")}");
            sb.AppendLine($"Criada em {_dataCriacao.Value.ToShortDateString()}");
            DateOnly dataPrevista = DateOnly.FromDayNumber(_dataCriacao.Value.DayNumber + _prazoPrevisto);
            sb.AppendLine($"Data prevista {dataPrevista.ToShortDateString()}");
            if (!EstahAberta())
            {
                sb.AppendLine($"Concluída em {_dataConclusao.Value.ToShortDateString()}");
            }
            if (_prestador != null)
            {
                sb.AppendLine($"Prestador: {_prestador.ToString()}");
            }
            if(_requisitos.Count > 0)
            {
                sb.AppendLine($"Requisitos: {String.Join(", ", _requisitos)}");
            }
            return sb.ToString();
        }
    }
}

