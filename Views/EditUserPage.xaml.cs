using pract.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pract.Views
{
    public partial class EditUserPage : Page
    {
        public event Action UserUpdated;
        private Users _currentUser;

        public EditUserPage(Users user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadRoles();
            LoadUserData();
        }

        private void LoadRoles()
        {
            try
            {
                var roles = AppConnect.modelOdb.Roles.ToList();
                RoleComboBox.ItemsSource = roles;
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке ролей: {ex.Message}");
            }
        }

        private void LoadUserData()
        {
            if (_currentUser != null)
            {
                LoginTextBox.Text = _currentUser.login;
                RoleComboBox.SelectedValue = _currentUser.roleId;
            }
        }

        private void ResetPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordPanel.Visibility = Visibility.Visible;
        }

        private void ResetPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordPanel.Visibility = Visibility.Collapsed;
            NewPasswordBox.Password = "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            HideError();

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || RoleComboBox.SelectedItem == null)
            {
                ShowError("Заполните все обязательные поля");
                return;
            }

            if (ResetPasswordCheckBox.IsChecked == true && string.IsNullOrWhiteSpace(NewPasswordBox.Password))
            {
                ShowError("Введите новый пароль");
                return;
            }

            try
            {
                if (_currentUser.login != LoginTextBox.Text)
                {
                    var existingUser = AppConnect.modelOdb.Users
                        .FirstOrDefault(u => u.login == LoginTextBox.Text && u.id != _currentUser.id);

                    if (existingUser != null)
                    {
                        ShowError("Пользователь с таким логином уже существует");
                        return;
                    }
                }

                _currentUser.login = LoginTextBox.Text;
                _currentUser.roleId = (int)RoleComboBox.SelectedValue;

                if (ResetPasswordCheckBox.IsChecked == true)
                {
                    _currentUser.password = NewPasswordBox.Password;
                }

                AppConnect.modelOdb.SaveChanges();

                MessageBox.Show("Данные пользователя успешно обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                UserUpdated?.Invoke();

                GoBack();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при обновлении пользователя: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateTo(new AdminPage());
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
