using Client01.ru.kso.Database.Catalog;
using Client01.ru.kso.Database.Datatype;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Client01.ru.kso.Pages.PageAlbum
{

    public sealed partial class AlbumListPage : Page
    {
        private readonly List<Album> AlbumList;
        public AlbumListPage()
        {
            this.InitializeComponent();
            AlbumList = MusicCatalogCollection.GetCatalog().GetAlbumList();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage), e.ClickedItem);
        }
    }
}
