namespace RafaStore.Core.Utils;

public static class StringUtils
{
    public static string ApenasNumeros(this string str, string input)
        => new string(str.Where(char.IsDigit).ToArray());
}