namespace SkiaHelper.Extensions;

public static class StringExtensions
{
    public static bool IsNullEmptyOrWhiteSpace(this string str) =>
        string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
}
