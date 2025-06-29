using pract.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pract
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.Initialize();
            NavigateToLogin();
        }

        public void NavigateToLogin()
        {
            MainFrame.Navigate(new Views.LoginPage());
        }

        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }

        public void Logout()
        {
            AppConnect.currentUser = null;

            NavigateToLogin();

            MessageBox.Show("Вы успешно вышли из аккаунта", "Выход", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
