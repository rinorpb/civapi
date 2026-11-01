using Microsoft.AspNetCore.Http.HttpResults;

namespace Civapi;

public static class ClassifyApi
{
    public static void ConfigureClassifyApi(this IEndpointRouteBuilder app)
    {
        var documentsApi = app.MapGroup("/classify");

        documentsApi.MapPost("/", Results<Ok, BadRequest> (Candidate candidate) =>
        {
            if (candidate.FileName.EndsWith(".pdf")) return TypedResults.Ok();
            return TypedResults.BadRequest();
        });
    }
}