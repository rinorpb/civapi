using Xunit;
using Civapi;

namespace Civapi.Tests;

public class CivapiClassifyApi_ClassifyShould
{
    [Fact]
    public void ClassifyApi_InputIsUnrestricted_ReturnUnrestricted()
    {
        Candidate input = new Candidate();
        input.FileName = "public.pdf";
        input.Content = "This test document should be unrestricted.";
        ClassifyResult result = ClassifyApi.Classify(input);
        Assert.Equal("unrestricted", result.Result);
    }

    [Fact]
    public void ClassifyApi_InputContainsEmail_ReturnPersonal()
    {
        Candidate input = new Candidate();
        input.FileName = "personal.pdf";
        input.Content = "This document does indeed contain jane.doe@example.com, which follows the email address standard.";
        ClassifyResult result = ClassifyApi.Classify(input);
        Assert.Equal("personal", result.Result);
    }
    
    [Fact]
    public void ClassifyApi_InputContainsNationalRegistry_ReturnPersonal()
    {
        Candidate input = new Candidate();
        input.FileName = "personal.pdf";
        input.Content = "This document does indeed contain 11.11.11-111.11, which is the shape of a Belgian national registry number.";
        ClassifyResult result = ClassifyApi.Classify(input);
        Assert.Equal("personal", result.Result);
    }
    
    [Fact]
    public void ClassifyApi_InputIsSensitive_ReturnSensitive()
    {
        Candidate input = new Candidate();
        input.FileName = "sensitive.pdf";
        input.Content = "Bij deze dus het medisch verslag van Ann Doe:";
        ClassifyResult result = ClassifyApi.Classify(input);
        Assert.Equal("sensitive", result.Result);
    }
}
