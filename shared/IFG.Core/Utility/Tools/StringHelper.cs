namespace Core.Utility.Tools;

public static class StringHelper
{
    public static string Repeat(string _str, int length)
    {
        return string.Concat(Enumerable.Repeat("0", length - _str.Length)) + _str;
    }

    public static string GenerateTreeNodePath(string maxCode, string parentCode, int codeLength)
    {
        if (!string.IsNullOrWhiteSpace(maxCode))
            maxCode = (Convert.ToInt32(maxCode.Substring(maxCode.Length - codeLength)) + 1).ToString();
        else maxCode = "1";
        maxCode = parentCode + StringHelper.Repeat(maxCode, codeLength);
        return maxCode;
    }

    public static string RemoveLastCharacterIfExists(this string expression, char compariosnCharacter)
    {
        if (!expression.Last().Equals(compariosnCharacter))
            return expression;

        return expression.Substring(0, expression.Length - 1);
    }
}
