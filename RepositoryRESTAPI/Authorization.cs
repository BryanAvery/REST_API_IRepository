namespace RepositoryRESTAPI
{
    public enum Authorization
    {
        NoAuth,
        BearerToken,
        BasicAuth,
        DigestAuth,
        OAuth10,
        OAuth20,
        HawkAutherntication,
        AWSSignature,
        NTLMAuthentication
    }
}
