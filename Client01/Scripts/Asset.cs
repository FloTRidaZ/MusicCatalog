using System;
using System.Data.SqlTypes;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Client01.Scripts
{
    public sealed class Asset
    {
        public int Id { get; }
        public string Name { get; }
        public BitmapImage ImageSrc { get; private set; }
        public SqlBinary MusicSrc { get; private set; }
        public SqlBinary TextDataSrc { get; private set; }

        public Asset(int id, string name, Stream imageStream, SqlBinary musicAsset, SqlBinary textDataAsset)
        {
            Id = id;
            Name = name;
            CreateImageAsset(imageStream);
            MusicSrc = musicAsset;
            TextDataSrc = textDataAsset;
        }

        private async void CreateImageAsset(Stream imageStream)
        {
            IRandomAccessStream src = WindowsRuntimeStreamExtensions.AsRandomAccessStream(imageStream);
            ImageSrc = new BitmapImage();
            await ImageSrc.SetSourceAsync(src);
            
        }
    }
}
