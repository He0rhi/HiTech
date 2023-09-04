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
    public partial class Manager : Window
    {
        List<RoleId> roleid = new List<RoleId>();
        List<ProductInfo> product_info = new List<ProductInfo>();
        List<UserInfo> userInfo = new List<UserInfo>();
        List<User> user = new List<User>();

        public string id_us = "";
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Host=localhost;Username=manager_user1;Password=12345;Database=HiTech");
        }
        public Manager()
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



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

    }


   
   


}





