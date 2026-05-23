namespace VoguMap.Application.Helpers
{
    public static class StringEscaper
    {
        public static string EscapeLikeString(string value)
        {
            return value.Replace("\\", "\\\\")
                        .Replace("%", "\\%")
                        .Replace("_", "\\_");
        }
    }
}