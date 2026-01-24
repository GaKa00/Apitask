
using Apitask.Data;
using Apitask.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Apitask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddAuthorization();


            builder.Services.AddDbContext<APIdbContext>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var context = new APIdbContext(
              builder.Services.BuildServiceProvider().GetRequiredService<DbContextOptions<APIdbContext>>());

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.MapOpenApi();
                app.MapScalarApiReference();

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/people", async (APIdbContext db) =>
            {
                var people = await db.People
                    .Include(p => p.PersonInterests)
                        .ThenInclude(pi => pi.Interest)
                    .Include(p => p.InterestLinks)
                        .ThenInclude(l => l.Interest)
                    .Select(p => new
                    {
                        p.Id,
                        p.FirstName,
                        p.LastName,
                        p.PhoneNumber,
                        Interests = p.PersonInterests.Select(pi => new
                        {
                            pi.InterestId,
                            pi.Interest.Title,
                            pi.Interest.Description
                        }),
                        Links = p.InterestLinks.Select(l => new
                        {
                            l.Id,
                            l.Url,
                            l.InterestId,
                            InterestTitle = l.Interest.Title
                        })
                    })
                    .ToListAsync();

                return Results.Ok(people);
            });

     
            app.MapGet("/people/{id:int}/interests", async (int id, APIdbContext db) =>
            {
                var person = await db.People
                    .Include(p => p.PersonInterests)
                        .ThenInclude(pi => pi.Interest)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (person == null) return Results.NotFound();

                var interests = person.PersonInterests
                    .Select(pi => new
                    {
                        pi.InterestId,
                        pi.Interest.Title,
                        pi.Interest.Description
                    })
                    .ToList();

                return Results.Ok(interests);
            });

          
            app.MapGet("/people/{id:int}/links", async (int id, APIdbContext db) =>
            {
                var person = await db.People
                    .Include(p => p.InterestLinks)
                        .ThenInclude(l => l.Interest)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                {
                    return Results.NotFound();
                }

                var links = person.InterestLinks
                    .Select(l => new
                    {
                        l.Id,
                        l.Url,
                        l.InterestId,
                        InterestTitle = l.Interest.Title
                    })
                    .ToList();

                return Results.Ok(links);
            });

            app.MapPut("/people/{personId}/interests/{oldInterestId}", async (int personId, int oldInterestId, int newInterestId, APIdbContext db) =>
            {
                var interest = await db.PersonInterests
                    .FirstOrDefaultAsync(pi => pi.PersonId == personId && pi.InterestId == oldInterestId);

                if (interest == null) return Results.NotFound();
                if (!await db.Interests.AnyAsync(i => i.Id == newInterestId)) return Results.BadRequest();

              
                db.PersonInterests.Remove(interest);

               
                var newInterest = new PersonInterest
                {
                    PersonId = personId,
                    InterestId = newInterestId
                };
                await db.PersonInterests.AddAsync(newInterest);

                await db.SaveChangesAsync();

                return Results.Ok(newInterest);
            });



            app.MapPut("/people/{personId}/interests/{interestId}/link", async (int personId, int interestId, string url, APIdbContext db) =>
            {
                var link = await db.Links
                    .FirstOrDefaultAsync(l => l.PersonId == personId && l.InterestId == interestId);

                if (link == null)
                {

                    return Results.NotFound();
                }
       
                link.Url = url.Trim();
                await db.SaveChangesAsync();

                var result = new
                {
                    link.Id,
                    link.Url,
                    link.PersonId,
                    link.InterestId
                };

                return Results.Ok(result);
            });

            app.Run();
        }
    }
}
