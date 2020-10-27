using Client01.ru.kso.Database.Catalog;
using Client01.ru.kso.Database.Datatype;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01.ru.kso.Pages.PageAlbum
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
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
