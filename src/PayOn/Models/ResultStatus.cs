using Newtonsoft.Json;

namespace PayOn.Models
{
    public enum ResultStatus
    {
        Approved,
        ManualReview,
        Pending,
        Transient,
        Declined,
        Unknown
    }
}
