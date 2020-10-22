using System.Collections.Generic;

namespace Client01.Scripts
{
    public sealed class MusicCatalogCollection
    {
        private readonly ArtistCollection _artistCollection;
        private readonly AlbumCollection _albumCollection;
        private readonly TrackCollection _trackCollection;

        public MusicCatalogCollection()
        {
            _artistCollection = new ArtistCollection();
            _albumCollection = new AlbumCollection();
            _trackCollection = new TrackCollection();
        }

        public void AddArtistListOnce(List<Artist> artistList)
        {
            if (_artistCollection.IsEmpty())
            {
                _artistCollection.Content = artistList;
            }
        }

        public void AddAlbumListOnce(List<Album> albumList)
        {
            if (_albumCollection.IsEmpty())
            {
                _albumCollection.Content = albumList;
            }
        }

        public void AddTrackListOnce(List<Track> trackList)
        {
            if (_trackCollection.IsEmpty())
            {
                _trackCollection.Content = trackList;
            }
        }

        public List<Artist> GetArtistList()
        {
            return _artistCollection.Content;
        }

        public List<Album> GetAlbumList()
        {
            return _albumCollection.Content;
        }

        public List<Track> GetTrackList()
        {
            return _trackCollection.Content;
        }

    }
}
