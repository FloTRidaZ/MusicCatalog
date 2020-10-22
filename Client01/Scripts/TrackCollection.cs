using System.Collections.Generic;

namespace Client01.Scripts
{
    public sealed class TrackCollection
    {
        public List<Track> Content { get; set; }
        public bool IsEmpty()
        {
            return Content == null || Content.Count == 0;
        }
    }
}
