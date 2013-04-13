using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using DAL.Concrete;
using NUnit.Framework;

namespace Tests.UnitTests.DAL
{
    //reflexao para ir buscar a connection e fazer transacoes
    [TestFixture]
    public class PlaylistDALTests
    {
        [Test]
        public void test_get_all_empty_db()
        {
            var repo = new DbPlaylistRepository();
            IEnumerable<Playlist> playlists = repo.GetAll();
            Assert.AreEqual(0, playlists.Count());
        }
    }
}