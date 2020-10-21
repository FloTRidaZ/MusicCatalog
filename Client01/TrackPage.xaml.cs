using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
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
using Client01.Scripts;
using Windows.UI.Xaml.Media.Imaging;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TrackPage : Page
    {
        public TrackPage()
        {
            this.InitializeComponent();
            App app = Application.Current as App;
            Artist artist = app.ArtistList[1];
            using (SqlConnection connection = new SqlConnection(app.GetConnectionString()))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = @"SELECT file_stream FROM image_table WHERE CAST(path_locator AS NVARCHAR(MAX)) LIKE '" + artist.Cover + "'";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        BitmapImage bitmap = new BitmapImage();
                        SetStream(bitmap, reader);                     
                        Image.Source = bitmap;
                    }
                }
            }
                
        }

        public async void SetStream(BitmapImage bitmap, SqlDataReader reader)
        {
            await bitmap.SetSourceAsync(WindowsRuntimeStreamExtensions.AsRandomAccessStream(reader.GetStream(0)));
        }
    }
}
