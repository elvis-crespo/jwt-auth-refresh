namespace JWT_AuthAndRefrest.Models.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string RefreshTokenS { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? EsActivo  { get; set; }

        //Relacion
        public int? IdUser { get; set; }
        public User User { get; set; }
    }
}
