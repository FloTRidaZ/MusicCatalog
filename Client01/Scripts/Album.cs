using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Client01.Scripts
{
    public sealed class Album
    {
        private readonly Genre _genre;
        public int Id { get; }
        public string Name { get; }
        public Artist Artist { get; }
        public BitmapImage CoverSrc { get; private set; }
        public SqlBinary TextData { get; }
        public List<Track> TrackList { get; }

        public Album(int id, string name, Artist artist, Stream coverStream, SqlBinary textData, string genre)
        {
            Id = id;
            Name = name;
            Artist = artist;
            TextData = textData;
            CreateCoverFromStream(coverStream);
            _genre = BuildGenre(genre);
            TrackList = new List<Track>();
        }

        private Genre BuildGenre(string genre)
        {
            Genre g;
            switch (genre)
            {
                case "Hard Rock":
                    g = Genre.HARD_ROCK;
                    break;
                case "Symphonic Black Metal":
                    g = Genre.SYMPHONIC_BLACK_METAL;
                    break;
                case "Symphonic Metal":
                    g = Genre.SYMPHONIC_METAL;
                    break;
                default:
                    g = Genre.UNKNOWN;
                    break;
            }
            return g;
        }

        public string GetGenre()
        {
            string genre = "Unknown";
            switch (_genre)
            {
                case Genre.HARD_ROCK:
                    genre = "Hard Rock";
                    break;
                case Genre.SYMPHONIC_BLACK_METAL:
                    genre = "Symphonic Black Metal";
                    break;
                case Genre.SYMPHONIC_METAL:
                    genre = "Symphonic Metal";
                    break;
            }
            return genre;
        }

        public Genre GetRawGenre()
        {
            return _genre;
        }

        private async void CreateCoverFromStream(Stream coverStream)
        {
            IRandomAccessStream src = WindowsRuntimeStreamExtensions.AsRandomAccessStream(coverStream);
            CoverSrc= new BitmapImage();
            await CoverSrc.SetSourceAsync(src);
        }
    }
}
