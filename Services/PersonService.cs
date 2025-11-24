using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

namespace Person.Services;

public class PersonService
{
    private readonly PersonContext _context;

    public PersonService(PersonContext context)
    {
        _context = context;
    }

    public async Task<PersonModel> CreateAsync(string name)
    {
        var person = new PersonModel(name);

        await _context.AddAsync(person);
        await _context.SaveChangesAsync();

        return person;
    }
    
    public async Task<List<PersonModel>> GetActiveAsync()
    {
        return await _context.People
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<PersonModel?> UpdateNameAsync(Guid id, string name)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        if (person == null) return null;

        person.ChangeName(name);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<PersonModel?> SetInactiveAsync(Guid id)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null) return null;

        person.SetInactive();
        await _context.SaveChangesAsync();
        return person;
    }
}