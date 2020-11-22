namespace PayOn.Tests
{
    public class AcsResponse
    {
        public string RedirectUrl { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(RedirectUrl) &&
                    RedirectUrl.Contains("https");
            }
        }
    }
}