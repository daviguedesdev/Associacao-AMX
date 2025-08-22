using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMXAssociacao
{
    /// <summary>
    /// Representa um Associado, contendo um identificador único, a matrícula, nome, lista de habilidades e lista de demandas.
    /// </summary>
    class Associado
    {
        private static int ProximaMatricula = 1;
        private int _matricula;
        private string _nome;
        private Carteira _carteira;
        private HashSet<Habilidade> _habilidades;
        private LinkedList<Demanda> _demandas;

        /// <summary>
        /// Inicializa uma nova instância da classe <c>Associado</c>, atribuindo automaticamente a próxima matrícula sequencial.
        /// Também inicializa o nome com o valor informado, uma nova carteira, uma lista vazia de habilidades e uma lista vazia de demandas.
        /// </summary>
        /// <param name="nome">O nome do associado.</param>
        public Associado(string nome)
        {
            if (nome == null || nome.Length == 0)
                throw new ArgumentNullException($"O nome não pode ser vazio ou nulo");

            _matricula = ProximaMatricula++;
            _nome = nome.ToUpper();
            _carteira = new Carteira();
            _habilidades = new HashSet<Habilidade>();
            _demandas = new LinkedList<Demanda>();
        }

        /// <summary>
        /// Adiciona uma nova habilidade para o associado.
        /// </summary>
        /// <param name="h">Habilidade a ser adicionada</param>
        /// <returns>
        /// Caso o associado não possua a habilidade, ela é adicionada ao final da lista de habilidades e retorna verdadeiro.
        /// Caso contrário, não adiciona a habilidade na lista e retorna falso.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando a habilidade fornecida é nula.
        /// </exception>
        public bool AdicionarHabilidade(Habilidade habilidade)
        {
            ArgumentNullException.ThrowIfNull(habilidade);
            return _habilidades.Add(habilidade);
        }

        /// <summary>
        /// Remove uma habilidade do associado
        /// </summary>
        /// <param name="h">Habilidade a ser removida</param>
        /// <returns>
        /// Se o associado não possui a habilidade, retorna falso.
        /// Caso contrário, a habilidade é removida da lista e retorna verdadeiro
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando a habilidade fornecida é nula.
        /// </exception>
        public bool RemoverHabilidade(Habilidade habilidade)
        {
            if (habilidade == null)
                throw new ArgumentNullException("A habilidade não pode ser nula!");

            return _habilidades.Remove(habilidade);
        }

        /// <summary>
        /// Solicita um serviço com base em uma demanda, debitando o tempo previsto do crédito disponível.
        /// </summary>
        /// <param name="d">A demanda a ser solicitada.</param>
        /// <returns>
        /// Retorna o saldo de créditos restante se a solicitação foi feita com sucesso. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando a dema fornecida é nula.
        /// </exception>
        public double SolicitarServico(Demanda d)
        {
            if (d == null)
                throw new ArgumentNullException("A demanda não pode ser nula!");

            if (!PodeSolicitar())
                throw new ArgumentException($"{ToString()} não possui créditos suficiente.");

            _carteira.Debitar(d.TempoPrevisto());
            _demandas.AddLast(d);
            return _carteira.Saldo();
        }

        /// <summary>
        /// Verifica se o cliente pode solicitar uma demanda
        /// </summary>
        /// <param name="d">Demanda a ser verificada</param>
        /// <returns>
        /// Se a demanda está aberta e o associado possui as habilidades, retorna verdadeiro
        /// Caso contrário, retorna falso.
        /// </returns>
        public bool PodeSolicitar()
        {
            return _carteira.PodeDebitar();
        }

        /// <summary>
        /// Verifica se o associado possui todas as habilidades exigidas pela demanda.
        /// Caso possua, atribui a demanda ao associado.
        /// </summary>
        /// <param name="d">A demanda a ser verificada e atribuída ao associado.</param>
        /// <returns>O total de minutos acumulados.</returns>
        /// <exception cref="ArgumentException">
        /// Lançada quando a duração da Demanda for inválida.
        /// </exception>
        /// <remarks>Se a quantidade de minutos acumulada atinger o valor
        /// mínimo de conversão, então haverá conversão de minutos para créditos.
        /// Assim, o valor de minutos retornado será o excedente que não foi
        /// convertido para créditos</remarks>
        public int ReceberServico(int duracaoDemanda)
        {
            if (duracaoDemanda <= 0)
                throw new ArgumentException($"A duração da Demanda (em minutos) dever ser maior que zero.");

            return _carteira.AdicionarMinutos(duracaoDemanda);
        }

        /// <summary>
        /// Verifica se o associado possui uma habilidades específica.
        /// </summary>
        /// <param name="habilidade">A habilidade a ser verificada.</param>
        /// <returns>
        /// <c>true</c> se o associado possuir a habilidade fornecida, caso contrário, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando a habilidade fornecida é nula.
        /// </exception>
        public bool PossuiHabilidade(Habilidade habilidade)
        {
            if (habilidade == null)
                throw new ArgumentNullException("A habilide não pode ser nula!");

            return _habilidades.Contains(habilidade);
        }

        /// <summary>
        /// Verifica se o associado possui todas as habilidades requeridas pela demanda.
        /// </summary>
        /// <param name="demanda">A demanda a ser assumida.</param>
        /// <returns>
        /// <c>true</c> se o associado possuir todas as habilidades necessárias com base nos requisitos; 
        /// caso contrário, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando a demanda fornecida é nula.
        /// </exception>
        public bool VerificarRequisitos(Demanda demanda)
        {
            if (demanda == null)
                throw new ArgumentNullException("Demanda não pode ser nula!");

            return demanda.VerificarRequisitos(_habilidades);
        }

        /// <summary>
        /// Retorna uma lista encadeada com as demandas do associado.
        /// </summary>
        /// <returns>
        /// LinkedList contendo todas as demandas do associado.
        /// </returns>
        public LinkedList<Demanda> Demandas()
        {
            return _demandas;
        }

        /// <summary>
        /// Procura uma demanda na lista de demandas do associado com base no identificador informado.
        /// </summary>
        /// <param name="idDemanda">Identificador da demanda a ser encontrada</param>
        /// <returns>
        /// Se o associado possui uma demanda com o identificador informado, retorna o objeto Demanda.
        /// </returns>
        public Demanda PesquisarDemanda(int idDemanda)
        {
            foreach (Demanda item in _demandas)
            {
                if (item.GetHashCode() == idDemanda)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Obtém uma lista de todas as demandas que estão atualmente abertas (pendentes).
        /// </summary>
        /// <returns>
        /// Uma lista de objetos <see cref="Demanda"/> contendo todas as demandas 
        /// que ainda não foram concluídas ou fechadas.
        /// Se não houver demandas pendentes, uma lista vazia será retornada.
        /// </returns>
        public LinkedList<Demanda> DemandasPendentes()
        {
            LinkedList<Demanda> demanda = new LinkedList<Demanda>();
            foreach (Demanda item in _demandas)
            {
                if (item.EstahAberta())
                    demanda.AddLast(item);
            }
            return demanda;
        }

        /// <summary>
        /// Obtém uma lista com as demandas realizadas pelo prestador recebido como parâmetro.
        /// </summary>
        /// <param name="prestador">Associado para o qual se deseja saber as demandas prestadas</param>
        /// <returns>
        /// Uma lista de objetos <see cref="Demanda"/> contendo as demandas 
        /// realizadas pelo prestador indicado.
        /// Se não houver demandas pendentes, uma lista vazia será retornada.
        /// </returns>
        public LinkedList<Demanda> RelatorioServicosPrestados(Associado prestador)
        {
            LinkedList<Demanda> demanda = new LinkedList<Demanda>();
            foreach (Demanda item in _demandas)
            {
                if (item.VerificarPrestador(prestador))
                    demanda.AddLast(item);
            }
            return demanda;
        }

        public string UsosDaHabilidade(Habilidade habilidade)
        {
            StringBuilder sb = new StringBuilder();
            if (_habilidades.Contains(habilidade))
                sb.AppendLine(this.ToString());

            foreach (Demanda d in _demandas)
            {
                if (d.PossuiRequisito(habilidade))
                    sb.AppendLine(d.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Determina se o objeto especificado é um <c>Associado</c> com a mesma matrícula que esta instância.
        /// </summary>
        /// <param name="o">O objeto a ser comparado com a instância atual.</param>
        /// <returns>
        /// <c>true</c> se o objeto especificado for um <c>Associado</c> e tiver a mesma matrícula; caso contrário, <c>false</c>.
        /// </returns>     
        public override bool Equals(Object? o)
        {
            return o != null && o is Associado outro && _matricula == outro._matricula;
        }

        /// <summary>
        /// Retorna uma representação textual simplificada do objeto, incluindo nome, matrícula, créditos e habilidades.
        /// </summary>
        /// <returns>
        /// Uma string formatada contendo os dados da instância atual.
        /// </returns>
        public override string ToString()
        {
            return $"{_nome} (MAT#{_matricula:D4})";
        }

        /// <summary>
        /// Retorna uma representação textual simplificada do objeto, incluindo nome, matrícula, créditos e habilidades.
        /// </summary>
        /// <returns>
        /// Uma string formatada contendo os dados da instância atual.
        /// </returns>
        public string Relatorio()
        {
            string msg = ToString() + $", {_carteira}";
            if(_habilidades.Count > 0)
                msg += ", Habilidade(s): " + string.Join(", ", _habilidades);

            return msg;
        }

        /// <summary>
        /// Calcula um código hash para o objeto atual com base no seu número de matrícula.
        /// </summary>
        /// <returns>
        /// Um valor inteiro que representa o código hash do objeto.
        /// Esse valor é derivado diretamente da **_matricula** do objeto.
        /// </returns>
        public override int GetHashCode()
        {
            return _matricula;
        }

        /// <summary>
        /// Getter para o saldo de créditos do associado.
        /// </summary>
        /// <returns>
        /// Double com saldo de créditos do associado.
        /// </returns>
        public double Saldo()
        {
            return _carteira.Saldo();
        }
    }
}