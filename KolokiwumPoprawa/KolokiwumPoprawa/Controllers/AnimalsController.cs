using System.Transactions;
using KolokiwumPoprawa.DTOs;
using KolokiwumPoprawa.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KolokiwumPoprawa.Controllers;



[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalsRepository _animalsRepository;

    public AnimalsController(IAnimalsRepository animalsRepository)
    {
        _animalsRepository = animalsRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnimal(int id)
    {
        if (!await _animalsRepository.DoesAnimalExist(id))
            return NotFound($"Animal wit given ID {id} does not exists");

        var animal = await _animalsRepository.GetAnimal(id);

        return Ok(animal);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAnimalV2(NewAnimalDTO newAnimal)
    {

        if (!await _animalsRepository.DoesOwnerExist(newAnimal.OwnerId))
            return NotFound($"Owner with given id {newAnimal.OwnerId} not exists");
        if (!await _animalsRepository.DoesAnimalClassExist(newAnimal.OwnerId))
            return NotFound($"AnimalClass with given id {newAnimal.AnimalClassId} not exists");

        
        foreach (var procedure in newAnimal.Procedures)
        {
            if (!await _animalsRepository.DoesProcedureExist(procedure.ProcedureId))
                return NotFound($"Procedure with given id {procedure.ProcedureId} not exists");
        }

        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var id = await _animalsRepository.AddAnimal(new NewAnimalDTO()
            {
                Name = newAnimal.Name,
                AnimalClassId = newAnimal.AnimalClassId,
                AdmissionDate = newAnimal.AdmissionDate,
                OwnerId = newAnimal.OwnerId
            });

            foreach (var procedure in newAnimal.Procedures)
            {
                await _animalsRepository.AddProcedureAnimal(id, procedure);
            }

            scope.Complete();
        }

        return Created(Request.Path.Value ?? "api/animals", newAnimal);
    }
    
    

}