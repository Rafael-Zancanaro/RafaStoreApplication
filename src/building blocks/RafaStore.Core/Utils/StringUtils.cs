namespace RafaStore.Core.Utils;

public static class StringUtils
{
    public static string ApenasNumeros(this string str, string input)
        => new([.. str.Where(char.IsDigit)]);
}