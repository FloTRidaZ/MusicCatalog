using Client01.ru.kso.Database.Catalog;
using Client01.ru.kso.Database.Datatype;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01.ru.kso.Pages.PageArtist
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ArtistListPage : Page
    {
        private readonly List<Artist> ArtistList;
        public ArtistListPage()
        {
            this.InitializeComponent();
            ArtistList = MusicCatalogCollection.GetCatalog().GetArtistList();
        }

        private void ArtistGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistPage), e.ClickedItem);
        }
    }
}
