using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_AuthAndRefrest.Models.Entities
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string NameUser { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Key { get; set; } = null!;

        //public HashSet<Alumno> Alumnos { get; set; } = new HashSet<Alumno>();
        //Navegacion
        public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
    }
}
