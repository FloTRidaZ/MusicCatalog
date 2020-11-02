using Client01.ru.kso.Pages.Account;
using Client01.ru.kso.Pages.PageAlbum;
using Client01.ru.kso.Pages.PageArtist;
using Client01.ru.kso.Pages.PageTrack;
using System;
using System.Collections.Generic;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Client01.ru.kso.Pages.Main
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
        private readonly List<(string tag, NavigationViewItem item)> _items = new List<(string tag, NavigationViewItem item)>();
        private readonly ApplicationDataContainer _localSettings;
        public MainPage()
        {
            this.InitializeComponent();
            MediaPlayer mediaPlayer = new MediaPlayer
            {
                AudioCategory = MediaPlayerAudioCategory.Media
            };
            _media.SetMediaPlayer(mediaPlayer);
            _localSettings = ApplicationData.Current.LocalSettings;
            _items.Add(("Authorization", _toAuthorizationBtn));
            _items.Add(("Exit", _logOutBtn));
            if (_localSettings.Values.ContainsKey("acc"))
            {
                _toAuthorizationBtn.Visibility = Visibility.Collapsed;
                _logOutBtn.Visibility = Visibility.Visible;
            }
        }



        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem selected = args.SelectedItem as NavigationViewItem;
            Type page = _pages.Find(p => p.tag == selected.Tag as string).page;
            ContentFrame.Navigate(page);
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationView navigationView = sender as NavigationView;
            navigationView.SelectedItem = navigationView.MenuItems[0];
            (navigationView.SettingsItem as NavigationViewItem).Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(TrackListPage));
        }

        private void ToAuthorizationItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(AuthorizationPage), _items);
        }

        private void LogOutBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _toAuthorizationBtn.Visibility = Visibility.Visible;
            _logOutBtn.Visibility = Visibility.Collapsed;
            _localSettings.Values.Remove("acc");
            ContentFrame.Navigate(typeof(TrackListPage));
            ShowDialog();
        }

        private async void ShowDialog()
        {
            await new ContentDialog
            {
                Title = "Информация",
                Content = "Вы вышли из аккаунта",
                PrimaryButtonText = "ОК"
            }.ShowAsync();
        }
    }
}
