using pract.ApplicationData;
using System;
using System.Windows;
using System.Windows.Controls;

namespace pract.Views
{
    public partial class ChangePasswordPage : Page
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            HideError();

            if (string.IsNullOrWhiteSpace(OldPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                ShowError("Заполните все поля");
                return;
            }

            if (AppConnect.currentUser.password != OldPasswordBox.Password)
            {
                ShowError("Неверный старый пароль");
                return;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                ShowError("Новый пароль и подтверждение не совпадают");
                return;
            }

            if (NewPasswordBox.Password == OldPasswordBox.Password)
            {
                ShowError("Новый пароль должен отличаться от старого");
                return;
            }

            try
            {
                AppConnect.currentUser.password = NewPasswordBox.Password;
                AppConnect.modelOdb.SaveChanges();

                MessageBox.Show("Пароль успешно изменен. Вы будете перенаправлены на страницу входа.", 
                               "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.Logout();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при смене пароля: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
