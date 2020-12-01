using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Database.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Client01.ru.kso.Database.Catalog
{
    public sealed class MusicCatalogCollection
    {
        private readonly ArtistCollection _artistCollection;
        private readonly AlbumCollection _albumCollection;
        private readonly TrackCollection _trackCollection;
        private static MusicCatalogCollection _catalog;

        public static MusicCatalogCollection GetCatalog()
        {
            if (_catalog != null)
            {
                return _catalog;
            }
            CreateCatalog((Application.Current as App).GetConnectionString());
            return _catalog;
        }

        public static void CreateNewCatalog()
        {
            if (_catalog != null)
            {
                return;
            }
            CreateCatalog((Application.Current as App).GetConnectionString());
        }

        private static void CreateCatalog(string connectionString)
        {
            try
            {
                _catalog = new MusicCatalogCollection();
                List<Artist> artistList;
                List<Album> albumList;
                List<Track> trackList;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    artistList = BuildArtistList(connection);
                    albumList = BuildAlbumList(connection, artistList);
                    trackList = BuildTrackList(connection, albumList);
                }
                AttachToArtistList(artistList, albumList);
                AttachToAlbumList(albumList, trackList);
                _catalog.SetArtistList(artistList);
                _catalog.SetAlbumList(albumList);
                _catalog.SetTrackList(trackList);
            }
            catch (Exception)
            {
                ShowErrorDialog();
            }
        }

        private async static void ShowErrorDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Ошибка соединения",
                Content = "Ошибка соединения с удаленным сервером, пожалуйста проверьте подключение к интернету",
                PrimaryButtonText = "ОК"
            };
            ManageDialog(await dialog.ShowAsync());
        }

        private static void ManageDialog(ContentDialogResult result)
        {
            if (result == ContentDialogResult.Primary)
            {
                Application.Current.Exit();
            }
        }

        private MusicCatalogCollection()
        {
            _artistCollection = new ArtistCollection();
            _albumCollection = new AlbumCollection();
            _trackCollection = new TrackCollection();
        }

    private static void AttachToArtistList(List<Artist> to, List<Album> from)
    {
        foreach (Artist anArtist in to)
        {
            foreach (Album anAlbum in from)
            {
                if (anAlbum.Artist.Name.Equals(anArtist.Name))
                {
                    anArtist.AlbumList.Add(anAlbum);
                }
            }
        }
    }

    private static void AttachToAlbumList(List<Album> to, List<Track> from)
    {
        foreach (Album anAlbum in to)
        {
            foreach (Track aTrack in from)
            {
                if (aTrack.Album.Name.Equals(anAlbum.Name))
                {
                    anAlbum.TrackList.Add(aTrack);
                }
            }
        }
    }

    private static List<Track> BuildTrackList(SqlConnection connection, List<Album> albumList)
    {
        List<Track> trackList = new List<Track>();
        using (SqlCommand cmd = connection.CreateCommand())
        {
            cmd.CommandText = DBQueryCollection.QUERY_FROM_TRACK;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Album album;
                while (reader.Read())
                {
                    album = albumList.Find(alb => alb.Name == reader.GetString(2));
                    Track track = new Track(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        album,
                        reader.GetInt32(5)
                    )
                    {
                        SrcId = reader.GetGuid(3),
                        Letters = reader.GetSqlBinary(4)
                    };
                    trackList.Add(track);
                }
            }
        }
        return trackList;
    }

    private static List<Album> BuildAlbumList(SqlConnection connection, List<Artist> artistList)
    {
        List<Album> albumList = new List<Album>();
        using (SqlCommand cmd = connection.CreateCommand())
        {
            cmd.CommandText = DBQueryCollection.QUERY_FROM_ALBUM;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Artist artist;
                while (reader.Read())
                {
                    artist = artistList.Find(a => a.Name == reader.GetString(2));
                    Album album = new Album(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    artist,
                    reader.GetString(3)
                    )
                    {
                        TextData = reader.GetSqlBinary(5)
                    };
                    album.CreateCoverFromStream(reader.GetStream(4));
                    albumList.Add(album);
                }
            }
        }
        return albumList;
    }

    private static List<Artist> BuildArtistList(SqlConnection connection)
    {
        List<Artist> artistList = new List<Artist>();
        using (SqlCommand cmd = connection.CreateCommand())
        {
            cmd.CommandText = DBQueryCollection.QUERY_FROM_ARTIST;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Artist artist = new Artist(
                        reader.GetInt32(0),
                        reader.GetString(1)
                    )
                    {
                        TextData = reader.GetSqlBinary(3)
                    };
                    artist.CreateCoverFromStream(reader.GetStream(2));
                    artistList.Add(artist);

                }
            }
        }
        return artistList;
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

        private void SetArtistList(List<Artist> artistList)
        {
            _artistCollection.Content = artistList;
        }

        private void SetAlbumList(List<Album> albumList)
        {
            _albumCollection.Content = albumList;
        }

        private void SetTrackList(List<Track> trackList)
        {
            _trackCollection.Content = trackList;
        }

    }
}
