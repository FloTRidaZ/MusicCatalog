using System.Collections.Generic;

namespace Client01.Scripts
{
    public sealed class AlbumCollection
    {
        public List<Album> Content { get; set; }

        public bool IsEmpty()
        {
            return Content == null || Content.Count == 0;
        }

    }
}
