using System;

namespace Dto
{
    public class TrackDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
    }
}