using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Client01.Scripts
{
    public sealed class Artist 
    {
        public int Id { get; }
        public string Name { get; }
        public BitmapImage CoverSrc { get; private set; }
        public SqlBinary TextData { get; set; }
        public List<Album> AlbumList { get; }
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
