using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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


namespace КП1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        MyDatabase DB = new MyDatabase();
        public MainWindow()
        {
            InitializeComponent();
        }
        public void Registr(object sender, RoutedEventArgs e)
        {
            Registration RegistrationForm = new Registration();
            RegistrationForm.Show();
        }
        public void Log_in(object sender, RoutedEventArgs e)
        {
            try
            {
                bool Enter = false;
                foreach (Log_in L in DB.Log_in.Where(l => l.Login == LoginEnter.Text && l.Password == PasswordEnter.Password))
                {
                    foreach (User U in DB.User.Where(u => u.ID == L.ID))
                    {
                        Enter = true;
                        Profile Profile = new Profile(U);
                        this.Close();
                        Profile.Show();
                        break;
                    }
                    break;
                }
                if (Enter == true)
                    Close();
                else
                    MessageBox.Show("Неверный логин или пароль :(");
            }
            catch
            {

            }
        }
    }
}
