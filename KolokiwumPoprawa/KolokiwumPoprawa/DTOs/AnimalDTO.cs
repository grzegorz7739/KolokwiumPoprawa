namespace KolokiwumPoprawa.DTOs;

public class AnimalDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AnimalClassId { get; set; }
    public DateTime AdmissionDate { get; set; }
    public OwnerDTO Owner { get; set; }
}

public class OwnerDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}