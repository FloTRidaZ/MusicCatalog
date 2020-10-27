using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Pages.PageAlbum;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01.ru.kso.Pages.PageArtist
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ArtistPage : Page
    {
        private List<Album> AlbumList;
        private Artist _artist;
        public ArtistPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _artist = e.Parameter as Artist;
            FillArtistContent();

        }

        private void FillArtistContent()
        {
            _artistName.Text = _artist.Name;
            _cover.Source = _artist.CoverSrc;
            _info.Text = Encoding.Default.GetString(_artist.TextData.Value);
            AlbumList = _artist.AlbumList;
        }

        private void ArtistList_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage), e.ClickedItem);
        }
    }
}
