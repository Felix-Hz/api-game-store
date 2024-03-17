using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;
/*
 DBContext is a combination fo unit of work patterns and repository.
 Object that creates a session with the DB.
*/
public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    // Objects that need to be mapped.
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    // Modify what runs when applying migrations.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighting" },
            new { Id = 2, Name = "Sports" },
            new { Id = 3, Name = "Roleplaying" },
            new { Id = 4, Name = "Family" }
        );
    }
}
