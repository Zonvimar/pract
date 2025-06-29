using pract.ApplicationData;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace pract.Views
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadUsers();
            SetWelcomeMessage();
        }

        private void SetWelcomeMessage()
        {
            if (AppConnect.currentUser != null)
            {
                WelcomeTextBlock.Text = $"Добро пожаловать, {AppConnect.currentUser.login}!";
            }
        }

        private void LoadUsers()
        {
            try
            {
                var users = AppConnect.modelOdb.Users.Include("Roles").ToList();
                UsersListView.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelection = UsersListView.SelectedItem != null;
            EditUserButton.IsEnabled = hasSelection;
            
            if (hasSelection && UsersListView.SelectedItem is Users selectedUser)
            {
                UnblockUserButton.IsEnabled = selectedUser.isBlocked;
            }
            else
            {
                UnblockUserButton.IsEnabled = false;
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var addUserPage = new AddUserPage();
            addUserPage.UserAdded += () => LoadUsers();
            NavigateToPage(addUserPage);
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListView.SelectedItem is Users selectedUser)
            {
                var editUserPage = new EditUserPage(selectedUser);
                editUserPage.UserUpdated += () => LoadUsers();
                NavigateToPage(editUserPage);
            }
        }

        private void UnblockUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListView.SelectedItem is Users selectedUser)
            {
                var result = MessageBox.Show($"Снять блокировку с пользователя \"{selectedUser.login}\"?", 
                                           "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        selectedUser.isBlocked = false;
                        selectedUser.loginAttempts = 0;
                        AppConnect.modelOdb.SaveChanges();
                        
                        MessageBox.Show("Пользователь разблокирован", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при разблокировке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.Logout();
        }

        private void NavigateToPage(Page page)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateTo(page);
        }
    }
}
