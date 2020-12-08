using Client01.ru.kso.Database.Datatype;
using Client01.ru.kso.Database.Query;
using Client01.ru.kso.Pages.PageTrack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Client01.ru.kso.Pages.Account
{
    public sealed partial class AuthorizationPage : Page
    {
        private readonly App _app;
        public AuthorizationPage()
        {
            this.InitializeComponent();
            _app = Application.Current as App;
        }

        private void BtnAuthorization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_app.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    string email = _emailInput.Text;
                    string password = _passwordInput.Password;
                    cmd.CommandText = string.Format(DBQueryCollection.QUERY_FROM_ACCOUNT, email, password);
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
                        string name = reader.GetString(2);
                        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                        ApplicationDataCompositeValue valuePairs = new ApplicationDataCompositeValue
                        {
                            { "email", email },
                            { "name", name }
                        };
                        localSettings.Values["acc"] = valuePairs;
                        User.CreateInstance(email, name);
                    }
                    ContentDialog success = new ContentDialog
                    {
                        Title = "Успех",
                        Content = "Авторизация прошла успешно",
                        PrimaryButtonText = "ОК"
                    };
                    _app.MainPage.SwitchPaneFooter(Enum.PaneFooterType.LOG_IN);
                    this.Frame.Navigate(typeof(TrackListPage));
                    ShowDialog(success);
                }
            }
            catch (Exception)
            {
                ShowErrorDialog();
            }
        }

        private async void ShowErrorDialog()
        {
            await new ContentDialog
            {
                Title = "Ошибка соединения",
                Content = "Ошибка соединения с сервером, пожалуйста проверьте подключение!",
                PrimaryButtonText = "ОК"
            }.ShowAsync();
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
