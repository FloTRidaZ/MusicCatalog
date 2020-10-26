using Client01.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Client01
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        private const string CONNECTION_STRING = @"Data Source = DESKTOP-HBEEL2G\SQLEXPRESS; Initial Catalog = MusicCatalogDB; User ID = sa; Password = flotridaz58rus";
        
        private const string QUERY_FROM_ARTIST =
            "SELECT at.id, at.name, it.file_stream AS cover, tdt.file_stream AS info FROM artist_table AS at " +
                        "JOIN image_table AS it ON CAST(it.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.cover AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.info AS NVARCHAR(MAX));";

        private const string QUERY_FROM_ALBUM =
            "SELECT at.id, at.name, art.name AS artist, gt.name AS genre, it.file_stream AS cover, tdt.file_stream AS info " +
                        "FROM album_table AS at " +
                        "JOIN artist_table AS art ON art.id LIKE at.artist " +
                        "JOIN genre_table AS gt ON gt.id LIKE at.genre " +
                        "JOIN image_table AS it ON CAST(it.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.cover AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.info AS NVARCHAR(MAX));";

        private const string QUERY_FROM_TRACK =
            "SELECT tb.id, tb.name, alt.name AS album, mt.stream_id as src, tdt.file_stream as letters, tb.album_pos FROM track_table AS tb " +
                        "JOIN album_table AS alt ON alt.id LIKE tb.album " +
                        "JOIN music_table AS mt ON CAST(mt.path_locator AS NVARCHAR(MAX)) LIKE CAST(tb.src AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(tb.letters AS NVARCHAR(MAX));";
        public MusicCatalogCollection CatalogCollection { get; private set; }
        /// <summary>
        /// Инициализирует одноэлементный объект приложения. Это первая выполняемая строка разрабатываемого
        /// кода, поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем. Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitData();

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }
        }

        private void InitData()
        {
            CatalogCollection = new MusicCatalogCollection();
            List<Artist> artistList;
            List<Album> albumList;
            List<Track> trackList;
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                artistList = BuildArtistList(connection);
                albumList = BuildAlbumList(connection, artistList);
                trackList = BuildTrackList(connection, albumList);
            }
            AttachToArtistList(artistList, albumList);
            AttachToAlbumList(albumList, trackList);
            CatalogCollection.AddArtistListOnce(artistList);
            CatalogCollection.AddAlbumListOnce(albumList);
            CatalogCollection.AddTrackListOnce(trackList);
        }
        
        private void AttachToArtistList(List<Artist> to, List<Album> from)
        {
            foreach (Artist anArtist in to)
            {
                foreach(Album anAlbum in from)
                {
                    if (anAlbum.Artist.Name.Equals(anArtist.Name))
                    {
                        anArtist.AlbumList.Add(anAlbum);
                    }
                }
            }
        }

        private void AttachToAlbumList(List<Album> to, List<Track> from)
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

        private List<Track> BuildTrackList(SqlConnection connection, List<Album> albumList)
        {
            List<Track> trackList = new List<Track>();
            using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = QUERY_FROM_TRACK;
                using(SqlDataReader reader = cmd.ExecuteReader())
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

        private List<Album> BuildAlbumList(SqlConnection connection, List<Artist> artistList)
        {
            List<Album> albumList = new List<Album>();
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = QUERY_FROM_ALBUM;
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

        private List<Artist> BuildArtistList(SqlConnection connection)
        {
            List<Artist> artistList = new List<Artist>();
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = QUERY_FROM_ARTIST;
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

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }

        public string GetConnectionString()
        {
            return CONNECTION_STRING;
        }
    }
}
