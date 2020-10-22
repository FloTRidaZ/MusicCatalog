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
        public Guid MusicSrcId { get; private set; }
        public SqlBinary TextDataSrc { get; private set; }

        public Asset(int id, string name, Guid musicAsset, SqlBinary textDataAsset)
        {
            Id = id;
            Name = name;
            MusicSrcId = musicAsset;
            TextDataSrc = textDataAsset;
        }
    }
}
