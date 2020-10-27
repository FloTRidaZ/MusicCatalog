using Client01.ru.kso.Pages.PageTrack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01.ru.kso.Pages.Account
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AuthorizationPage : Page
    {
        private readonly App _app;
        private List<(string tag, NavigationViewItem item)> _items;
        public AuthorizationPage()
        {
            this.InitializeComponent();
            _app = Application.Current as App;
        }

        private void BtnAuthorization_Click(object sender, RoutedEventArgs e)
        {
            using(SqlConnection connection = new SqlConnection(_app.GetConnectionString()))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                string email = _emailInput.Text;
                string password = _passwordInput.Password;
                cmd.CommandText = "SELECT * FROM account_table WHERE login LIKE '" + email + "' AND password LIKE '" + password + "';";
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        ContentDialog error = new ContentDialog
                        {
                            Title = "Ошибка авторизации",
                            Content = "Неверно введен логин или пароль",
                            PrimaryButtonText = "ОК"
                        };
                        ShowDialog(error);
                        return;
                    }
                    reader.Read();
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    ApplicationDataCompositeValue valuePairs = new ApplicationDataCompositeValue();
                    valuePairs.Add("email", email);
                    valuePairs.Add("name", reader.GetString(2));
                    localSettings.Values["acc"] = valuePairs;
                }
                ContentDialog success = new ContentDialog
                {
                    Title = "Успех",
                    Content = "Авторизация прошла успешно",
                    PrimaryButtonText = "ОК"
                };
                NavigationViewItem toAuthorizationItem = _items.Find(i => i.tag == "Authorization").item;
                NavigationViewItem exitItem = _items.Find(i => i.tag == "Exit").item;
                toAuthorizationItem.Visibility = Visibility.Collapsed;
                exitItem.Visibility = Visibility.Visible;
                this.Frame.Navigate(typeof(TrackListPage));
                ShowDialog(success);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _items = e.Parameter as List<(string tag, NavigationViewItem item)>;
        }

        private async void ShowDialog(ContentDialog dialog)
        {
            await dialog.ShowAsync();

        }

        private void ToRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegistrationPage));
        }
    }
}
