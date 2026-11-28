using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using SimpleAuctions.DTOs;
using SimpleAuctions.Models;
using SimpleAuctions.Repositories;
namespace SimpleAuctions.Endpoints;
using SimpleAuctions.Rules;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/api/users", async (CreateUserDto data, UserRepository repository,
            IPasswordHasher<User> hasher) =>
        {
            string email = data.Email?.Trim().ToLowerInvariant() ?? "";
            string? error = UserRules.Validate(email, data.Password);
            if (error is not null)
                return Results.BadRequest(new { message = error });
            var user = new User(0, email, "");
            string hash = hasher.HashPassword(user, data.Password);
            try
            {
                user = await repository.Create(email, hash);
                return Results.Created($"/api/users/{user.Id}", new UserResponseDto(user.Id, user.Email));
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return Results.Conflict(new { message = "Email is already registered." });
            }
        });
        // Endast egen användare; inga lösenordshashar i API-svaren.
        app.MapGet("/api/users/{id:int}", async (int id, HttpContext context, UserRepository repository) =>
        {
            int? userId = context.Session.GetInt32("UserId");
            if (userId is null) return Results.Unauthorized();
            if (id != userId) return Results.StatusCode(403);
            var user = await repository.GetById(id);
            return user is null ? Results.NotFound() : Results.Ok(new UserResponseDto(user.Id, user.Email));
        });
    }
}
