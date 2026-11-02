namespace Civapi;

public static class AccessLevels
{
    public const string Public = "public";
    public const string Internal = "internal";
    public const string Personal = "personal";
    public const string Sensitive = "sensitive";

    public static bool IsValidAccessLevel(string input)
    {
        if (input == Public) return true;
        if (input == Internal) return true;
        if (input == Personal) return true;
        if (input == Sensitive) return true;
        return false;
    }
}
