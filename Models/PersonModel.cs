namespace Person.Models;

public class PersonModel
{
    public PersonModel(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        IsActive = true;
    }
    
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public bool IsActive { get; set; }
    
    public void ChangeName(string name)
    {
        Name = name;
    }

    public void SetInactive()
    {
        IsActive = false;
    }
}