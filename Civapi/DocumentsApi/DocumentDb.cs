using Microsoft.EntityFrameworkCore;

public class DocumentDb : DbContext
{
    public DocumentDb(DbContextOptions<DocumentDb> options) : base(options) { }
    public DbSet<Document> Documents => Set<Document>();
}