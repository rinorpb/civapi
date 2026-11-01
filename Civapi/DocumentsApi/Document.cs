public class Document
{
    public long Id { get; set; }
    string? Name { get; set; }
    string Path { get; set; }
    private DateOnly UploadDate { get; set; }
    private DateOnly? TerminationDate {get; set; }
    private string AccessLevel { get; set; }
}