namespace MetaLog
{
    public static class Extensions
    {
        /// <summary>
        ///     Validate this string if it is not null or whitespace
        /// </summary>
        public static bool IsValid(this string str) => !string.IsNullOrWhiteSpace(str);
    }
}