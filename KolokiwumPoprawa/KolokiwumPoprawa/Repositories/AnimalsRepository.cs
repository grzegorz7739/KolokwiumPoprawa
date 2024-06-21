using KolokiwumPoprawa.DTOs;
using Microsoft.Data.SqlClient;

namespace KolokiwumPoprawa.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private readonly IConfiguration _configuration;

    public AnimalsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesAnimalExist(int id)
    {
        var query = "SELECT 1 FROM Animal Where ID = @ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<AnimalDTO> GetAnimal(int id)
    {
        var query = @"Select Animal.ID AS AnimalID,
							Animal.Name AS AnimalName,
							AnimalClassID,
							AdmissionDate,
							Owner.ID as OwnerID,
							FirstName,
							LastName
                        FROM Animal 
                        JOIN Owner on Owner.ID = Animal.OwnerID
                        WHERE Animal.ID = @ID";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        
        var animalIdOrdinal = reader.GetOrdinal("AnimalID");
        var animalNameOrdinal = reader.GetOrdinal("AnimalName");
        var animalTypeOrdinal = reader.GetOrdinal("AnimalClassID");
        var admissionDateOrdinal = reader.GetOrdinal("AdmissionDate");
        var ownerIdOrdinal = reader.GetOrdinal("OwnerID");
        var firstNameOrdinal = reader.GetOrdinal("FirstName");
        var lastNameOrdinal = reader.GetOrdinal("LastName");

        AnimalDTO animalDto = null;
        
        while (await reader.ReadAsync())
        {
                animalDto = new AnimalDTO()
                {
                    Id = reader.GetInt32(animalIdOrdinal),
                    Name = reader.GetString(animalNameOrdinal),
                    AnimalClassId = reader.GetInt32(animalTypeOrdinal),
                    AdmissionDate = reader.GetDateTime(admissionDateOrdinal),
                    Owner = new OwnerDTO()
                    {
                        Id = reader.GetInt32(ownerIdOrdinal),
                        FirstName = reader.GetString(firstNameOrdinal),
                        LastName = reader.GetString(lastNameOrdinal),
                    }
                };
        }

        if (animalDto is null) throw new Exception();
        
        return animalDto;
        
    }
    
    public async Task<bool> DoesProcedureExist(int id)
    {
        var query = "SELECT 1 FROM [Procedure] WHERE ID = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    public async Task<bool> DoesAnimalClassExist(int id)
    {
        var query = "SELECT 1 FROM Animal_Class WHERE ID = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    
    
    public async Task<bool> DoesOwnerExist(int id)
    {
        var query = "SELECT 1 FROM Owner WHERE ID = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    
    public async Task AddProcedureAnimal(int animalId, ProcedureWithDate procedure)
    {
        var query = $"INSERT INTO Procedure_Animal VALUES(@ProcedureID, @AnimalID, @Date)";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ProcedureID", procedure.ProcedureId);
        command.Parameters.AddWithValue("@AnimalID", animalId);
        command.Parameters.AddWithValue("@Date", procedure.Date);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
    
    public async Task<int> AddAnimal(NewAnimalDTO animal)
    {
        var insert = @"INSERT INTO Animal VALUES(@Name, @AdmissionDate, @OwnerId, @AnimalClassId);
					   SELECT @@IDENTITY AS ID;";
	    
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
	    
        command.Connection = connection;
        command.CommandText = insert;
	    
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@AnimalClassId", animal.AnimalClassId);
        command.Parameters.AddWithValue("@AdmissionDate", animal.AdmissionDate);
        command.Parameters.AddWithValue("@OwnerId", animal.OwnerId);
	    
        await connection.OpenAsync();
	    
        var id = await command.ExecuteScalarAsync();

        if (id is null) throw new Exception();
	    
        return Convert.ToInt32(id);
    }
}