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
        protected SetCookieHeaderValue AntiForgeryCookie;
        protected SetCookieHeaderValue AuthenticationCookie;
        protected string AntiForgeryToken;
        protected static Regex AntiForgeryFormFieldRegex = 
            new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        public IntegrationTestFixture()
        {
            Server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseEnvironment("Development"));
            Client = Server.CreateClient();
        }

        protected async Task<string> EnsureAntiForgeryToken()
        {
            if (AntiForgeryToken != null) return AntiForgeryToken;

            var response = await Client.GetAsync("/labels/create");
            response.EnsureSuccessStatusCode();
            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                AntiForgeryCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(
                        c => c.Name.StartsWith(".AspNetCore.AntiForgery.", StringComparison.InvariantCultureIgnoreCase)
                    );
            }
            Assert.NotNull(AntiForgeryCookie);
            Client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(AntiForgeryCookie.Name, AntiForgeryCookie.Value).ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();
            var match = AntiForgeryFormFieldRegex.Match(responseHtml);
            AntiForgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;
            Assert.NotNull(AntiForgeryToken);

            return AntiForgeryToken;
        }

        protected async Task<Dictionary<string, string>> EnsureAntiForgeryTokenOnForm(Dictionary<string, string> formData = null)
        {
            if (formData == null) formData = new Dictionary<string, string>();

            formData.Add("__RequestVerificationToken", await EnsureAntiForgeryToken());
            return formData;
        }

        public async Task EnsureAntiForgeryTokenHeader()
        {
            Client.DefaultRequestHeaders.Add("XSRF-TOKEN", await EnsureAntiForgeryToken());
        }

        public async Task EnsureAuthenticationCookie()
        {
            if (AuthenticationCookie != null) return;
            var formData = await EnsureAntiForgeryTokenOnForm(new Dictionary<string, string>()
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
            AntiForgeryToken = null;
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
