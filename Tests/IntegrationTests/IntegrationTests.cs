using System.Net;
using System.Net.Http;
using NUnit.Framework;
using UI;

namespace Tests.IntegrationTests
{
    [TestFixture]
    internal class IntegrationTests
    {
        [Test]
        public void get_playlists_test()
        {
            Server.RunServer();
            using (var client = new HttpClient())
            {
                Assert.AreEqual(HttpStatusCode.OK,
                                client.GetAsync(ResolveUrl.Playlists()).Result.StatusCode);
            }
            Server.StopServer();
        }

        [Test]
        public void get_root_test()
        {
            Server.RunServer();
            using (var client = new HttpClient())
            {
                Assert.AreEqual(HttpStatusCode.OK,
                                client.GetAsync("http://localhost:8080/").Result.StatusCode);
            }
            Server.StopServer();
        }
    }
}