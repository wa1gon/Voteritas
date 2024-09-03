namespace TrustedVoteLibrary.Utils;

public static class UtilExtensions
{
    /// <summary>
    /// Extension method for a string to test if it is null, empty or white space.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? genericEnumerable)
    {
        return ((genericEnumerable == null) || (!genericEnumerable.Any()));
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T>? genericCollection)
    {
        if (genericCollection == null)
        {
            return true;
        }

        return genericCollection.Count < 1;
    }  
}
