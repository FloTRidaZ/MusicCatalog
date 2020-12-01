using Client01.ru.kso.Database.Catalog;
using Client01.ru.kso.Database.Datatype;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Client01.ru.kso.Pages.PageArtist
{
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
