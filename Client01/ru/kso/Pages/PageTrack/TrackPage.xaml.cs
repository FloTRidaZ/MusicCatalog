using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Database.Query;
using Client01.ru.kso.Pages.PageAlbum;
using Client01.ru.kso.Pages.PageArtist;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01.ru.kso.Pages.PageTrack
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TrackPage : Page
    {
        private Track _track;
        public TrackPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _track = e.Parameter as Track;
            FillTrackContent();
            MediaPlayerElement mediaPlayerElement = (this.Frame.Parent as StackPanel).Children.ToList().
             Find(ui => ui.GetType() == typeof(MediaPlayerElement)) as MediaPlayerElement;
            mediaPlayerElement.Visibility = Visibility.Visible;
            CreateMediaSource(mediaPlayerElement.MediaPlayer, _track);
        }

        private void FillTrackContent()
        {
            _cover.Source = _track.Album.CoverSrc;
            _trackName.Text = _track.Name;
            _trackAlbum.Text = "Альбом " + _track.Album.Name;
            _trackArtist.Text = "Исполнитель " + _track.Artist.Name;
            _trackLetters.Text = "Текст песни\n" + Encoding.Default.GetString(_track.Letters.Value);
        }

        private void CreateMediaSource(MediaPlayer player, Track target)
        {
            App app = Application.Current as App;
            try
            {
                using (SqlConnection connection = new SqlConnection(app.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = string.Format(DBQueryCollection.QUERY_FOR_MUSIC_SRC, target.SrcId.ToString());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        IRandomAccessStream stream = WindowsRuntimeStreamExtensions.AsRandomAccessStream(reader.GetStream(0));
                        MediaSource source = MediaSource.CreateFromStream(stream, "MPEG");
                        player.Source = source;
                    }
                }
            }
            catch (SqlException)
            {
                ShowErrorDialog();
            }
        }

        private async void ShowErrorDialog()
        {
            await new ContentDialog
            {
                Title = "Ошибка соединения",
                Content = "Ошибка соединения с сервером, некоторые ресурсы будут недоступны",
                PrimaryButtonText = "ОК"
            }.ShowAsync();
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
