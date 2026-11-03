using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Civapi;

public static class DocumentsApi
{
    public static void ConfigureDocumentsApi(this IEndpointRouteBuilder app)
    {
        var documentsApi = app.MapGroup("/documents");
        
        documentsApi.MapGet("/", async (DocumentDb db) =>
            {
                List<Document> result = await db.Documents.Where(doc => doc.AccessLevel == AccessLevels.Public).ToListAsync();
                Log.Information("documents: Fetched {Count} public documents", result.Count);
                return result;
            })
            .WithName("GetDocuments");

        documentsApi.MapGet("/{id}", async (HttpContext context, long id, DocumentDb db) =>
            {
                Document doc = await db.Documents.FindAsync(id);
                if (doc == null) return Results.NotFound();
                if (doc.AccessLevel == AccessLevels.Public)
                {
                    Log.Information("documents: Fetched {DocName} ({DocAccess} by id {DocId})", doc.FileName, doc.AccessLevel, doc.Id);
                    return Results.Ok(doc);
                }

                if (context.User.HasClaim(claim => (claim.Type == "scope") && claim.Value.Split(" ").Contains(doc.AccessLevel)))
                {
                    Log.Information("documents: Fetched {DocName} ({DocAccess} by id {DocId}", doc.FileName, doc.AccessLevel, doc.Id);
                    return Results.Ok(doc);
                }
                
                Log.Information("documents: Unauthorized fetch attempt {DocName} ({DocAccess} by id {DocId}", doc.Id);
                return Results.Unauthorized();
            })
            .WithName("GetDocumentById");

        documentsApi.MapPost("/", async (HttpContext context, Document document, DocumentDb db) =>
        {
            document.DaysToSave ??= 0; // if null, set 0 (save indefinitely)
            if (document.DaysToSave < 0) return Results.BadRequest();

            if (!AccessLevels.IsValidAccessLevel(document.AccessLevel)) return Results.BadRequest();
            
            document.UploadDate = DateOnly.FromDateTime(DateTime.Now);
            document.Path = $"{document.AccessLevel}/{document.FileName}";
            
            db.Documents.Add(document);
            //todo: try to save document
            await db.SaveChangesAsync();
            
            Log.Information("documents: Saved {DocName} ({DocAccess}) at {DocPath} with id {DocId} by {User}", document.FileName, document.AccessLevel, document.Path, document.Id, context.User.Identity.Name);
            
            return Results.Created($"/documents/{document.Id}", document);
        }).RequireAuthorization("upload");
    }
}