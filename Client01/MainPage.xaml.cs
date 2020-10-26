using System;
using System.Collections.Generic;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            ("album", typeof(AlbumListPage)) 
        };
        private List<(string tag, NavigationViewItem item)> _items = new List<(string tag, NavigationViewItem item)>();
        private ApplicationDataContainer _localSettings;
        public MainPage()
        {
            this.InitializeComponent();
            _media.SetMediaPlayer(new MediaPlayer { Volume = 35 });
            _localSettings = ApplicationData.Current.LocalSettings;
            _items.Add(("Authorization", _toAuthorizationBtn));
            _items.Add(("Exit", _exitBtn));
            if (_localSettings.Values.ContainsKey("Name"))
            {
                _toAuthorizationBtn.Visibility = Visibility.Collapsed;
                _exitBtn.Visibility = Visibility.Visible;
            }
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

        private void ToAuthorizationItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(AuthorizationPage), _items);
        }

        private void ExitBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _toAuthorizationBtn.Visibility = Visibility.Visible;
            _exitBtn.Visibility = Visibility.Collapsed;
            _localSettings.Values.Remove("Name");
        }
    }
}
