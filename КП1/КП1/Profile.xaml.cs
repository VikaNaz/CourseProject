using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Controls;
using System.Data.Entity;
using System.Threading;

//using System.Windows.Controls;

namespace КП1
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        MyDatabase DB = new MyDatabase();
        User Us;
        int i = 0;

        public Profile()
        {
            InitializeComponent();

        }

        public Profile(User U)
        {
            InitializeComponent();



            if (DB.Messages.Where(m => m.Sender_ID == U.ID).Count() != 0)
                NameDialog.Loaded += NameDialog_Loaded;

            Us = U;
            xName.Content = U.Name + " " + U.LName;
            xEmail.Content = U.Email;
            xDR.Content = U.DR.ToString();

            if (U.Photo != null)
            {
                MemoryStream imageS = new MemoryStream(U.Photo);
                System.Drawing.Image returnImagge = System.Drawing.Image.FromStream(imageS);
                string path = Directory.GetCurrentDirectory() + U.ID.ToString();
                returnImagge.Save(path);

                Photo.Source = new BitmapImage(new Uri(path));
            }
            //else
            //Photo.Source = new BitmapImage(new Uri("улыбка.png"));

            DB.User.Where(u => u.ID != U.ID).Load();
            qwe.ItemsSource = DB.User.Local;

            NameDialog.SelectionChanged += NameDialog_SelectionChanged;
            Message.Loaded += Message_Loaded;
            Prof.Loaded += Prof_Loaded;


        }

        private void Prof_Loaded(object sender, RoutedEventArgs e)
        {
            NameDialog.SelectedItem = null;
        }

        private void Message_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(50000000);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadMessageList();
        }

        private void NameDialog_Loaded(object sender, RoutedEventArgs e)
        {
            DB.Dialog.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
            NameDialog.ItemsSource = DB.Dialog.Local;
            //NameDialog.SelectedItem = NameDialog.Items[0];

            if (NameDialog.SelectedItem == NameDialog.Items[i])
            {
                LoadMessageList();
                i++;
            }

        }

        private void NameDialog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NameDialog.SelectedItem != null) LoadMessageList();
        }

        public void SendMes(User U)
        {
            //List<User> List = DB.Users.ToList();
            //User U = List[EntID - 1];
            TabItem Ti = new TabItem();
            Ti.Height = 30; Ti.Width = 40;
            Ti.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);
            Ti.Header = U.Name;

            TextBox Tx = new TextBox();
            Tx.Text = "Lol Kek Chebureck !";
            Tx.Height = 30;

            Ti.Content = Tx;
            //TabC.Items.Add(Ti);
            MainTb.SelectedItem = Message;
            //TabC.SelectedItem = Ti;
        }

        ////
        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            qwe.SelectionChanged += Qwe_SelectionChanged;
        }
        ////

        private void Dialog(User Receiver)
        {
            Dialog D1 = new Dialog { Receiver_ID = Receiver.ID, Sender_ID = Us.ID, Receiver_Name = Receiver.Name };
            Dialog D2 = new Dialog { Sender_ID = Receiver.ID, Receiver_ID = Us.ID, Receiver_Name = Us.Name };
            DB.Dialog.Add(D1); DB.Dialog.Add(D2);
            DB.SaveChanges();
            DB.Dispose();
            DB = new MyDatabase();
            DB.Dialog.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
            NameDialog.ItemsSource = DB.Dialog.Local;
            OpenMessages(Receiver.ID);
        }

        private void Qwe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DB.Dispose();
            DB = new MyDatabase();

            
            User Receiver = qwe.SelectedItem as User;
            if (DB.Dialog.Count() == 0)
            {
                Dialog(Receiver);
            }
            else
            {
                /////// NIKTO NE VIBIRAETSYA   сделать два условия, если сообщения в диалоге уже естьи если нет
                int rec_ID = (qwe.SelectedItem as User).ID;
                if (DB.Dialog.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID == rec_ID).Count() == 0)
                {
                    Dialog(Receiver);
                }
                else
                //OpenMessages((qwe.SelectedItem as User).ID);
                {
                    NameDialog.SelectedItem = null;
                    MainTb.SelectedItem = Message;
                    DB = new MyDatabase();
                    DB.Dialog.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
                    NameDialog.ItemsSource = DB.Dialog.Local;
                }
            }
            qwe.SelectionChanged -= Qwe_SelectionChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DB.Dispose();
            DB = new MyDatabase();
            DB.User.Load();
            qwe.ItemsSource = DB.User.Local;
        }

        private void OpenMessages(int id_receiver)
        {
            MainTb.SelectedItem = Message;
            foreach (Dialog d in NameDialog.Items)
            {
                if (d.Receiver_ID == id_receiver)
                {
                    NameDialog.SelectedItem = d;
                    Dialog dd = new Dialog();
                    dd = NameDialog.SelectedItem as Dialog;
                    DB.Messages.Where(m => m.DialogID == dd.ID).Load();
                    _messagesList.ItemsSource = DB.Messages.Local;
                    NameDialog.SelectedItem = d;
                    //break;
                }
            }
        }

        private void Send_Message(object sender, RoutedEventArgs e)
        {
            if (NameDialog.SelectedItem == null)
            {
                MessageBox.Show("Выберите диалог!");
            }
            else
            {
                if (MyMessage.Text == null)
                    MessageBox.Show("Введите сообщение!");
                else
                {
                    Messages M = new Messages
                    {
                        Sender_ID = (NameDialog.SelectedItem as Dialog).Sender_ID,
                        Receiver_ID = (NameDialog.SelectedItem as Dialog).Receiver_ID,
                        Message = MyMessage.Text,
                        DialogID = (NameDialog.SelectedItem as Dialog).ID
                    };

                    DB.Messages.Add(M);
                    DB.SaveChanges();
                    MyMessage.Clear();
                }
            }

        }

        private void LoadMessageList()
        {

            DB.Dispose();
            DB = new MyDatabase();
            Dialog dd = (NameDialog.SelectedItem as Dialog);

            DB.Messages.Where(m => m.DialogID == dd.ID);
            //DB.Messages.Where(m => (m.ReceiverID == dd.Receiver_ID && m.SenderID == dd.Sender_ID) || (m.ReceiverID == dd.Sender_ID && m.SenderID == dd.Receiver_ID)).Load();
           // UnloadedEvent(dd);
            _messagesList.ItemsSource = DB.Messages.Local;

        }

        private void DeleteMessage(object sender, MouseButtonEventArgs e)
        {
            if (_messagesList.SelectedItem == null)
                MessageBox.Show("Выберите сообщение для удаления.");
            else
            {
                Messages M = _messagesList.SelectedItem as Messages;
                DB.Messages.Remove(M);
                DB.SaveChanges();
            }
        }
    
    }
}
