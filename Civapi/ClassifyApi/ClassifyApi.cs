using System.Text.RegularExpressions;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Civapi;

public static class ClassifyApi
{
    public static void ConfigureClassifyApi(this IEndpointRouteBuilder app)
    {
        var documentsApi = app.MapGroup("/classify");

        documentsApi.MapPost("/", Results<Ok<ClassifyResult>, BadRequest> (Candidate candidate) =>
        {
            if (!ValidateFiletype(candidate.FileName, acceptedFileTypes)) return TypedResults.BadRequest();
            if (candidate.Content.Length > 200) return TypedResults.BadRequest();
            return TypedResults.Ok(new ClassifyResult("internal"));
        });
    }

    struct ClassifyResult(string result)
    {
        public string result;
    }

    static string Classify(string content)
    {
        //personal information check
        //email
        Regex emailPattern = new Regex("");
        //national registry
        Regex nationalRegistryPattern = new Regex("");
        
        //sensitive information check
        
        return "internal";
    }
    
    static string[] acceptedFileTypes = { "pdf", "docx", "odt", "txt" };

    static bool ValidateFiletype(string fileName, string[] acceptedExtensions)
    {
        string extension = fileName.Split('.').Last();
        return acceptedExtensions.Contains(extension);
    }
}