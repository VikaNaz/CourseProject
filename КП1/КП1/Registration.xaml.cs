using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace КП1
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        MyDatabase DB = new MyDatabase();

        public Registration()
        {
            InitializeComponent();

        }

        public void Registr(object sender, RoutedEventArgs e)
        {
            User U = new User();
            Us(U);

            // Log_in L = new Log_in();
            // Log(L);

            Close();
        }
        

        private static byte[] ConvertImageToByteArray(string fileNameS)
        {
            string fileName = "";
            Regex r = new Regex(@"file:///");
            if (r.IsMatch(fileNameS))
            {
                int i = fileNameS.Length;

                for (int j = 8; j < i; j++)
                    fileName += fileNameS[j];
            }
            else fileName = fileNameS;
            Bitmap bitMap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitMap.RawFormat;
            var imageToConvert = System.Drawing.Image.FromFile(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, bmpFormat);
                return ms.ToArray();
            }
        }

        void Us(User Prof)
        {
            //List<User> List = DB.Users.ToList();

            Prof.Name = regName.Text;
            Prof.LName = regLName.Text;
            Prof.Email = regEmail.Text;
            Prof.DR = Convert.ToDateTime(regDR.Text);
            //Prof.DR = Convert.ToDateTime(regDR);
            if (regPhoto.Source == null)
            {
                Prof.Photo = ConvertImageToByteArray("..\\..\\Улыбка.png");
            }
            else
            {
                Prof.Photo = ConvertImageToByteArray(Convert.ToString(regPhoto.Source));
            }
            

            if (regM.IsChecked == true)
                //DB.Users.Add(new User { Name = regName.Text, LName = regLName.Text, Email = regEmail.Text, DR = Convert.ToDateTime(regDR.Text), Gender = false });
                Prof.Gender = false;
            else
                //DB.Users.Add(new User { Name = regName.Text, LName = regLName.Text, Email = regEmail.Text, DR = Convert.ToDateTime(regDR.Text), Gender = true });
                Prof.Gender = true;

            Prof.Log_in.Add(new Log_in { Login = regLogin.Text, Password = regPassword.Password, TCP = "1", ID = Prof.ID });

            DB.User.Add(Prof);
            DB.SaveChanges();
        }

        private void open_ButonClick(object sender, RoutedEventArgs e)
        {
            string Path;
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF" + "|Все файлы (*.*)|*.* ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                Path = myDialog.FileName;
                regPhoto.Source = new BitmapImage(new Uri(Path));                
            }
            
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
    