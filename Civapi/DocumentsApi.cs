using Microsoft.AspNetCore.Http.HttpResults;

namespace Civapi;

public static class DocumentsApi
{
    public static void ConfigureDocumentsApi(this IEndpointRouteBuilder app)
    {
        Document[] sampleDocs =
        [
            new(1, "Concert Flyer", "public/concertflyer.pdf", DateOnly.FromDateTime(DateTime.Now), null, "public"),
            new(2, "ERD", "internal/erd.pdf", DateOnly.FromDateTime(DateTime.Now).AddDays(-89), null, "internal"),
            new(3, "John Doe Cover Letter", "personal/johndoe.docx", DateOnly.FromDateTime(DateTime.Now).AddDays(-12), DateOnly.FromDateTime(DateTime.Now).AddMonths(1), "personal"),
            new(4, "Ann Doe blood test", "sensitive/anndoe.pdf", DateOnly.FromDateTime(DateTime.Now).AddDays(-4), DateOnly.FromDateTime(DateTime.Now).AddMonths(1), "sensitive"),
        ];
        
        var documentsApi = app.MapGroup("/documents");
        documentsApi.MapGet("/", () => sampleDocs)
            .WithName("GetDocuments");

        documentsApi.MapGet("/{id}", Results<Ok<Document>, NotFound> (int id) =>
                sampleDocs.FirstOrDefault(a => a.Id == id) is { } doc
                    ? TypedResults.Ok(doc)
                    : TypedResults.NotFound())
            .WithName("GetDocumentById");
    }
    
    public record Document(int Id, string? Title, string Path, DateOnly UploadDate, DateOnly? TerminationDate = null, string AccessLevel = "internal");
}