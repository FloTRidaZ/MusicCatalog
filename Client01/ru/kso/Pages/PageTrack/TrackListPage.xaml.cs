using Client01.ru.kso.Database.Catalog;
using Client01.ru.kso.Database.Datatype;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;


namespace Client01.ru.kso.Pages.PageTrack
{
    public sealed partial class TrackListPage : Page
    {
        private readonly List<Track> TrackList;
        public TrackListPage()
        {
            this.InitializeComponent();
            TrackList = MusicCatalogCollection.GetCatalog().GetTrackList();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TrackPage), e.ClickedItem);
        }
    }
}
