using Client01.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Client01
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        private const string connectionString = @"Data Source = DESKTOP-HBEEL2G\SQLEXPRESS; Initial Catalog = MusicCatalogDB; User ID = sa; Password = flotridaz58rus";
        public List<Artist> ArtistList { get; private set; }
        public List<Album> AlbumList { get; private set; }
        /// <summary>
        /// Инициализирует одноэлементный объект приложения. Это первая выполняемая строка разрабатываемого
        /// кода, поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

        }

        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем. Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitData();
            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }
        }

        private void InitData()
        {
            ArtistList = new List<Artist>();
            AlbumList = new List<Album>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT at.id, at.name, it.file_stream AS cover, tdt.file_stream AS info FROM artist_table AS at " +
                        "JOIN image_table AS it ON CAST(it.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.cover AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.info AS NVARCHAR(MAX));";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            
                            Artist artist = new Artist(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetStream(2),
                                reader.GetSqlBinary(3)
                            );

                            ArtistList.Add(artist);
                          
                        }
                    }
                    cmd.CommandText = "SELECT at.id, at.name, art.name AS artist, gt.name AS genre, it.file_stream AS cover, tdt.file_stream AS info " +
                        "FROM album_table AS at " +
                        "JOIN artist_table AS art ON art.id LIKE at.artist " +
                        "JOIN genre_table AS gt ON gt.id LIKE at.genre " +
                        "JOIN image_table AS it ON CAST(it.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.cover AS NVARCHAR(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS NVARCHAR(MAX)) LIKE CAST(at.info AS NVARCHAR(MAX));";
                    using (SqlDataReader reader1 = cmd.ExecuteReader())
                    {
                        Artist artist;
                        while (reader1.Read())
                        {
                            artist = ArtistList.Find(a => a.Name == reader1.GetString(2));
                            Album album = new Album(
                                reader1.GetInt32(0),
                                reader1.GetString(1),
                                artist,
                                reader1.GetStream(4),
                                reader1.GetSqlBinary(5),
                                reader1.GetString(3)

                            );
                            AlbumList.Add(album);
                        }
                    }
                    cmd.CommandText = "SELECT it.file_stream AS image, mt.file_stream as music, tdt.file_stream as text_data FROM image_table AS it, " +
                        "asset_table AS at " +
                        "JOIN music_table AS mt ON CAST(mt.path_locator AS nvarchar(MAX)) LIKE CAST(at.music_asset AS nvarchar(MAX)) " +
                        "JOIN text_data_table AS tdt ON CAST(tdt.path_locator AS nvarchar(MAX)) LIKE CAST(at.text_asset AS nvarchar(MAX))" +
                        "WHERE CAST(it.path_locator AS nvarchar(MAX)) LIKE CAST(at.image_asset AS nvarchar(MAX));";

                }
            }
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }

        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}
