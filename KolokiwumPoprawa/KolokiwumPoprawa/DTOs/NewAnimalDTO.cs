namespace KolokiwumPoprawa.DTOs;

public class NewAnimalDTO
{
    public string Name { get; set; }
    public int AnimalClassId { get; set; }
    public DateTime AdmissionDate { get; set; }
    public int OwnerId { get; set; }
    
    public IEnumerable<ProcedureWithDate> Procedures { get; set; } = new List<ProcedureWithDate>();
}

public class ProcedureWithDate
{
    public int ProcedureId { get; set; }
    public DateTime Date { get; set; }
}