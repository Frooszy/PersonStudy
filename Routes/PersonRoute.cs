using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;
using Person.Services;

namespace Person.Routes;

public static class PersonRoute
{
    public static void PersonRotes(WebApplication app)
    {
        var route = app.MapGroup("person");
        
        route.MapPost("", async (PersonRequest req, PersonService service) =>
        {
            var person = await service.CreateAsync(req.name);
            return Results.Ok(person);
        });

        route.MapGet("", async (PersonService service) =>
        {
            var people = await service.GetActiveAsync();
            return Results.Ok(people);
        });

        route.MapPut("{id:guid}", async (Guid id, PersonRequest req, PersonService service) =>
        {
            var person = await service.UpdateNameAsync(id, req.name);
            return person is null ? Results.NotFound() : Results.Ok(person);
        });

        route.MapDelete("{id:guid}", async (Guid id, PersonService service) =>
        {
            var person = await service.SetInactiveAsync(id);
            return person is null ? Results.NotFound() : Results.Ok(person);
        });
    }
}