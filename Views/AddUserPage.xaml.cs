using pract.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pract.Views
{
    public partial class AddUserPage : Page
    {
        public event Action UserAdded;

        public AddUserPage()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                var roles = AppConnect.modelOdb.Roles.ToList();
                RoleComboBox.ItemsSource = roles;
                
                if (roles.Any())
                {
                    RoleComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке ролей: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            HideError();

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                RoleComboBox.SelectedItem == null)
            {
                ShowError("Заполните все поля");
                return;
            }

            try
            {
                var existingUser = AppConnect.modelOdb.Users
                    .FirstOrDefault(u => u.login == LoginTextBox.Text);

                if (existingUser != null)
                {
                    ShowError("Пользователь с таким логином уже существует");
                    return;
                }

                var newUser = new Users
                {
                    login = LoginTextBox.Text,
                    password = PasswordBox.Password,
                    roleId = (int)RoleComboBox.SelectedValue,
                    lastlogin = null,
                    isBlocked = false,
                    loginAttempts = 0
                };

                AppConnect.modelOdb.Users.Add(newUser);
                AppConnect.modelOdb.SaveChanges();

                MessageBox.Show("Пользователь успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                UserAdded?.Invoke();

                GoBack();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при добавлении пользователя: {ex.Message}");
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
