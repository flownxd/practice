namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        string low = input.ToLower();

        string cl = "";
        foreach (char c in low)
        {
            if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
            {
                cl += c;
            }
        }
        char[] ch = cl.ToCharArray();
        Array.Reverse(ch);
        string reverse = new string(ch);

        return cl == reverse;
    }
}