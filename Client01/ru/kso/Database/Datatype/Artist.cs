using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Client01.ru.kso.Database.Datatype
{
    public sealed class Artist 
    {
        public int Id { get; }
        public string Name { get; }
        public BitmapImage CoverSrc { get; set; }
        public SqlBinary TextData { get; set; }
        public List<Album> AlbumList { get; set; }
        public Artist(int id, string name)
        {
            Id = id;
            Name = name;
            AlbumList = new List<Album>();
        }

        public async void CreateCoverFromStream(Stream coverStream)
        {
            CoverSrc = new BitmapImage();
            IRandomAccessStream src = WindowsRuntimeStreamExtensions.AsRandomAccessStream(coverStream);
            await CoverSrc.SetSourceAsync(src);
            
        }
    }
}
