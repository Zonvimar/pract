using pract.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pract.Views
{
    public partial class LoginPage : Page
    {
        private int currentAttempts = 0;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            HideError();

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowError("Заполните все поля");
                return;
            }

            try
            {
                var user = AppConnect.modelOdb.Users
                    .Include("Roles")
                    .FirstOrDefault(u => u.login == LoginTextBox.Text);

                if (user == null)
                {
                    HandleFailedLogin(null);
                    return;
                }

                if (user.isBlocked)
                {
                    ShowError("Пользователь заблокирован, дождитесь разблокировки от администратора");
                    return;
                }

                if (user.lastlogin.HasValue)
                {
                    var lastLogin = user.lastlogin.Value;
                    var oneMonthAgo = DateTime.Now.AddMonths(-1);
                    
                    if (lastLogin < oneMonthAgo)
                    {
                        user.isBlocked = true;
                        AppConnect.modelOdb.SaveChanges();
                        ShowError("Пользователь заблокирован (не входил более месяца)");
                        return;
                    }
                }

                if (user.password != PasswordBox.Password)
                {
                    user.loginAttempts++;
                    
                    if (user.loginAttempts >= 3)
                    {
                        user.isBlocked = true;
                    }
                    
                    AppConnect.modelOdb.SaveChanges();
                    HandleFailedLogin(user);
                    return;
                }

                user.lastlogin = DateTime.Now;
                user.loginAttempts = 0;
                AppConnect.modelOdb.SaveChanges();
                
                AppConnect.currentUser = user;

                if (user.Roles.name == "Пользователь")
                {
                    NavigateToPage(new ChangePasswordPage());
                }
                else if (user.Roles.name == "Администратор")
                {
                    NavigateToPage(new AdminPage());
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при входе: {ex.Message}");
            }
        }

        private void HandleFailedLogin(Users user)
        {
            if(user != null)
            {
                currentAttempts++;
            }

            if (currentAttempts >= 3)
            {
                ShowError("Превышено количество попыток входа");
            }
            else
            {
                if (user != null)
                {
                    ShowError($"Неверный логин или пароль. Попытка {currentAttempts} из 3");
                }
                else
                {
                    ShowError("Пользователь не найден, проверьте правильность введеного логина");
                }
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

        private void NavigateToPage(Page page)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateTo(page);
        }
    }
}
