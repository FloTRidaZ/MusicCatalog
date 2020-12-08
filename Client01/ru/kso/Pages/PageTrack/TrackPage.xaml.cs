using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Database.Query;
using Client01.ru.kso.Pages.PageAlbum;
using Client01.ru.kso.Pages.PageArtist;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace Client01.ru.kso.Pages.PageTrack
{

    public sealed partial class TrackPage : Page
    {
        private Track _track;
        private MediaPlayerElement _mediaElement;
        public TrackPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _track = e.Parameter as Track;
            FillTrackContent();
            _mediaElement = ((this.Frame.Parent as ScrollViewer).Parent as Grid).Children.ToList().
            Find(ui => ui.GetType() == typeof(MediaPlayerElement)) as MediaPlayerElement;
        }

        private void FillTrackContent()
        {
            _cover.Source = _track.Album.CoverSrc;
            _trackName.Text = _track.Name;
            _trackAlbum.Text = "Альбом " + _track.Album.Name;
            _trackArtist.Text = "Исполнитель " + _track.Artist.Name;
            _trackLetters.Text = "Текст песни\n" + Encoding.Default.GetString(_track.Letters.Value);
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

        private async void BtnPlay_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!User.IsLogIn())
            {
                await new ContentDialog
                {
                    Title = "Информация",
                    Content = "Авторизуйтесь в системе для получения возможности прослушивания песен",
                    PrimaryButtonText = "ОК"
                }.ShowAsync();
                return;
            }
            _mediaElement.MediaPlayer.Pause();
            IRandomAccessStream src = null;
            await Task.Run(() =>
            {
                App app = Application.Current as App;
                try
                {
                    using (SqlConnection connection = new SqlConnection(app.GetConnectionString()))
                    {
                        connection.Open();
                        SqlCommand cmd = connection.CreateCommand();
                        cmd.CommandText = string.Format(DBQueryCollection.QUERY_FOR_MUSIC_SRC, _track.SrcId.ToString());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            src = WindowsRuntimeStreamExtensions.AsRandomAccessStream(reader.GetStream(0));
                        }
                    }
                }
                catch (Exception)
                {
                    ShowErrorDialog();
                    return;
                } 
            });
            MediaSource mediaSource = MediaSource.CreateFromStream(src, "MPEG");
            MediaPlaybackItem mediaPlaybackItem = new MediaPlaybackItem(mediaSource);
            MediaItemDisplayProperties props = mediaPlaybackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Music;
            props.MusicProperties.Artist = _track.Artist.Name;
            props.MusicProperties.AlbumTitle = _track.Album.Name;
            props.MusicProperties.Title = _track.Name;
            mediaPlaybackItem.ApplyDisplayProperties(props);
            _mediaElement.MediaPlayer.Source = mediaPlaybackItem;
            _mediaElement.MediaPlayer.Play();
        }
    }
}
