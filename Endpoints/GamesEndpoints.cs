using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    // Extension of the WebApplication HTTP Class.
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        /* 
            -------------------------
            Request handling pipeline
            -------------------------
        */

        // RouteGroupBuilder - all of them start now with /games 

        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
            .Include(game => game.Genre)
            .Select(game => game.ToGameSummaryDto())
            .AsNoTracking()
        );

        // GET /games/{id} 
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        /* 
            The post method adds the parameter validation from the Minimal API lib
        */
        {

            Game game = newGame.ToEntity();
            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);

            // If not found, FindIndex returns -1
            if (existingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();
            // Status code 204
            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            dbContext.Games.Where(game => game.Id == id).ExecuteDelete();
            // Status code 204
            return Results.NoContent();
        });

        return group;
    }

}
