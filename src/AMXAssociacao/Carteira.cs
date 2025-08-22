namespace AMXAssociacao
{
    /// <summary>
    /// Classe Carteria para a AMXAssociacao de moradores. Uma Carteira é
    /// responsável por converter minutos trabalhados em créditos e minutos
    /// de demandas contratadas em créditos a serem pagos, cada uma com suas
    /// constantes de conversão específicas.
    /// </summary>
    class Carteira
    {
        /// <summary>
        /// Constante: minutos trabalhados necessários para cada conversão
        /// em créditos.
        /// </summary>
        private const int CreditosBaseMinutos = 180;

        /// <summary>
        /// Constante: créditos recebidos em cada conversão de minutos
        /// trabalhados.
        /// </summary>
        private const double CreditoTaxaConversao = 2d;

        /// <summary>
        /// Constante: mínimo de minutos a serem demandados para prestação
        /// de serviços. Representa a menor fração de tempo para a duração
        /// de uma demanda.
        /// </summary>
        private const int DebitoBaseMinutos = 30;

        /// <summary>
        /// Constante: créditos a serem cobrados para cada fração de tempo
        /// de uma demanda.
        /// </summary>
        private const double DebitoTaxaConversao = 0.5d;

        /// <summary>
        /// Constante: limite de créditos negativos que um associado pode ter.
        /// Após o saldo da carteira atingir esse valor, o associado não poderá
        /// solicitar mais serviços e terá que prestar serviços para acumular
        /// novos créditos.
        /// </summary>
        private const double LimiteCreditos = -10d;

        /// <summary>
        /// Minutos acumulados pelo associado pela prestação de serviços.
        /// </summary>
        private int _minutosTrabalhados;

        /// <summary>
        /// Creditos disponíveis após conversão dos minutos trabalhados.
        /// </summary>
        private double _creditos;

        /// <summary>
        /// Construtor padrão. Cria uma carteira vazia.
        /// </summary>
        public Carteira()
        {
            _minutosTrabalhados = 0;
            _creditos = 0;
        }

        /// <summary>
        /// Retorna o saldo atual da carteira em créditos.
        /// </summary>
        /// <returns>Double com o valor de créditos acumulados.</returns>
        public double Saldo()
        {
            return _creditos;
        }

        /// <summary>
        /// Adiciona minutos à carteira do associado.
        /// </summary>
        /// <param name="minutos">Quantidade minutos a adicionar. Deve ser um
        /// múltiplo positivo de DebitoBaseMinutos.</param>
        /// <returns>Valor de minutos acumulados.</returns>
        /// <remarks>Se a quantidade de minutos acumulada for atinger o valor
        /// CreditosBaseMinutos, então haverá conversão de minutos para créditos.
        /// Assim, o valor de minutos retornado será o excedente que não foi
        /// convertido para créditos</remarks>
        public int AdicionarMinutos(int minutos)
        {
            _minutosTrabalhados += ValidarMinutos(minutos);
            _creditos += MinutosParaCreditos(_minutosTrabalhados);
            _minutosTrabalhados %= CreditosBaseMinutos;
            return _minutosTrabalhados;
        }

        /// <summary>
        /// Valida minutos a serem adicionados ou debitados da carteira.
        /// </summary>
        /// <param name="minutos">Quantidade minutos a adicionar a serem
        /// validados. Deve ser um múltiplo positivo de DebitoBaseMinutos.</param>
        /// <returns>Inteiro com os minutos recebidos com parâmetro para
        /// facilitar uso da função de forma aninhada com outras chamadas de
        /// métodos.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se os minutos não forem um
        /// múltiplo positivo de DebitoBaseMinutos.</exception>
        private int ValidarMinutos(int minutos)
        {
            if (minutos < DebitoBaseMinutos || minutos % DebitoBaseMinutos != 0)
            {
                throw new ArgumentOutOfRangeException(
                    "minutos: devem ser múltiplos positivos de 30."
                );
            }
            return minutos;
        }

        /// <summary>
        /// Converte minutos trabalhados para créditos.
        /// </summary>
        /// <param name="minutos">Quantidade minutos a adicionar a serem validados.
        /// Deve ser um múltiplo positivo de CreditosBaseMinutos.</param>
        /// <returns>Double representando os créditos correspondentes. Cada
        /// CreditosBaseMinutos corresponde a um CreditoTaxaConversao
        /// créditos.</returns>
        private double MinutosParaCreditos(int minutos)
        {
            return CreditoTaxaConversao * (_minutosTrabalhados / CreditosBaseMinutos);
        }

        /// <summary>
        /// Debita os minutos desejados dos créditos em carteira.
        /// </summary>
        /// <param name="minutos">Quantidade minutos a serem debitados.</param>
        /// <returns>Double com os créditos disponívies após o débito.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se os minutos não forem um
        /// múltiplo positivo de DebitoBaseMinutos.
        /// <exception cref="ArgumentException">Se não houver créditos suficientes para realizar
        /// o débito.</exception>
        public double Debitar(int minutos)
        {
            minutos = ValidarMinutos(minutos);
            if (!PodeDebitar())
            {
                throw new ArgumentException(
                    $"Não há creditos suficientes para debitar {minutos} minutos.\n" +
                    $"Solcitado: {MinutosParaDebitos(minutos):F2}\n" +
                    $"Saldo....: {_creditos:F2}\n" +
                    $"Limite...: {LimiteCreditos:F2}"

                );
            }
            _creditos -= MinutosParaDebitos(minutos);
            return _creditos;
        }

        /// <summary>
        /// Verifica se a carteira possui limite para debitar.
        /// </summary>
        /// <returns> Bool informado se possui créditos suficientes para
        /// debitar.</returns>
        /// <remarks>Cada associado possui um LimiteCreditos que podem ser
        /// utilizados até que seja necessário prestar algum seviço para
        /// solicitar novas demandas.</remarks>
        public bool PodeDebitar()
        {
            return _creditos > LimiteCreditos;
        }

        /// <summary>
        /// Converte minutos a para créditos a serem debitados.
        /// </summary>
        /// <param name="minutos">Quantidade minutos a debitar da carteira.
        /// Deve ser um múltiplo positivo de DebitoBaseMinutos.</param>
        /// <returns>Double representando os créditos correspondentes a serem
        /// debitados. Cada DebitoBaseMinutos correspondem a um
        /// DebitoTaxaConversao créditos.</returns>
        private double MinutosParaDebitos(int minutos)
        {
            return DebitoTaxaConversao * (minutos / DebitoBaseMinutos);
        }

        /// <summary>
        /// Representação da Carteira em formato texto.
        /// </summary>
        /// <returns>String no formato: "Carteira com <CRÉDITOS> créditos e
        /// <MINUTOS> minutos."</returns>
        public override string? ToString()
        {
            return $"Créditos: {Saldo():F2}, Minutos: {_minutosTrabalhados}";
        }
    }
}