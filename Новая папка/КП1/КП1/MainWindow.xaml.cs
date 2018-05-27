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
using System.Security.Cryptography;

namespace КП1
{
    public partial class MainWindow : Window
    {
        Database DB = new Database();
        

        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        public void Registr(object sender, RoutedEventArgs e)
        {
            try
            {
                Registration RegistrationForm = new Registration();
                RegistrationForm.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        public void Log_in(object sender, RoutedEventArgs e)
        {
            try
            {
                bool Enter = false;

                foreach (Log_in L in DB.Log_in.Where(l => l.Login == LoginEnter.Text))
                {
                    if (Hash.PasswordsMatch(Hash.EncryptPassword(LoginEnter.Text, PasswordEnter.Password), L.Password))
                        {
                        foreach (User U in DB.Users.Where(u => u.ID == L.ID))
                        {
                            Enter = true;
                            Profile Profile = new Profile(U);
                            this.Close();
                            Profile.Show();
                            break;
                        }
                        break;
                    }
                }                
                if (Enter == true)
                    Close();
                else
                    MessageBox.Show("Неверный логин или пароль :(");                
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
               this.DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
