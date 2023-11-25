using Microsoft.EntityFrameworkCore;

namespace ASP_SPD_111.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ASP_SPD_111");
        }
    }
}
/* Д.З. Повторити ADO/EntityFramework (EF), LINQ-TO-EF
 * - Описати сутність (Entity) для даних від форми, створеної на
 * попередньому ДЗ. Додати поля Id та Moment (DateTime) - момент
 * надходження даних
 * - Додати до контексту даних відповідний DbSet
 * - Створити міграцію
 * - Застосувати міграцію
 * ** У випадку успішної валідації даних форми додати їх до 
 *    таблиці БД засобами ASP/EF
 */
