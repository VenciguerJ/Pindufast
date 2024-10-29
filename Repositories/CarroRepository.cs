using System.Data;
using Dapper;
using PinduFast.Models;

namespace PinduFast.Repositories;
public class CarroRepository : IRepository<Carro>
{
    private readonly IDbConnection _dbConnection;

    public CarroRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Carro>> GetAll()
    {
        return await _dbConnection.QueryAsync<Carro>("SELECT * FROM Carro");
    }

    public async Task<Carro?> GetById(int id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Carro>("SELECT * FROM Carro WHERE Id = @Id", new { Id = id });
    }
    public async Task<IEnumerable<Carro>> BuscaComplete(string? searchString, int? IsActive)
    {
        var query = "SELECT * FROM Carro WHERE Nome LIKE @Search AND Ativo = @IsActive";
        var parametros = new { Search = $"%{searchString}%", IsActive };

        // Use QueryAsync to return a list of Carro objects asynchronously
        return await _dbConnection.QueryAsync<Carro>(query, parametros);
    }
    
    public async Task Add(Carro entity)
    
    {
        var query = "INSERT INTO Carro (Placa, Nome, Portas, Preco, Imagem, DataPublicacao, Ativo) VALUES (@Placa, @Nome, @Portas, @Preco, @Imagem, @DataPublicacao, @Ativo)";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    public async Task Update(Carro entity)
    {
        var query = "UPDATE Carro SET Placa = @Placa, Nome = @Nome, Portas = @Portas, Preco = @Preco, Imagem = @Imagem, DataPublicacao = @DataPublicacao, Ativo = @Ativo WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    public async Task Delete(int id)
    {
        await _dbConnection.ExecuteAsync("DELETE FROM Carro WHERE Id = @Id", new { Id = id });
    }
}
