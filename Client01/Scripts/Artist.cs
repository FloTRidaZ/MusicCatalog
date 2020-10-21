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
        public SqlBinary TextData { get; }
        public List<Album> AlbumList {
            get
            {
                return AlbumList;
            }
            set
            {
                if (AlbumList is null)
                {
                    AlbumList = value;
                }
            }
        }
        public Artist(int id, string name, Stream coverStream, SqlBinary textData)
        {
            Id = id;
            Name = name;
            TextData = textData;
            CreateCover(coverStream);
        }

        public async void CreateCover(Stream coverStream)
        {
            CoverSrc = new BitmapImage();
            IRandomAccessStream src = WindowsRuntimeStreamExtensions.AsRandomAccessStream(coverStream);
            await CoverSrc.SetSourceAsync(src);
            
        }
    }
}
