using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;
using Person.Services;
using Xunit;

namespace Person.Tests.Services;

public class PersonServiceTests
{
    private readonly PersonService _service;
    private readonly PersonContext _context;
    
    private static Faker Faker => new();
    
    public PersonServiceTests()
    {
        var options = new DbContextOptionsBuilder<PersonContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new PersonContext(options);
        _service = new PersonService(_context);
    }

    [Fact]
    public async Task CreateAsync_Should_AddPerson()
    {
        // Arrange
        var name = Faker.Name.FullName();
        
        //Act
        var person = await _service.CreateAsync(name);
        
        //Assert
        person.Name.Should().Be(name);
        person.IsActive.Should().BeTrue();

        var dbPerson = await _context.People.FindAsync(person.Id);
        dbPerson.Should().NotBeNull();
        dbPerson.Name.Should().Be(name);
    }

    [Fact]
    public async Task UpdateNameAsync_Should_UpdateName_WhenPersonExists()
    {
        // Arrange
        var initialName = Faker.Name.FullName();
        var person = new PersonModel(initialName);
        _context.People.Add(person);
        await _context.SaveChangesAsync();
        
        // Act
        var newName = Faker.Name.FullName();
        var updatedPerson = await _service.UpdateNameAsync(person.Id, newName);

        // Assert
        updatedPerson.Should().NotBeNull();
        updatedPerson.Name.Should().Be(newName);
        updatedPerson.IsActive.Should().BeTrue();

        var dbPerson = await _context.People.FindAsync(person.Id);
        dbPerson.Name.Should().Be(newName);
        dbPerson.IsActive.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdateNameAsync_Should_ReturnNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _service.UpdateNameAsync(nonExistentId, "NewName");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateNameAsync_Should_ReturnNull_WhenPersonIsInactive()
    {
        // Arrange
        var inactivePerson = new PersonModel(Faker.Name.FullName());
        inactivePerson.SetInactive();
        _context.People.Add(inactivePerson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.UpdateNameAsync(inactivePerson.Id, "NewName");

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task SetInactiveAsync_Should_SetPersonInactive_WhenPersonExists()
    {
        // Arrange
        var person = new PersonModel(Faker.Name.FullName());
        _context.People.Add(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.SetInactiveAsync(person.Id);

        // Assert
        result.Should().NotBeNull();
        result.IsActive.Should().BeFalse();

        var dbPerson = await _context.People.FindAsync(person.Id);
        dbPerson.IsActive.Should().BeFalse();
    }
    
    [Fact]
    public async Task SetInactiveAsync_Should_ReturnNull_WhenPersonNotFound()
    {
        // Act
        var result = await _service.SetInactiveAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetActiveAsync_Should_ReturnOnlyActivePeople()
    {
        // Arrange
        var activePerson = new PersonModel(Faker.Name.FullName());
        var inactivePerson = new PersonModel(Faker.Name.FullName());
        inactivePerson.SetInactive();

        _context.People.AddRange(activePerson, inactivePerson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetActiveAsync();

        // Assert
        result.Should().ContainSingle(p => p.Id == activePerson.Id);
        result.Should().NotContain(p => p.Id == inactivePerson.Id);
    }
}