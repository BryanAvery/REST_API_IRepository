using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryRESTAPI
{
    public class Repository : IRepository
    {
        private readonly HttpClient _client;

        public Repository(string uri, Authorization authorization = Authorization.NoAuth, string username = null, SecureString password = null)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(uri);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            switch (authorization)
            {
                case Authorization.BasicAuth:
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{ConvertToUnsecureString(password)}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    break;

                case Authorization.NoAuth:
                    break;
                case Authorization.BearerToken:
                case Authorization.DigestAuth:
                case Authorization.OAuth10:
                case Authorization.OAuth20:
                case Authorization.HawkAutherntication:
                case Authorization.AWSSignature:
                case Authorization.NTLMAuthentication:
                    throw new NotImplementedException();
            }
        }

        public virtual async Task<T> AddAsync<T>(T entity, string requestUri)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(requestUri, entity);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        public virtual async Task<HttpStatusCode> DeleteAsync(string requestUri)
        {
            HttpResponseMessage response = await _client.DeleteAsync(requestUri);
            return response.StatusCode;
        }

        public virtual async Task EditAsync<T>(T entity, string requestUri)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(requestUri, entity);
            response.EnsureSuccessStatusCode();
        }

        public virtual async Task<T> GetAsync<T>(string path)
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<T>();
            }
            return entity;
        }

        protected internal string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                throw new ArgumentNullException("securePassword");
            }

            var unmanagedString = IntPtr.Zero;

            try
            {
                // Copy the contents of the SecureString to unmanaged memory
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);

                // Allocate a managed string and copy the contents of the unmanaged 
                //string data into it
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Free the unmanaged string pointer
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}