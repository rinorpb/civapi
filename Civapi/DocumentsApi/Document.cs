public class Document
{
    public long Id { get; set; }
    public string FileName { get; set; }
    public string? Path { get; set; }
    public DateOnly? UploadDate { get; set; }
    public int? DaysToSave {get; set; }
    public string AccessLevel { get; set; }
}