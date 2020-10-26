using Client01.Scripts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TrackPage : Page
    {
        private const string QUERY_FOR_MUSIC_SRC = "SELECT file_stream FROM music_table" +
            " WHERE CAST(stream_id AS nvarchar(MAX)) LIKE '{0}';";
        private Track _track;
        public TrackPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _track = e.Parameter as Track;
            _cover.Source = _track.Album.CoverSrc;
            _trackName.Text = _track.Name;
            _trackAlbum.Text = "Альбом " + _track.Album.Name;
            _trackArtist.Text = "Исполнитель " + _track.Artist.Name;
            _trackLetters.Text = "Текст песни\n" + Encoding.Default.GetString(_track.Letters.Value);
            MediaPlayerElement mediaPlayerElement = (this.Frame.Parent as StackPanel).Children.ToList().
             Find(ui => ui.GetType() == typeof(MediaPlayerElement)) as MediaPlayerElement;
            mediaPlayerElement.Visibility = Visibility.Visible;
            CreateMediaSource(mediaPlayerElement.MediaPlayer, _track);
            

        }
        
        private void CreateMediaSource(MediaPlayer player, Track target)
        {
            App app = Application.Current as App;
            using(SqlConnection connection = new SqlConnection(app.GetConnectionString()))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format(QUERY_FOR_MUSIC_SRC, target.SrcId.ToString());
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    IRandomAccessStream stream = WindowsRuntimeStreamExtensions.AsRandomAccessStream(reader.GetStream(0));
                    MediaSource source = MediaSource.CreateFromStream(stream, "MPEG");
                    player.Source = source;
                }
            }
        }

        private void TrackAlbum_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage), _track.Album);
        }

        private void TrackArtist_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistPage), _track.Artist);
        }
    }
}
