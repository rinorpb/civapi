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
            return TypedResults.Ok(Classify(candidate.Content));
        });
    }

    static ClassifyResult Classify(string content)
    {
        //sensitive information check
        
        //personal information check
        //email
        Regex emailPattern = new Regex("""
                                       /"*[!#$%&'*+\-/=?^_`{|}~\.\w]+"*@\[*[\w\-\.:]+\]*/
                                       """); // "*[!#$%&'*+\-/=?^_`{|}~\.\w]+"*@\[*[\w\-\.:]+\]*
        //national registry
        Regex nationalRegistryPattern = new Regex(@"\d\d.\d\d.\d\d-\d\d\d.\d\d"); // \d\d.\d\d.\d\d-\d\d\d.\d\d
        
        List<Regex> patterns =
        [
            //emailPattern,
            nationalRegistryPattern
        ];

        if (patterns.Any(regex => regex.IsMatch(content))) return new ClassifyResult("personal");
        
        return new ClassifyResult("unrestricted");
    }
    
    static string[] acceptedFileTypes = { "pdf", "docx", "odt", "txt" };

    static bool ValidateFiletype(string fileName, string[] acceptedExtensions)
    {
        string extension = fileName.Split('.').Last();
        return acceptedExtensions.Contains(extension);
    }
}