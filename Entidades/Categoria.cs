namespace ApiSeries.Entidades
{
    public class Categoria
    {

        public int Id { get; set; } 

        public string Name { get; set; }

        public  string Genero { get; set; }

        public int SerieId { get; set; }

        public Serie Serie { get; set; }
    }
}
