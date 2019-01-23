using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Tuneage.WebApi.Test;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration
{
    public class IntegrationTestFixture : IDisposable
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected SetCookieHeaderValue AntiforgeryCookie;
        protected SetCookieHeaderValue AuthenticationCookie;
        protected string AntiforgeryToken;
        protected static Regex AntiforgeryFormFieldRegex = 
            new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        public IntegrationTestFixture()
        {
            Server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseEnvironment("Development"));
            Client = Server.CreateClient();
        }

        protected async Task<string> EnsureAntiforgeryToken()
        {
            if (AntiforgeryToken != null) return AntiforgeryToken;

            var response = await Client.GetAsync("/labels/create");
            response.EnsureSuccessStatusCode();
            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                AntiforgeryCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(
                        c => c.Name.StartsWith(".AspNetCore.Antiforgery.", StringComparison.InvariantCultureIgnoreCase)
                    );
            }
            Assert.NotNull(AntiforgeryCookie);
            Client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(AntiforgeryCookie.Name, AntiforgeryCookie.Value).ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();
            var match = AntiforgeryFormFieldRegex.Match(responseHtml);
            AntiforgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;
            Assert.NotNull(AntiforgeryToken);

            return AntiforgeryToken;
        }

        protected async Task<Dictionary<string, string>> EnsureAntiforgeryTokenOnForm(Dictionary<string, string> formData = null)
        {
            if (formData == null) formData = new Dictionary<string, string>();

            formData.Add("__RequestVerificationToken", await EnsureAntiforgeryToken());
            return formData;
        }

        public async Task EnsureAntiforgeryTokenHeader()
        {
            Client.DefaultRequestHeaders.Add("XSRF-TOKEN", await EnsureAntiforgeryToken());
        }

        public async Task EnsureAuthenticationCookie()
        {
            if (AuthenticationCookie != null) return;
            var formData = await EnsureAntiforgeryTokenOnForm(new Dictionary<string, string>()
            {
                { "Email", "SomePredefinedUserEmail" },
                { "Password ", "SomePredefinedUserPassword" }
            });

            var response = await Client.PostAsync("/account/login (which doesn't currently exist)", new FormUrlEncodedContent(formData));
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                AuthenticationCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(
                        c => c.Name.StartsWith(".AspNetCore.Identity.", StringComparison.InvariantCultureIgnoreCase)
                    );
            }
            Assert.NotNull(AuthenticationCookie);
            Client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(AuthenticationCookie.Name, AuthenticationCookie.Value).ToString());

            // The current pair of anti-forgery cookie+token would no longer be valid, since the tokens are generated
            // based on the authenticated user. We need a new token after authentication. The cookie can remain.
            AntiforgeryToken = null;
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
