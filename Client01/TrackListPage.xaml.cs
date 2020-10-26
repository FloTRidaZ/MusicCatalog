﻿using Client01.Scripts;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TrackListPage : Page
    {
        private readonly List<Track> TrackList;
        public TrackListPage()
        {
            this.InitializeComponent();
            App app = Application.Current as App;
            TrackList = app.CatalogCollection.GetTrackList();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TrackPage), e.ClickedItem);
        }
    }
}
