using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using Npgsql;

namespace HiTech
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        List<RoleId> roleid = new List<RoleId>();
        List<ProductInfo> product_info = new List<ProductInfo>();
        List<UserInfo> userInfo = new List<UserInfo>();
        List<User> user = new List<User>();

        public string id_us = "";
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Host=localhost;Username=root_user1;Password=root;Database=HiTech");
        }
        public Administrator()
        {
            InitializeComponent();
            roleid.Clear();
            XmlSerializer xml = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = File.OpenRead("username.xml"))
            {
                if (fs == null)
                {
                    user.Add(new User("gues1"));


                    using (FileStream fs1 = new FileStream("username.xml", FileMode.Create))
                    {

                        xml.Serialize(fs1, user);

                    }
                }
                else
                {
                    List<User> bufflist = new List<User>();
                    bufflist = xml.Deserialize(fs) as List<User>;
                    foreach (User a in bufflist)
                    {
                        user.Add(a);

                    }
                }
            }



        }


        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                string[] photos = (string[])e.Data.GetData(DataFormats.FileDrop);
                var filename = System.IO.Path.GetFullPath(photos[0]);
                fullput.Content = filename;
                BitmapImage my = new BitmapImage();
                my.BeginInit();
                my.UriSource = new Uri(filename);
                my.EndInit();
                namepic.Source = my;
                //photunredlist.Add(fullput.Content.ToString());
                //photid = photunredlist.Count;
            }
        }



        private void add_product(object sender, RoutedEventArgs e)
        {
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {




                    // select ADD_PRODUCT(2,'Samsung Galaxy A20',2000,'A20.img','Черный','Южная Корея','Это смартфон');
                    int category_id = 0;
                    string query = @"select ADD_PRODUCT(@1,@2,@3,@4,@5,@6,@7);";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                    if (categorybutton.Text.ToString() == "Смартфоны")
                    {
                        category_id = 1;
                    }
                    else if (categorybutton.Text.ToString() == "Часы")
                    {
                        category_id = 2;
                    }
                    else if (categorybutton.Text.ToString() == "Планшеты")
                    {
                        category_id = 3;
                    }
                    else if (categorybutton.Text.ToString() == "Браслеты")
                    {
                        category_id = 4;
                    }
                    else
                    {
                        MessageBox.Show("Ведите одну из категорий: Смартфон, Часы, Планшет, Браслет ");
                    }

                    cmd.Parameters.AddWithValue("@1", category_id);
                    cmd.Parameters.AddWithValue("@2", namebutton.Text);
                    cmd.Parameters.AddWithValue("@3", Convert.ToInt32(pricebutton.Text));
                    cmd.Parameters.AddWithValue("@4", fullput.Content);
                    cmd.Parameters.AddWithValue("@5", colorbutton.Text);
                    cmd.Parameters.AddWithValue("@6", countrybutton.Text);
                    cmd.Parameters.AddWithValue("@7", discrbuttton.Text);


                    con.Open();
                    int n = cmd.ExecuteNonQuery();

                    con.Close();
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.ToString());
            }
        }

        private void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    
                    string query = @"select * from Product where product_name=@1";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@1", numdel.Text);

                    con.Open();
                    var reated = cmd.ExecuteReader();
                    while (reated.Read()) {
                        a = reated.GetInt32(reated.GetOrdinal("product_id"));
                            }
                    con.Close();
                    string query_delete = @"call DELETE_PRODUCT(@1);";
                    NpgsqlCommand delete_cmd = new NpgsqlCommand(query_delete, con);
                    delete_cmd.Parameters.AddWithValue("@1", a);
                    con.Open();
                    var delete = delete_cmd.ExecuteNonQuery();
                    con.Close();



                }
            }
            catch (Exception h)
            {
                MessageBox.Show(h.ToString());
            }

        }

        private void vivod_button_Click(object sender, RoutedEventArgs e)
        {
         
            using (NpgsqlConnection con = GetConnection())
            {
                string query = @"select * from PRODUCT_INFO where product_name = @1;";
                product_info.Clear();
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@1", vivod.Text);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product_info.Add(new ProductInfo(
                        reader.GetString(reader.GetOrdinal("product_name")),
                        reader.GetInt32(reader.GetOrdinal("product_price")),
                        reader.GetString(reader.GetOrdinal("category_name")),
                        reader.GetString(reader.GetOrdinal("product_color")),
                        reader.GetString(reader.GetOrdinal("product_country")),
                        reader.GetString(reader.GetOrdinal("product_description"))
                        ));
                }
                foreach(var a in product_info)
                {
                    namebutton.Text = a.Name;
                    pricebutton.Text =  a.Price.ToString();
                    categorybutton.Text = a.Category;
                    colorbutton.Text = a.Color;
                    countrybutton.Text = a.Country;
                    discrbuttton.Text = a.Descriprion;
                }
                con.Close();
            }
            }

        private void zamena_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            int b = 0;
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {

                    string query = @"select * from Product where product_name=@1";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@1", vivod.Text);

                    con.Open();
                    var reated = cmd.ExecuteReader();
                    while (reated.Read())
                    {
                        a = reated.GetInt32(reated.GetOrdinal("product_id"));
                        b = reated.GetInt32(reated.GetOrdinal("category_id"));

                    }
                    con.Close();

                    string query_changed = @"call UPDATE_PRODUCT(@1,@2,@3,@4,@5,@6,@7,@8)";
                    NpgsqlCommand changed_cmd = new NpgsqlCommand(query_changed, con);
                    changed_cmd.Parameters.AddWithValue("@1", a);
                    changed_cmd.Parameters.AddWithValue("@2", b);
                    changed_cmd.Parameters.AddWithValue("@3", namebutton.Text);
                    changed_cmd.Parameters.AddWithValue("@4", Convert.ToInt32( pricebutton.Text));
                    changed_cmd.Parameters.AddWithValue("@5", fullput.Content);
                    changed_cmd.Parameters.AddWithValue("@6", colorbutton.Text);
                    changed_cmd.Parameters.AddWithValue("@7", countrybutton.Text);
                    changed_cmd.Parameters.AddWithValue("@8", discrbuttton.Text);

                    con.Open();
                int g= changed_cmd.ExecuteNonQuery();
                    con.Close();



                }
            }
            catch (Exception h)
            {
                MessageBox.Show(h.ToString());
            }
        }

     

        private void delete_user_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {

                    string get_id = @"select * from Users where user_name =@1";
                    NpgsqlCommand cmd = new NpgsqlCommand(get_id, con);
                    cmd.Parameters.AddWithValue("@1", deluser_name.Text);

                    con.Open();
                    var reated = cmd.ExecuteReader();
                    while (reated.Read())
                    {
                        a = reated.GetInt32(reated.GetOrdinal("user_id"));

                    }
                    con.Close();


                    string query = @"call DELETE_USER(@1);";
                    NpgsqlCommand delete_cmd = new NpgsqlCommand(query, con);
                    delete_cmd.Parameters.AddWithValue("@1", a);





                    con.Open();
                    int n = delete_cmd.ExecuteNonQuery();

                    con.Close();
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.ToString());
            }
        }
        private void calendar_SelectedDatesChanged2(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar2.SelectedDate;

            MessageBox.Show(selectedDate.Value.Date.ToString("yyyy-MM-dd"));

        }

        private void vivod_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    id_us = text_user.Text;
                    string query = @"select * from SEARCH_USER_BY_NAME(@1);";
                    product_info.Clear();
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@1", text_user.Text);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userInfo.Add(new UserInfo(
                            reader.GetString(reader.GetOrdinal("user_name")),
                            reader.GetDateTime(reader.GetOrdinal("user_date")),
                            reader.GetString(reader.GetOrdinal("user_mail"))
                            ));
                    }
                    foreach (var a in userInfo)
                    {
                        user_name_box.Text = a.Name.ToString();
                        user_mail_box.Text = a.Mail.ToString();
                       calendar2.SelectedDate = a.Date;

                    }
                    con.Close();
                }
            }catch(Exception j)
            {
                MessageBox.Show(j.ToString());
            }
        }

        private void zamena_user_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = calendar2.SelectedDate;
            int a = 0;
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    string get_id = @"select * from Users where user_name =@1";
                    NpgsqlCommand cmd = new NpgsqlCommand(get_id, con);
                    cmd.Parameters.AddWithValue("@1", id_us);

                    con.Open();
                    var reated = cmd.ExecuteReader();
                    while (reated.Read())
                    {
                        a = reated.GetInt32(reated.GetOrdinal("user_id"));

                    }
                    con.Close();
                    string query = @"call UPDATE_USER(@1,@2,to_date(@3,'YYYY-MM-DD'),@4)";
                    NpgsqlCommand change_cmd = new NpgsqlCommand(query, con);
                    change_cmd.Parameters.AddWithValue("@1",a);

                    change_cmd.Parameters.AddWithValue("@2", user_name_box.Text);
                    change_cmd.Parameters.AddWithValue("@4", user_mail_box.Text); ;
                    change_cmd.Parameters.AddWithValue("@3", selectedDate.Value.Date.ToString("yyyy-MM-dd"));





                    con.Open();
                    int n = change_cmd.ExecuteNonQuery();

                    con.Close();

                }
                

            }

            catch (Exception g)
            {
                MessageBox.Show(g.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }


    public class ProductInfo
    {
        public ProductInfo() { }
        public ProductInfo(string product_name_arg, int product_price_arg, string category_name_arg, string product_color_arg, string product_country_arg, string product_description_arg)
        {
            Name = product_name_arg;
            Price = product_price_arg;
            Category = category_name_arg;
            Color = product_color_arg;
            Country = product_country_arg;
            Descriprion = product_description_arg;

        }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Country { get; set; }
        public string Descriprion { get; set; }



    }
    public class UserInfo
    {
        public UserInfo() { }
        public UserInfo(string product_name_arg, DateTime user_date_arg, string user_mail_arg)
        {
            Name = product_name_arg;
            Date = user_date_arg;
            Mail = user_mail_arg;
          
        }
        public UserInfo(string product_name_arg)
        {
            Name = product_name_arg;
            

        }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Mail { get; set; }
      


    }


}





