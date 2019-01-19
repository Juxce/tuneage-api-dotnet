using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Tuneage.Data.TestData;
using Xunit;

namespace Tuneage.WebApi.Tests.Integration.Mvc
{
    public class LabelsIntegrationTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public LabelsIntegrationTests()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        [Fact]
        public async void AllGet_ShouldReturnViewWithLabelsData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Index - Tuneage.WebApi</title>", responseString);
            foreach (var label in TestDataGraph.Labels.LabelsRaw)
            {
                Assert.Contains(label.Name, responseString);
                Assert.Contains(label.WebsiteUrl, responseString);
            }
        }

        [Fact]
        public async void DetailsGet_ShouldReturnViewWithExistingLabelData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels/details/" + TestDataGraph.Labels.LabelExisting.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Details - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Labels.LabelExisting.Name, responseString);
            Assert.Contains(TestDataGraph.Labels.LabelExisting.WebsiteUrl, responseString);
        }

        [Fact]
        public async void DetailsGet_ShouldReturnNotFoundResultWhenCalledWithBadData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels/details/" + TestDataGraph.Labels.LabelIdNonExistent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact]
        public async void CreateGet_ShouldReturnViewForCreatingNewLabel()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels/create");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Create - Tuneage.WebApi</title>", responseString);
        }

        [Fact(Skip = "This POST attempt returns a Bad Request. Need to figure out right way to do this.")]
        public async void CreatePost_ShouldReturnViewWithNewlyCreatedLabelData()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                { "Name", TestDataGraph.Labels.LabelNew.Name },
                { "WebsiteUrl", TestDataGraph.Labels.LabelNew.WebsiteUrl }
            };

            // Act
            var response = await _client.PostAsync("/labels/create", new FormUrlEncodedContent(formData));

            // Assert
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/Labels", response.Headers.Location.ToString());
        }

        [Fact]
        public async void EditGet_ShouldReturnViewWithExistingLabelData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels/edit/" + TestDataGraph.Labels.LabelExisting.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Edit - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Labels.LabelExisting.Name, responseString);
            Assert.Contains(TestDataGraph.Labels.LabelExisting.WebsiteUrl, responseString);
        }

        [Fact]
        public async void EditGet_ShouldReturnNotFoundResultWhenCalledWithBadData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels/edit/" + TestDataGraph.Labels.LabelIdNonExistent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(string.Empty, responseString);
        }

        [Fact(Skip = "This POST attempt returns a Bad Request. Need to figure out right way to do this.")]
        public async void EditPost_ShouldReturnViewWithNewlyUpdatedLabelData()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                { "LabelId", TestDataGraph.Labels.LabelUpdated.LabelId.ToString() },
                { "Name", TestDataGraph.Labels.LabelUpdated.Name },
                { "WebsiteUrl", TestDataGraph.Labels.LabelUpdated.WebsiteUrl }
            };

            // Act
            var response = await _client.PostAsync("/labels/edit/" + TestDataGraph.Labels.LabelUpdated.LabelId, new FormUrlEncodedContent(formData));

            // Assert
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
            Assert.Equal("/Labels", response.Headers.Location.ToString());
        }

        [Fact]
        public async void DeleteGet_ShouldReturnViewWithExistingLabelData()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/labels/delete/" + TestDataGraph.Labels.LabelExisting.LabelId);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Delete - Tuneage.WebApi</title>", responseString);
            Assert.Contains(TestDataGraph.Labels.LabelExisting.Name, responseString);
            Assert.Contains(TestDataGraph.Labels.LabelExisting.WebsiteUrl, responseString);
        }

        [Fact(Skip = "This POST attempt returns a Bad Request. Need to figure out right way to do this.")]
        public async void DeletePost_ShouldReturnViewWithExistingLabelDataRemoved()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                { "LabelId", TestDataGraph.Labels.LabelExisting.LabelId.ToString() },
                { "Name", TestDataGraph.Labels.LabelExisting.Name },
                { "WebsiteUrl", TestDataGraph.Labels.LabelExisting.WebsiteUrl }
            };

            // Act
            var response = await _client.PostAsync("/labels/delete/" + TestDataGraph.Labels.LabelExisting.LabelId, new FormUrlEncodedContent(formData));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<title>Index - Tuneage.WebApi</title>", responseString);
            Assert.DoesNotContain(TestDataGraph.Labels.LabelExisting.Name, responseString);
            Assert.DoesNotContain(TestDataGraph.Labels.LabelExisting.WebsiteUrl, responseString);
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
