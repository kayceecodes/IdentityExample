using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

public static class AuthEndpoints
{
    public static void ConfigurationAuthEndpoints(this WebApplication app) 
    {
        app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
        {
            await signInManager.SignOutAsync().ConfigureAwait(false);
        })
        .RequireAuthorization()
        .WithOpenApi();

        app.MapPost("/api/login", async (SignInManager<IdentityUser> signInManager, LoginRequest model) =>
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Results.Ok(new { Message = "Login successful." });
            }

            if (result.RequiresTwoFactor)
            {
                return Results.Ok(new { Message = "Two-factor authentication required." });
            }

            if (result.IsLockedOut)
            {
                return Results.Unauthorized();
            }

            return Results.Unauthorized();
        })
        .WithName("Login")
        .WithTags("Authentication");
    }
}