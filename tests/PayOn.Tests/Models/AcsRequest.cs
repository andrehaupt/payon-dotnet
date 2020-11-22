namespace PayOn.Tests
{
    public class AcsRequest
    {
        public string FormActionUrl { get; set; }
        public string Md { get; set; }
        public string PaRes { get; set; }
        public string Ndcid { get; set; }
        public string ReturnCode { get; set; }

        public bool IsValid
        {   
            get
            {
                return !string.IsNullOrWhiteSpace(FormActionUrl) &&
                    FormActionUrl.Contains("https") &&
                    !string.IsNullOrWhiteSpace(Md) &&
                    !string.IsNullOrWhiteSpace(PaRes) &&
                    !string.IsNullOrWhiteSpace(ReturnCode);
            }
        }
    }
}