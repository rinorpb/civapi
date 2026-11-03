using System.Text.RegularExpressions;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;

namespace Civapi;

public static class ClassifyApi
{
    public static void ConfigureClassifyApi(this IEndpointRouteBuilder app)
    {
        var documentsApi = app.MapGroup("/classify");

        documentsApi.MapPost("/", (Candidate candidate) =>
        {
            if (!ValidateFiletype(candidate.FileName, acceptedFileTypes)) return Results.BadRequest();
            if (candidate.Content.Length > 200) return Results.BadRequest();
            return Results.Ok(Classify(candidate));
        });
    }

    public static ClassifyResult Classify(Candidate input)
    {
        ClassifyResult result = new ClassifyResult("unrestricted");
        
        //personal information check
        //email, basic regex, doesn't match EVERY email according to the standard but most
        Regex emailPattern = new Regex("""
                                       "*[\w!#$%&'*+/=?^_`{|}~.-]+"*@\[*[\w.:-]+\]*
                                       """);
        //national registry
        Regex nationalRegistryPattern = new Regex(@"\d\d.\d\d.\d\d-\d\d\d.\d\d"); // \d\d.\d\d.\d\d-\d\d\d.\d\d
        
        List<Regex> patterns =
        [
            emailPattern,
            nationalRegistryPattern
        ];

        if (patterns.Any(regex => regex.IsMatch(input.Content))) result = new ClassifyResult("personal");
        
        //sensitive information check
        string[] triggerWords =
        {
            "medisch verslag",
            "diagnose",
            "strafrechtelijk",
            "medical report",
            "diagnosis",
            "judicial"
        };

        foreach (var triggerWord in triggerWords)
        {
            if (input.Content.Contains(triggerWord))
            {
                result = new ClassifyResult("sensitive");
                break;
            }
        }
        
        Log.Information("classify: {FileName} as {Result}", input.FileName, result.Result);

        return result;
    }
    
    static string[] acceptedFileTypes = { "pdf", "docx", "odt", "txt" };

    static bool ValidateFiletype(string fileName, string[] acceptedExtensions)
    {
        string extension = fileName.Split('.').Last();
        return acceptedExtensions.Contains(extension);
    }
}