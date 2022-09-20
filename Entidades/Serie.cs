namespace ApiSeries.Entidades
{
    public class Serie
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Categoria> categorias { get; set; }
    }
}
