namespace PayOn.Tests
{
    public class AcsRedirectResponse
    {
        public string Id { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Id);
            }
        }
    }
}