using JWT_AuthAndRefrest.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWT_AuthAndRefrest.Models.Entities.Seeding
{
    public class SeedingInicial
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var user0 = new User() { Id = 1, NameUser = "Admin", Key = "123" };

            modelBuilder.Entity<User>().HasData(user0);
        }


    }
}
