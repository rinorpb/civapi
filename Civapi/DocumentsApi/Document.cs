public class Document
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string Path { get; set; }
    public DateOnly UploadDate { get; set; }
    public DateOnly? TerminationDate {get; set; }
    public string AccessLevel { get; set; }
}