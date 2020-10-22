using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Client01
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly List<(string tag, Type page)> _pages = new List<(string tag, Type page)> 
        { 
            ("track", typeof(TrackListPage)),
            ("artist", typeof(ArtistListPage)),
            ("album", typeof(AlbumPage)) 
        };
        public MainPage()
        {
            this.InitializeComponent();
        }



        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem selected = args.SelectedItem as NavigationViewItem;
            var (tag, page) = _pages.Find(p => p.tag == selected.Tag as string);
            ContentFrame.Navigate(page);
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationView navigationView = sender as NavigationView;
            navigationView.SelectedItem = navigationView.MenuItems[0];
            NavigationViewItem settingsItem = navigationView.SettingsItem as NavigationViewItem;
            settingsItem.Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(TrackListPage));
        }
    }
}
