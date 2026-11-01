using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Civapi;

public static class DocumentsApi
{
    public static void ConfigureDocumentsApi(this IEndpointRouteBuilder app)
    {
        /*Document[] sampleDocs =
        [
            new(1, "Concert Flyer", "public/concertflyer.pdf", DateOnly.FromDateTime(DateTime.Now), null, "public"),
            new(2, "ERD", "internal/erd.pdf", DateOnly.FromDateTime(DateTime.Now).AddDays(-89), null, "internal"),
            new(3, "John Doe Cover Letter", "personal/johndoe.docx", DateOnly.FromDateTime(DateTime.Now).AddDays(-12), DateOnly.FromDateTime(DateTime.Now).AddMonths(1), "personal"),
            new(4, "Ann Doe blood test", "sensitive/anndoe.pdf", DateOnly.FromDateTime(DateTime.Now).AddDays(-4), DateOnly.FromDateTime(DateTime.Now).AddMonths(1), "sensitive"),
        ];*/
        
        var documentsApi = app.MapGroup("/documents");
        
        documentsApi.MapGet("/", async (DocumentDb db) => await db.Documents.ToListAsync())
            .WithName("GetDocuments");

        documentsApi.MapGet("/{id}", async (long id, DocumentDb db) => 
                await db.Documents.FindAsync(id) 
                    is { } todo 
                        ? Results.Ok(todo) 
                        : Results.NotFound())
            .WithName("GetDocumentById");

        documentsApi.MapPost("/", async (Document document, DocumentDb db) =>
        {
            document.DaysToSave ??= 0; // if null, set 0 (save indefinitely)
            if (document.DaysToSave < 0) return Results.BadRequest();
            
            document.UploadDate = DateOnly.FromDateTime(DateTime.Now);
            document.Path = $"{document.AccessLevel}/{document.FileName}";
            
            db.Documents.Add(document);
            //todo: try to save document
            await db.SaveChangesAsync();
            
            return TypedResults.Created($"/documents/{document.Id}", document);
        });
    }
}