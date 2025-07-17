namespace FastFood.WebAPI.RESTModel
{
    public class NarudzbaJeloREST
    {
        public int? JeloId { get; set; }
        public int? NarudzbaId { get; set; }
        public int? Kolicina { get; set; }
        public string Napomena { get; set; }
        public decimal Cijena { get; set; }
    }
}
