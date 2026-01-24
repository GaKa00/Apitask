using Apitask.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Apitask.Data
{
    public class APIdbContext : DbContext
    {
        public APIdbContext(DbContextOptions<APIdbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Interest> Interests { get; set; } = null!;
        public DbSet<PersonInterest> PersonInterests { get; set; } = null!;
        public DbSet<Link> Links { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PersonInterest>()
                .HasKey(pi => new { pi.PersonId, pi.InterestId });

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, FirstName = "Alice", LastName = "Cooper", PhoneNumber = "0701111111" },
                new Person { Id = 2, FirstName = "Bob", LastName = "Marley", PhoneNumber = "0702222222" },
                new Person { Id = 3, FirstName = "Charlie", LastName = "Puth", PhoneNumber = "0703333333" },
                new Person { Id = 4, FirstName = "David", LastName = "Guetta", PhoneNumber = "0704444444" }
            );

            modelBuilder.Entity<Interest>().HasData(
                new Interest { Id = 1, Title = "Music", Description = "Having some rythmic tunes in your ears man!" },
                new Interest { Id = 2, Title = "Sports", Description = "They dunno how to play ball like me!" },
                new Interest { Id = 3, Title = "Travel", Description = "We 10 000 miles above the clouds man!" },
                new Interest { Id = 4, Title = "Reading", Description = " Reading books." }
            );

            modelBuilder.Entity<PersonInterest>().HasData(
                new PersonInterest { PersonId = 1, InterestId = 1 },
                new PersonInterest { PersonId = 1, InterestId = 3 },
                new PersonInterest { PersonId = 2, InterestId = 1 },
                new PersonInterest { PersonId = 2, InterestId = 2 },
                new PersonInterest { PersonId = 3, InterestId = 1 },
                new PersonInterest { PersonId = 3, InterestId = 4 },
                new PersonInterest { PersonId = 4, InterestId = 2 }
            );

            modelBuilder.Entity<Link>().HasData(
                new Link { Id = 1, Url = "https://www.music.com", PersonId = 1, InterestId = 1 },
                new Link { Id = 2, Url = "https://www.rastaban.com", PersonId = 2, InterestId = 1 },
                new Link { Id = 3, Url = "https://www.charlieputh.com", PersonId = 3, InterestId = 1 },
                new Link { Id = 4, Url = "https://www.sports.com", PersonId = 2, InterestId = 2 },
                new Link { Id = 5, Url = "https://www.topbadminton.com", PersonId = 4, InterestId = 2 },
                new Link { Id = 6, Url = "https://www.flyhigh.com", PersonId = 1, InterestId = 3 },
                new Link { Id = 7, Url = "https://www.reading.com", PersonId = 3, InterestId = 4 }
            );
        }

    }
}
