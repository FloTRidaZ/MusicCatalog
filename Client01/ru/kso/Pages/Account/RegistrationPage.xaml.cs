using Client01.ru.kso.Database.Query;
using System;
using System.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client01.ru.kso.Pages.Account
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class RegistrationPage : Page
    {
        private readonly App _app;
        public RegistrationPage()
        {
            this.InitializeComponent();
            _app = Application.Current as App;
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_app.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    string email = _emailInput.Text;
                    string password = _passwordInput.Password;
                    string repeatPassword = _repeatPasswordInput.Password;
                    string name = _nameInput.Text;
                    if (!IsValidateData(name, email, password, repeatPassword))
                    {
                        return;
                    }
                    cmd.CommandText = string.Format(DBQueryCollection.QUERY_FOR_ACC_INSERT, email, password, name);
                    cmd.ExecuteNonQuery();
                    this.Frame.Navigate(typeof(AuthorizationPage));
                    ShowInfoDialog();
                }
            } catch (SqlException)
            {
                ShowErrorDialog("Потеряно соединение с интернетом");
            }

        }

        private async void ShowInfoDialog()
        {
            await new ContentDialog { Title = "Успех", Content = "Регистрация прошла успешно", PrimaryButtonText = "ОК" }.ShowAsync();
        }

        private bool IsValidateData(string name, string email, string password, string repeatPassword)
        {
            if (!email.Contains("@"))
            {
                ShowErrorDialog("Неправильный формат электронной почты");
                return false;
            }
            if (name.Length == 0)
            {
                ShowErrorDialog("Введите имя");
                return false;
            }
            if (email.Length < 4)
            {
                ShowErrorDialog("Слишком короткий логин");
                return false;
            }
            if (password.Length < 4)
            {
                ShowErrorDialog("Слишком короткий пароль");
                return false;
            }
            if (!password.Equals(repeatPassword))
            {
                ShowErrorDialog("Пароли не совпадают");
                return false;
            }
            return true;
        }

        private async void ShowErrorDialog(string msg)
        {
            await new ContentDialog { Title = "Ошибка регистрации", Content = msg, PrimaryButtonText = "ОК"}.ShowAsync();
        }
    }
}
