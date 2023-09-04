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
using System.IO;
using System.Xml.Serialization;
using Npgsql;
namespace HiTech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Host=localhost;Username=guest_user1;Password=12345;Database=HiTech");
        }
        public MainWindow()
        {

            InitializeComponent();
        }
        List<User> user = new List<User>();
        List<RoleId> roleid = new List<RoleId>();
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;
            user.Clear();
            try
            {

                using (NpgsqlConnection con = GetConnection())
                {

                    string query = @"select REGISTRATION(@1,@2,@3,to_date(@4,'YYYY-MM-DD'))";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@1", name_arg.Text);
                    cmd.Parameters.AddWithValue("@2", mail_arg.Text);
                    cmd.Parameters.AddWithValue("@3", password_arg.Text);
                    cmd.Parameters.AddWithValue("@4", selectedDate.Value.Date.ToString("yyyy-MM-dd"));





                    con.Open();
                    int n = cmd.ExecuteNonQuery();

                    con.Close();

                }
                user.Add(new User(name_arg.Text.ToString()));
                XmlSerializer xml1 = new XmlSerializer(typeof(List<User>));

                using (FileStream fs = new FileStream("username.xml", FileMode.Create))
                {
                   
                        xml1.Serialize(fs, user);
                    
                }
                user.Clear();
                user.Add(new User(name_arg.Text.ToString(), mail_arg.Text.ToString(), password_arg.Text.ToString(), selectedDate.Value.Date.ToString("yyyy-MM-dd")));
                MainWindow mainWindow = new MainWindow();
                UserWindow userwin = new UserWindow();

                userwin.Show();
                mainWindow.Close();
            }

            catch (Exception g)
            {
                MessageBox.Show(g.ToString());
            }

        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;

            MessageBox.Show(selectedDate.Value.Date.ToString("yyyy-MM-dd"));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SIGNIN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                user.Clear();
                using (NpgsqlConnection con = GetConnection())
                {

                    string query = @"select SIGNIN(@1,@2)";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@1", signname.Text);
                    cmd.Parameters.AddWithValue("@2", signpassword.Text);
                    user.Clear();
                    con.Open();
                    var reader = cmd.ExecuteScalar();

                    roleid.Add(new RoleId(signname.Text.ToString(),Convert.ToInt32( reader)));


                    con.Close();

                }
                using (NpgsqlConnection con = GetConnection())
                {

                    string query = @"select * from Users where user_mail = @1";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@1", signname.Text);
                    user.Clear();
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user.Add(new User(reader.GetString(reader.GetOrdinal("user_name"))));
                        roleid.Add(new RoleId(reader.GetInt32(reader.GetOrdinal("user_role"))));

                    }


                    con.Close();

                }

                XmlSerializer xml1 = new XmlSerializer(typeof(List<User>));

                
                using (FileStream fs = new FileStream("username.xml", FileMode.Create))
                {
                   
                        xml1.Serialize(fs, user);
                    
                }
                user.Clear();
                foreach (var a in roleid)
                {
                    if (a.role_id == 2)
                    {
                        Manager managerwindow = new Manager();

                        managerwindow.Show();
                        Close();
                        break;                    }
                    if (a.role_id == 1)
                    {
                        Administrator administrator = new Administrator();

                        administrator.Show();
                        Close();
                        break;
                    }
                    if (a.role_id == 3)
                    {
                        UserWindow userwi = new UserWindow();
                        userwi.Show();
                        Close();
                        break;
                    }
                  
                }


            }

            catch (Exception g)
            {
                MessageBox.Show(g.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserWindow1 userWindow1 = new UserWindow1();
            this.Close();
            userWindow1.Show();
        }
    }
    public class User
    {
        public User() { }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Date { get; set; }


        public User(string name_arg, string mail_arg, string password_arg, string date_arg)
        {
            Name = name_arg;
            Mail = mail_arg;
            Password = password_arg;
            Date = date_arg;
        }
        public User(string name_arg, string mail_arg, string date_arg)
        {
            Name = name_arg;
            Mail = mail_arg;
            Date = date_arg;
        }
        public User(string name_arg, string user_mail)
        {
            Name = name_arg;
            Mail = user_mail;
        }
        public User(string name_arg)
        {
            Name = name_arg;
        }
    }
    public class RoleId
    {
        public RoleId() { }
        public int role_id { get; set; }
        public string mail { get; set; }

        public RoleId(string mail_arg, int role_arg)
        {
            mail = mail_arg;
            role_id = role_arg;
        }
        public RoleId(int role_arg)
        {
            role_id = role_arg;
        }
    }




}
