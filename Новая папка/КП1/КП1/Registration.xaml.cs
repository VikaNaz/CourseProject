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
using System.Security.Cryptography;

namespace КП1
{

    public partial class Registration : Window
    {
        Database DB = new Database();

        public Registration()
        {
            InitializeComponent();

        }

        public void Registr(object sender, RoutedEventArgs e)
        {
            try
            {
                User U = new User();
                Us(U);

                
            }
        catch(Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }        

        private static byte[] ConvertImageToByteArray(string fileNameS)
        {
            //try
            //{
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
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            //}
        }

        void Us(User Prof)
        {
           try
           {
                Regex r1 = new Regex(@"[А-Яа-я]");
                
                if (r1.IsMatch(regName.Text))  Prof.Name = regName.Text;
                else
                {
                    throw new Exception("Неправильно введено имя!");
                }

                if (r1.IsMatch(regLName.Text)) Prof.LName = regLName.Text;
                else
                {
                    throw new Exception("Неправильно введена фамилия!");
                }

                Prof.Email = regEmail.Text;
                


                Prof.DR = Convert.ToDateTime(regDR.Text);
                if (regPhoto.Source == null)
                {
                    Prof.Photo = ConvertImageToByteArray("..\\..\\Улыбка.png");
                }
                else
                {
                    Prof.Photo = ConvertImageToByteArray(Convert.ToString(regPhoto.Source));
                }


                if (regM.IsChecked == true)
                    Prof.Gender = false;
                else
                    Prof.Gender = true;

                Prof.Log_in.Add(new Log_in
                {
                    Login = regLogin.Text,
                    Password = Hash.EncryptPassword(regLogin.Text, regPassword.Password),
                    ID = Prof.ID
                });

                DB.Users.Add(Prof);
                DB.SaveChanges();
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void open_ButonClick(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }
    }
}
    