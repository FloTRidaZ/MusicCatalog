using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Enum;
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

namespace Client01.ru.kso.Pages.Main
{
    public sealed partial class MainPage : Page
    {
        private readonly List<(string tag, Type page)> _pages = new List<(string tag, Type page)> 
        { 
            ("track", typeof(TrackListPage)),
            ("artist", typeof(ArtistListPage)),
            ("album", typeof(AlbumListPage)) 
        };
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
            if (_localSettings.Values.ContainsKey("acc"))
            {
                _toAuthorizationBtn.Visibility = Visibility.Collapsed;
                _logOutBtn.Visibility = Visibility.Visible;
                ApplicationDataCompositeValue values = _localSettings.Values["acc"] as ApplicationDataCompositeValue;
                string name = values["email"].ToString();
                string email = values["name"].ToString();
                User.CreateInstance(email, name);
            }
            App app = App.Current as App;
            app.MainPage = this;
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
            ContentFrame.Navigate(typeof(AuthorizationPage));
        }

        private void LogOutBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            User.ToLogOut();
            SwitchPaneFooter(PaneFooterType.LOG_OUT);
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

        public void SwitchPaneFooter(PaneFooterType type)
        {
            switch (type)
            {
                case PaneFooterType.LOG_IN:
                    SwitchToLogOut();
                    break;
                case PaneFooterType.LOG_OUT:
                    SwitchToLogIn();
                    break;
                default:
                    break;
            }
        }

        private void SwitchToLogIn()
        {
            _logOutBtn.Visibility = Visibility.Collapsed;
            _toAuthorizationBtn.Visibility = Visibility.Visible;
        }

        private void SwitchToLogOut()
        {
            _logOutBtn.Visibility = Visibility.Visible;
            _toAuthorizationBtn.Visibility = Visibility.Collapsed;
        }
    }
}
