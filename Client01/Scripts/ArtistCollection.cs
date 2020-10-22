using System.Collections.Generic;

namespace Client01.Scripts
{
    public sealed class ArtistCollection
    {
        public List<Artist> Content { get; set; }


        public bool IsEmpty()
        {
            return Content == null || Content.Count == 0;
        }
    }
}
