using Client01.Scripts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public TrackPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Track target = e.Parameter as Track;
            Cover.Source = target.Album.CoverSrc;
            TrackName.Text = target.Name;
            MediaPlayer player = new MediaPlayer();
            _mediaPlayerElement.SetMediaPlayer(player);
            CreateMediaSource(target, player);
            player.Play();
            
        }

        private void CreateMediaSource(Track target, MediaPlayer player)
        {
            App app = Application.Current as App;
            using(SqlConnection connection = new SqlConnection(app.GetConnectionString()))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT file_stream FROM music_table WHERE CAST(stream_id AS nvarchar(MAX)) LIKE '" + target.Asset.MusicSrcId.ToString() + "';";
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    Stream stream = reader.GetStream(0);
                    TrackName.Text = stream.Length.ToString();
                    IRandomAccessStream randomStream = WindowsRuntimeStreamExtensions.AsRandomAccessStream(stream);
                    MediaSource source = MediaSource.CreateFromStream(randomStream, "MPEG");
                    player.Source = source;
                }
            }
        }
    }
}
