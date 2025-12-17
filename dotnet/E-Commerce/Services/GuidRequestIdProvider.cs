namespace E_Commerce.Services
{
    public class GuidRequestIdProvider : IRequestIdProvider
    {
        public GuidRequestIdProvider()
        {
            RequestId = Guid.NewGuid().ToString("N");
        }

        public string RequestId { get; }
    }
}
