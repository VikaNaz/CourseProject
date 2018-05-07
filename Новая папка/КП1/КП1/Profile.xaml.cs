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
using System.Security.Cryptography;

namespace КП1
{

    public partial class Profile : Window
    {
        Database DB = new Database();
        User Us;
        int i = 0;

        public Profile()
        {
            InitializeComponent();

        }

        public Profile(User U)
        {
            try
            {
                InitializeComponent();


                if (DB.Messages.Where(m => m.SenderID == U.ID).Count() != 0)
                    NameDialog.Loaded += NameDialog_Loaded;

                Us = U;
                xName.Content = U.Name + " " + U.LName;
                xEmail.Content = U.Email;
                xDR.Content = U.DR.Day.ToString() + "." + U.DR.Month.ToString() + "." + U.DR.Year.ToString();

                if (U.Photo != null)
                {
                    MemoryStream imageS = new MemoryStream(U.Photo);
                    System.Drawing.Image returnImagge = System.Drawing.Image.FromStream(imageS);
                    string path = Directory.GetCurrentDirectory() + U.ID.ToString();
                    returnImagge.Save(path);

                    Photo.Source = new BitmapImage(new Uri(path));
                }
                else
                    Photo.Source = new BitmapImage(new Uri("улыбка.png"));

                DB.Users.Where(u => u.ID != U.ID).Load();
                qwe.ItemsSource = DB.Users.Local;

                NameDialog.SelectionChanged += NameDialog_SelectionChanged;
                Message.Loaded += Message_Loaded;
                Prof.Loaded += Prof_Loaded;

                DB.Friends.Where(f => f.MyID == Us.ID).Load();
                ListBoxFriend.ItemsSource = DB.Friends.Local;

                
            }
            catch (Exception)
            {
                MessageBox.Show("Упс. Что-то пошло не так.");
            }
        }

        private void Prof_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                NameDialog.SelectedItem = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void Message_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                LoadMessageList();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void NameDialog_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
                NameDialog.ItemsSource = DB.Dialogs.Local;

                if (NameDialog.SelectedItem == NameDialog.Items[i])
                {
                    LoadMessageList();
                    i++;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }

        }

        private void NameDialog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (NameDialog.SelectedItem != null)
                {
                    LoadMessageList();
                    System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Tick += Timer_Tick;
                    timer.Interval = new TimeSpan(50000000);
                    timer.Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        ////
        private void WriteMessage(object sender, MouseButtonEventArgs e)
        {
            try
            {
                qwe.SelectionChanged += Qwe_SelectionChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }
        ////

        private void Dialog(User Receiver)
        {
            try
            {
                Dialog D1 = new Dialog { Receiver_ID = Receiver.ID, Sender_ID = Us.ID, Receiver_Name = Receiver.Name, Sender_Name = Us.Name };
                Dialog D2 = new Dialog { Sender_ID = Receiver.ID, Receiver_ID = Us.ID, Receiver_Name = Us.Name, Sender_Name = Receiver.Name };
                DB.Dialogs.Add(D1); DB.Dialogs.Add(D2);
                DB.SaveChanges();
                DB.Dispose();
                DB = new Database();
                DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
                NameDialog.ItemsSource = DB.Dialogs.Local;
                OpenMessages(Receiver.ID);
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void Qwe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();

                User U = qwe.SelectedItem as User;
                User Receiver = qwe.SelectedItem as User;
                if (DB.Dialogs.Count() == 0)
                {
                    Dialog(Receiver);
                }
                else
                {
                    int rec_ID = (qwe.SelectedItem as User).ID;
                    if (DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID == rec_ID).Count() == 0)
                    {
                        Dialog(Receiver);
                    }
                    else
                    {
                        NameDialog.SelectedItem = null;
                        MainTb.SelectedItem = Message;
                        List<Dialog> List = DB.Dialogs.Where(d => d.Receiver_ID == U.ID && d.Sender_ID == Us.ID).ToList();
                        foreach (Dialog d in List)
                        {
                            NameDialog.SelectedItem = d;
                        }
                    }
                }

                qwe.SelectionChanged -= Qwe_SelectionChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();
                DB.Users.Load();
                qwe.ItemsSource = DB.Users.Local;
                DB.Friends.Where(f => f.MyID == Us.ID).Load();
                ListBoxFriend.ItemsSource = DB.Friends.Local;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void OpenMessages(int id_receiver)
        {
            try
            {
                MainTb.SelectedItem = Message;
                foreach (Dialog d in NameDialog.Items)
                {
                    if (d.Receiver_ID == id_receiver)
                    {
                        NameDialog.SelectedItem = d;
                        Dialog dd = new Dialog();
                        dd = NameDialog.SelectedItem as Dialog;
                        DB.Messages.Where(m => m.Dialog.Receiver_ID == dd.Receiver_ID && m.Dialog.Sender_ID == dd.Sender_ID && m.Dialog.Receiver_ID != dd.Sender_ID && m.Dialog.Sender_ID != dd.Receiver_ID).Load();
                        _messagesList.ItemsSource = DB.Messages.Local;
                        NameDialog.SelectedItem = d;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void Send_Message(object sender, RoutedEventArgs e)
        {
            try
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
                        DB.Dispose();
                        DB = new Database();

                        Message M = new Message
                        {
                            SenderID = (NameDialog.SelectedItem as Dialog).Sender_ID,
                            ReceiverID = (NameDialog.SelectedItem as Dialog).Receiver_ID,
                            Message1 = MyMessage.Text
                        };
                        DB.Messages.Add(M);
                        DB.SaveChanges();
                        MyMessage.Clear();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }

        }

        private void LoadMessageList()
        {
            try
            {
                DB.Dispose();
                DB = new Database();
                if (NameDialog.SelectedItem != null)
                {
                    Dialog dd = (NameDialog.SelectedItem as Dialog);

                    DB.Messages.Where(m => (m.ReceiverID == dd.Receiver_ID || m.ReceiverID == dd.Sender_ID) && (m.SenderID == dd.Sender_ID || m.SenderID == dd.Receiver_ID)).OrderByDescending(o => o.ID).Load();


                    _messagesList.ItemsSource = DB.Messages.Local;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void DeleteMessage(object sender, MouseButtonEventArgs e)
        {
            _messagesList.SelectionChanged += _messagesList_SelectionChanged;
        }

        private void _messagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Message M = _messagesList.SelectedItem as Message;
                if (_messagesList.SelectedItem != null)
                {
                    DB.Messages.Remove(M);
                    DB.SaveChanges();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }

            _messagesList.SelectionChanged -= _messagesList_SelectionChanged;
        }

        private void ButtonSearch(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();
                DB.Users.Where(u => u.Name == TextBoxSearch.Text || u.LName == TextBoxSearch.Text || (u.Name + " " + u.LName) == TextBoxSearch.Text).Load();
                SearchUsers.ItemsSource = DB.Users.Local;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void AddFRiend(object sender, MouseButtonEventArgs e)
        {
            try
            {
                qwe.SelectionChanged += Qwe_SelectionChanged1;
                SearchUsers.SelectionChanged += Qwe_SelectionChanged1;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void AddFRiendFromSearch(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SearchUsers.SelectionChanged += SearchUsers_SelectionChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void SearchUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();

                User U = SearchUsers.SelectedItem as User;
                if (DB.Friends.Count() == 0)
                {
                    Friend F1 = new Friend { MyID = Us.ID, FriendID = U.ID, LName = U.LName, Name = U.Name, Photo = U.Photo };
                    DB.Friends.Add(F1);
                    DB.SaveChanges();
                    DB.Dispose();
                    DB = new Database();
                    MessageBox.Show("Пользователь добавлен в друзья :)");
                }
                else
                {
                    if (DB.Friends.Where(f => f.MyID == Us.ID && f.FriendID == U.ID).Count() == 0)
                    {
                        Friend F1 = new Friend { MyID = Us.ID, FriendID = U.ID, LName = U.LName, Name = U.Name, Photo = U.Photo };
                        DB.Friends.Add(F1);
                        DB.SaveChanges();
                        DB.Dispose();
                        DB = new Database();
                        MessageBox.Show("Пользователь добавлен в друзья :)");
                    }
                    else
                    {
                        MessageBox.Show("Этот пользователь уже есть у вас в друзьях.");
                    }
                }

                qwe.SelectionChanged -= Qwe_SelectionChanged1;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void Qwe_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();

                User U = qwe.SelectedItem as User;
                if (DB.Friends.Count() == 0)
                {
                    Friend F1 = new Friend { MyID = Us.ID, FriendID = U.ID, LName = U.LName, Name = U.Name, Photo = U.Photo };
                    DB.Friends.Add(F1);
                    DB.SaveChanges();
                    DB.Dispose();
                    DB = new Database();
                    MessageBox.Show("Пользователь добавлен в друзья :)");
                }
                else
                {
                    if (DB.Friends.Where(f => f.MyID == Us.ID && f.FriendID == U.ID).Count() == 0)
                    {
                        Friend F1 = new Friend { MyID = Us.ID, FriendID = U.ID, LName = U.LName, Name = U.Name, Photo = U.Photo };
                        DB.Friends.Add(F1);
                        DB.SaveChanges();
                        DB.Dispose();
                        DB = new Database();
                        MessageBox.Show("Пользователь добавлен в друзья :)");
                    }
                    else
                    {
                        MessageBox.Show("Этот пользователь уже есть у вас в друзьях.");
                    }
                }

                qwe.SelectionChanged -= Qwe_SelectionChanged1;
            }
            catch(Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void WriteMessageFromSearch(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SearchUsers.SelectionChanged += SearchUsers_SelectionChanged1;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void SearchUsers_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();
                
                User U = SearchUsers.SelectedItem as User;
                User Receiver = SearchUsers.SelectedItem as User;
                if (DB.Dialogs.Count() == 0)
                {
                    Dialog(Receiver);
                }
                else
                {
                    int rec_ID = (SearchUsers.SelectedItem as User).ID;
                    if (DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID == rec_ID).Count() == 0)
                    {
                        Dialog(Receiver);
                    }
                    else
                    {
                        NameDialog.SelectedItem = null;
                        MainTb.SelectedItem = Message;
                        List<Dialog> List = DB.Dialogs.Where(d => d.Receiver_ID == U.ID && d.Sender_ID == Us.ID).ToList();
                        foreach (Dialog d in List)
                        {
                            NameDialog.SelectedItem = d;
                        }
                    }
                }

                SearchUsers.SelectionChanged -= SearchUsers_SelectionChanged1;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void WriteMessageFromFriend(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ListBoxFriend.SelectionChanged += ListBoxFriend_SelectionChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }

        private void ListBoxFriend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DB.Dispose();
                DB = new Database();

                User U = ListBoxFriend.SelectedItem as User;
                User Receiver = ListBoxFriend.SelectedItem as User;
                if (DB.Dialogs.Count() == 0)
                {
                    Dialog D1 = new Dialog { Receiver_ID = Receiver.ID, Sender_ID = Us.ID, Receiver_Name = Receiver.Name, Sender_Name = Us.Name };
                    Dialog D2 = new Dialog { Sender_ID = Receiver.ID, Receiver_ID = Us.ID, Receiver_Name = Us.Name, Sender_Name = Receiver.Name };
                    DB.Dialogs.Add(D1); DB.Dialogs.Add(D2);
                    DB.SaveChanges();
                    DB.Dispose();
                    DB = new Database();
                    DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
                    NameDialog.ItemsSource = DB.Dialogs.Local;
                    OpenMessages(Receiver.ID);
                }
                else
                { 
                    int rec_ID = (ListBoxFriend.SelectedItem as User).ID;
                    if (DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID == rec_ID).Count() == 0)
                    {
                        Dialog D1 = new Dialog { Receiver_ID = Receiver.ID, Sender_ID = Us.ID, Receiver_Name = Receiver.Name, Sender_Name = Us.Name };
                        Dialog D2 = new Dialog { Sender_ID = Receiver.ID, Receiver_ID = Us.ID, Receiver_Name = Us.Name, Sender_Name = Receiver.Name };
                        DB.Dialogs.Add(D1); DB.Dialogs.Add(D2);
                        DB.SaveChanges();
                        DB.Dispose();
                        DB = new Database();
                        DB.Dialogs.Where(d => d.Sender_ID == Us.ID && d.Receiver_ID != Us.ID).Load();
                        NameDialog.ItemsSource = DB.Dialogs.Local;
                        OpenMessages(Receiver.ID);
                    }
                    else
                    {
                        NameDialog.SelectedItem = null;
                        MainTb.SelectedItem = Message;
                        List<Dialog> List = DB.Dialogs.Where(d => d.Receiver_ID == U.ID && d.Sender_ID == Us.ID).ToList();
                        foreach (Dialog d in List)
                        {
                            NameDialog.SelectedItem = d;
                        }
                    }
                }

                ListBoxFriend.SelectionChanged -= ListBoxFriend_SelectionChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка.\nПовторите попытку.");
            }
        }
    }
}


  