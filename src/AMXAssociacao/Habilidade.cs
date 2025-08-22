using System.Text.RegularExpressions;
namespace AMXAssociacao
{
    /// <summary>
    /// Classe Habilidade para a AMXAssociacao de moradores. Uma Habildiade
    /// representa de forma textual as capacidades dos moradores, bem como,
    /// os conhecimentos necessários para resolver uma Demanda.
    /// </summary>
    class Habilidade
    {
        /// <summary>
        /// Descrição textual da Habilidade.
        /// </summary>
        private string _descricao;

        /// <summary>
        /// Construtor padrão. Cria uma Habilidade a partir de uma string.
        /// <param name="decricao">Descrição da Habilidade.</param>
        /// </summary>
        public Habilidade(string descricao)
        {
            ValidarDescricao(descricao);
            _descricao = NormalizarDescricao(descricao);
        }

        /// <summary>
        ///  Valida a descrição da Habilidade.
        /// </summary>
        /// <param name="descricao">String com a descrição da Habilidade.</param>
        /// <returns>Nenhum.</returns>
        /// <exception cref="ArgumentException">Se a descrição for uma string nula ou vazia.</exception>
        private void ValidarDescricao(string descricao)
        {
            if (descricao == null || descricao.Trim().Length == 0)
            {
                throw new ArgumentException("Descrição inválida.");
            }
        }

        /// <summary>
        /// Normaliza a descrição com letras maiúsculas, sem acentuação gráfica e espaços no início/fim da string.
        /// </summary>
        /// <param name="descricao">String com a descrição da Habilidade.</param>
        /// <returns>Descrição em letras maiúsculas, sem acentuação gráfica e spaços no início/fim da string.</returns>
        private string NormalizarDescricao(string descricao)
        {
            descricao = descricao.Trim().ToUpper();
            string[] palavras = descricao.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            descricao = String.Join(' ', palavras); // Remove espaços duplicados no meio da string

            Dictionary<char, char> caracteresASubstituir = new() {
                { 'Á', 'A' }, { 'À', 'A' }, { 'Â', 'A' }, { 'Ä', 'A' }, { 'Ã', 'A' },
                { 'É', 'E' }, { 'È', 'E' }, { 'Ê', 'E' }, { 'Ë', 'E' },
                { 'Í', 'I' }, { 'Ì', 'I' }, { 'Î', 'I' }, { 'Ï', 'I' },
                { 'Ó', 'O' }, { 'Ò', 'O' }, { 'Ô', 'O' }, { 'Ö', 'O' }, { 'Õ', 'O' },
                { 'Ú', 'U' }, { 'Ù', 'U' }, { 'Û', 'U' }, { 'Ü', 'U' },
                { 'Ç', 'C' },
                { 'Ñ', 'N' },
                { 'Ý', 'Y' }, { 'Ÿ', 'Y' },
                { ' ', '_' }, { '.', '_' }, { ',', '_' }, { ';', '_' }, { ':', '_' },
                { '-', '_' }, { '+', '_' }, { '=', '_' }, { '\\', '_' }, { '/', '_' },
                { '|', '_' }, { '{', '_' }, { '}', '_' }, { '[', '_' }, { ']', '_' },
                { '\'', '_' }, { '\"', '_' }, { '`', '_' }, { '~', '_' },
                { '!', '_' }, { '@', '_' }, { '#', '_' }, { '$', '_' }, { '%', '_' },
                { '^', '_' }, { '&', '_' }, { '*', '_' }, { '(', '_' }, { ')', '_' },
                { '?', '_' }, { '<', '_' }, { '>', '_' }
            };
            foreach (KeyValuePair<char, char> kv in caracteresASubstituir)
            {
                descricao = descricao.Replace(kv.Key, kv.Value);
            }
            descricao = Regex.Replace(descricao, "_+", "_");
            descricao = descricao.Trim('_');
            return descricao;
        }

        /// <summary>
        /// Representação de Harteira em formato texto.
        /// </summary>
        /// <returns>String com a descrição da Habilidade."</returns>
        public override string? ToString()
        {
            return _descricao;
        }

        /// <summary>
        /// HashCode do objeto obtido a partir do HashCode do atributo _descricao.
        /// </summary>
        /// <returns>HashCode da string de descricao do objeto.</returns>
        public override int GetHashCode()
        {
            return _descricao.GetHashCode();
        }

        /// <summary>
        /// Determina se o objeto especificado é igual ao objeto atual.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado com o objeto atual.</param>
        /// <returns>Verdadeiro se o objeto especificado for igual ao objeto atual, falso caso contrário.</returns>
        public override bool Equals(object? obj)
        {
            Habilidade outraHabilidade = obj as Habilidade;
            if (obj == null || outraHabilidade == null)
            {
                return false;
            }
            return _descricao.Equals(outraHabilidade._descricao);
        }
    }
}