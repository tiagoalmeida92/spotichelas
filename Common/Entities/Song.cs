using System;

namespace Common.Entities
{
    public class Song
    {
        public string Id { get; set; }
        public String Name { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
    }
}