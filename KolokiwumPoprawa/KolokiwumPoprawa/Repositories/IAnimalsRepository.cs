using KolokiwumPoprawa.DTOs;

namespace KolokiwumPoprawa.Repositories;

public interface IAnimalsRepository
{
    Task<bool> DoesAnimalExist(int id);
    Task<AnimalDTO> GetAnimal(int id);
    
    Task<int> AddAnimal(NewAnimalDTO animal);
    
    Task<bool> DoesOwnerExist(int id);

    Task<bool> DoesProcedureExist(int id);

    Task<bool> DoesAnimalClassExist(int id);
    
    Task AddProcedureAnimal(int animalId, ProcedureWithDate procedure);
}