using Client01.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class AlbumPage : Page
    {
        private List<Track> TrackList;
        private Album _album;
        public AlbumPage()
        {
            this.InitializeComponent();
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
