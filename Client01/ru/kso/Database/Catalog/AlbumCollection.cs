using Client01.ru.kso.Database.Datatype;
using System.Collections.Generic;

namespace Client01.ru.kso.Database.Catalog
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
