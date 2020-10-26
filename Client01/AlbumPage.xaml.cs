using Client01.Scripts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AlbumPage : Page
    {
        private List<Track> TrackList;
        private List<Review> ReviewList;
        private Album _album;
        public AlbumPage()
        {
            this.InitializeComponent();
            App app = Application.Current as App;
            BuildReviewList(app);
        }

        private void BuildReviewList(App app)
        {
            ReviewList = new List<Review>();
            using (SqlConnection connection = new SqlConnection(app.GetConnectionString()))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ac.name, rev.review_data FROM review_table AS rev JOIN account_table AS ac ON ac.login LIKE rev.acc_id;";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Review review = new Review(
                            reader.GetString(0),
                            reader.GetString(1)
                        );
                        ReviewList.Add(review);
                    }
                    
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _album = e.Parameter as Album;
            _cover.Source = _album.CoverSrc;
            _albumName.Text = _album.Name;
            _artistName.Text = _album.Artist.Name;
            _albumInfo.Text = Encoding.Default.GetString(_album.TextData.Value);
            TrackList = _album.TrackList;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TrackPage), e.ClickedItem);
        }

        private void ArtistName_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistPage), _album.Artist);
        }
    }
}
