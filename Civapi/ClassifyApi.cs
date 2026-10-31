using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

    public record Candidate(string FileName, string Content);
}