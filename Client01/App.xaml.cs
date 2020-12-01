using Client01.ru.kso.Database.Catalog;
using Client01.ru.kso.Pages.Main;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Client01
{
    sealed partial class App : Application
    {
        private const string CONNECTION_STRING = @"Data Source = DESKTOP-HBEEL2G\SQLEXPRESS; Initial Catalog = MusicCatalogDB; User ID = sa; Password = flotridaz58rus";
        public MusicCatalogCollection CatalogCollection { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitData();
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        private void InitData()
        {
            MusicCatalogCollection.CreateNewCatalog();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        public string GetConnectionString()
        {
            return CONNECTION_STRING;
        }
    }
}
