using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Persistence.DO;
using Persistence.Repositories;

namespace Tests.Persistence
{
    internal class TestTrackWebRepository
    {
        [Test]
        public void test_get_tracks()
        {
            //Arrange
            var repo = new TrackWebRepository();
            var ids = new List<String>
                {
                    "0mWiuXuLAJ3Brin3Or2x6v",
                    "3ZsjgLDSvusBgxGWrTAVto"
                };

            //Act
            IEnumerable<Track> tracks = repo.GetTracks(ids);

            //Assert
            Assert.AreEqual(ids.Count, tracks.Count());
        }

        [Test]
        public void test_search()
        {
            var repo = new TrackWebRepository();
            IEnumerable<Track> tracks = repo.Search("foo", 1);
            Assert.AreEqual(100, tracks.Count());
        }
    }
}