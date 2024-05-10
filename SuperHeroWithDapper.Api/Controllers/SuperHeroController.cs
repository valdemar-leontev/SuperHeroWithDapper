using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SuperHeroWithDapper.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuperHeroController : ControllerBase
{
    private readonly IConfiguration _config;

    public SuperHeroController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SuperHeroDataModel>>> GetAll()
    {
        using var connection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var superHeroes = await connection
            .QueryAsync<SuperHeroDataModel>("select * from SuperHero");

        return Ok(superHeroes);
    }

    [HttpGet("{heroId:int}")]
    public async Task<ActionResult<SuperHeroDataModel>> GetById(int heroId)
    {
        using var connection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var superHero = await connection
            .QueryFirstAsync<SuperHeroDataModel>(
                "select * from SuperHero where id = @Id",
                new { Id = heroId }
            );

        return Ok(superHero);
    }

    [HttpPost]
    public async Task<ActionResult<SuperHeroDataModel>> Add([FromBody] SuperHeroDataModel superHero)
    {
        using var connection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        await connection
            .ExecuteAsync(
                $@"insert into SuperHero (name, firstName, lastName, place)
                values (
                    @{nameof(SuperHeroDataModel.Name)},
                    @{nameof(SuperHeroDataModel.FirstName)},
                    @{nameof(SuperHeroDataModel.LastName)},
                    @{nameof(SuperHeroDataModel.Place)}
                )",
                superHero
            );

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] SuperHeroDataModel superHero)
    {
        using var connection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        await connection
            .ExecuteAsync(
                $@"update SuperHero set 
                    name = @{nameof(SuperHeroDataModel.Name)}, 
                    firstName = @{nameof(SuperHeroDataModel.FirstName)}, 
                    lastName = @{nameof(SuperHeroDataModel.LastName)}, 
                    place = @{nameof(SuperHeroDataModel.Place)}
                where id = superHero.Id",
                superHero
            );

        return NoContent();
    }

    [HttpDelete("{heroId:int}")]
    public async Task<ActionResult> Delete(int heroId)
    {
        using var connection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        await connection
            .ExecuteAsync("delete from SuperHero where id = @Id", new { Id = heroId });

        return NoContent();
    }
}