namespace PayOn.Infrastructure.Extensions
{
    public static class IntegerExtensions
    {
        public static string ToDecimalString(this int value)
        {
            return ((decimal) value / 100)
                .ToString("N2");
        }
    }
}
