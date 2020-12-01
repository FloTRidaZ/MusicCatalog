using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Database.Query;
using Client01.ru.kso.Pages.PageArtist;
using Client01.ru.kso.Pages.PageTrack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace Client01.ru.kso.Pages.PageAlbum
{

    public sealed partial class AlbumPage : Page
    {
        private readonly App _app;
        private List<Track> TrackList;
        private ObservableCollection<Review> ReviewList;
        private ApplicationDataContainer _localSettings;
        private Album _album;
        public AlbumPage()
        {
            this.InitializeComponent();
            _app = Application.Current as App;
            _localSettings = ApplicationData.Current.LocalSettings;
            if (!_localSettings.Values.ContainsKey("acc"))
            {
                _reviewTextInput.Visibility = Visibility.Collapsed;
                _buttonSend.Visibility = Visibility.Collapsed;
                TextBlock infoTextBlock = new TextBlock
                {
                    Text = "Оставлять комментарии могут только зарегистрированные пользователи"
                };
                _reviewBlockPanel.Children.Add(infoTextBlock);
            }
        }

        private void BuildReviewList()
        {
            try
            {
                ReviewList = new ObservableCollection<Review>();
                using (SqlConnection connection = new SqlConnection(_app.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = string.Format(DBQueryCollection.QUERY_FROM_REVIEW, _album.Id);
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
            } catch (Exception)
            {
                ShowErrorSqlDialog();
            }
            
        }

        private async void ShowErrorSqlDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Ошибка соединения",
                Content = "Произошла ошибка соединения с хранилищем данных, некоторые удаленные данные будут недоступны! ",
                PrimaryButtonText = "ОК",
            };
            await dialog.ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _album = e.Parameter as Album;
            FillAlbumContent();
            BuildReviewList();
        }

        private void FillAlbumContent()
        {
            _cover.Source = _album.CoverSrc;
            _albumName.Text = _album.Name;
            _artistName.Text = _album.Artist.Name;
            _albumInfo.Text = Encoding.Default.GetString(_album.TextData.Value);
            TrackList = _album.TrackList;
        }

        private void AlbumList_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TrackPage), e.ClickedItem);
        }

        private void ArtistName_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistPage), _album.Artist);
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_app.GetConnectionString()))
                {
                    connection.Open();
                    ApplicationDataCompositeValue valuePairs = _localSettings.Values["acc"] as ApplicationDataCompositeValue;
                    SqlCommand cmd = connection.CreateCommand();
                    string content = _reviewTextInput.Text.Trim();
                    string reviewer = valuePairs["email"].ToString();
                    Review review = new Review(reviewer, content);
                    ReviewList.Add(review);
                    cmd.CommandText = string.Format(DBQueryCollection.QUERY_FOR_REVIEW_INSERT, reviewer, content, _album.Id.ToString());
                    cmd.ExecuteNonQuery();
                    _reviewTextInput.Text = "";
                }
            }
            catch (Exception)
            {
                ShowErrorSqlDialog();
            }
            
        }

    }
}
