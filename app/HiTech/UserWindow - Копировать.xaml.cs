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
using System.Linq;
namespace HiTech
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow1 : Window
    {
       public List<User> user1 = new List<User>();
        public List<ProductAll> tovarinfo = new List<ProductAll>();
        public List<Basket> basket = new List<Basket>();
        public List<ProductAll> productAll = new List<ProductAll>();
        int ot = 0;
         string query_buff="";
        string category_buff = "";
        string search_buff = "";
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Host=localhost;Username=user1;Password=12345;Database=HiTech");
        }
        public UserWindow1()
        {
            InitializeComponent();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = File.OpenRead("username.xml"))
                {
                    if (fs == null)
                    {
                        user1.Add(new User("gues1"));


                        using (FileStream fs1 = new FileStream("username.xml", FileMode.Create))
                        {

                            xml.Serialize(fs1, user1);

                        }
                    }
                    else
                    {
                        List<User> bufflist = new List<User>();
                        bufflist = xml.Deserialize(fs) as List<User>;
                        foreach (User a in bufflist)
                        {
                            user1.Add(a);

                        }
                    }
                }

                using (NpgsqlConnection con = GetConnection())
                {
                    string query_all_tovars = "select * from product order by product_name offset @1 rows fetch next 8 rows only;";

                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);

                    all_tovars_cmd.Parameters.AddWithValue("@1", ot);



                    con.Open();
                    var reader = all_tovars_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                    }
                    con.Close();
                }


                int ml = 0;
                int ml2 = 0;
                int id_first = 0;
                int count_info = tovarinfo.Count;
                foreach (var a in tovarinfo)
                {

                    var f = a.IMG;
                    if (ml <= 3)
                    {
                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                     
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);


                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);
                        grid.MaxHeight = 250;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);

                        grid.Children.Add(stack);
                        products1.Children.Add(grid);

                        Grid.SetColumn(grid, ml);

                        ml++;
                    }
                    else if (ml > 3)
                    {
                        VisualStateManager visualStateManager = new VisualStateManager();

                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                
                      
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);

                        grid.MaxHeight = 250;
                        grid.MaxWidth = 170;

                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);

                        grid.Children.Add(stack);



                        products2.Children.Add(grid);
                        Grid.SetColumn(grid, ml2);

                        ml2++;

                    }
                }

            }catch(Exception g)
            {
                MessageBox.Show(g.ToString());
            }
            




        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();

                ot = ot - 8;
                if (ot < 0)
                {
                    ot = 0;
                }
                if (query_buff == "")
                {
                    using (NpgsqlConnection con = GetConnection())
                    {
                        string query_all_tovars = "select * from product order by product_name offset @1 rows fetch next 8 rows only;";

                        NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                        all_tovars_cmd.Parameters.AddWithValue("@1", ot);

                        con.Open();
                        var reader = all_tovars_cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                        }
                    }


                    int ml = 0;
                    int ml2 = 0;

                    foreach (var a in tovarinfo)
                    {

                        var f = a.IMG;
                        if (ml <= 3)
                        {
                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                           
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);
                            grid.MaxHeight = 250;
                            grid.MaxWidth = 170;
                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);

                            grid.Children.Add(stack);
                            products1.Children.Add(grid);

                            Grid.SetColumn(grid, ml);

                            ml++;
                        }
                        else if (ml > 3)
                        {
                            VisualStateManager visualStateManager = new VisualStateManager();

                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                          
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);

                            grid.MaxHeight = 250;
                            grid.MaxWidth = 170;

                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);
                            grid.Children.Add(stack);
                          



                            products2.Children.Add(grid);
                            Grid.SetColumn(grid, ml2);

                            ml2++;

                        }
                    }
                }
                else if (query_buff.Contains("SEARCH_PRODUCT_BY_CATEGORY"))
                {
                    products1.Children.Clear();
                    products2.Children.Clear();
                    tovarinfo.Clear();


                    using (NpgsqlConnection con = GetConnection())
                    {
                        string query_all_tovars = "select * from SEARCH_PRODUCT_BY_CATEGORY(@1) offset @2 rows fetch next 8 rows only;";
                        query_buff = query_all_tovars;
                        NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                        all_tovars_cmd.Parameters.AddWithValue("@1", category_buff);
                        all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                        con.Open();
                        var reader = all_tovars_cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                        }
                    }


                    int ml = 0;
                    int ml2 = 0;

                    foreach (var a in tovarinfo)
                    {

                        var f = a.IMG;
                        if (ml <= 3)
                        {
                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);
                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;
                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);

                            grid.Children.Add(stack);
                            products1.Children.Add(grid);

                            Grid.SetColumn(grid, ml);

                            ml++;
                        }
                        else if (ml > 3)
                        {
                            VisualStateManager visualStateManager = new VisualStateManager();

                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);

                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;

                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);

                            grid.Children.Add(stack);



                            products2.Children.Add(grid);
                            Grid.SetColumn(grid, ml2);

                            ml2++;

                        }
                    }
                }
                else if (query_buff.Contains("SEARCH_PRODUCT_BY_NAME"))
                {
                    products1.Children.Clear();
                    products2.Children.Clear();
                    tovarinfo.Clear();



                    using (NpgsqlConnection con = GetConnection())
                    {
                        string query_all_tovars = "select * from SEARCH_PRODUCT_BY_NAME(@1) offset @2 rows fetch next 8 rows only;";
                        query_buff = query_all_tovars;
                        NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                        all_tovars_cmd.Parameters.AddWithValue("@1", search_buff);
                        all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                        con.Open();
                        var reader = all_tovars_cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                        }
                    }


                    int ml = 0;
                    int ml2 = 0;

                    foreach (var a in tovarinfo)
                    {

                        var f = a.IMG;
                        if (ml <= 3)
                        {
                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);
                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;
                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);
                            grid.Children.Add(stack);
                            products1.Children.Add(grid);

                            Grid.SetColumn(grid, ml);

                            ml++;
                        }
                        else if (ml > 3)
                        {
                            VisualStateManager visualStateManager = new VisualStateManager();

                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);

                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;

                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);

                            grid.Children.Add(stack);



                            products2.Children.Add(grid);
                            Grid.SetColumn(grid, ml2);

                            ml2++;

                        }
                    }

                }
                else
                {

                }
            }catch(Exception k)
            {
                MessageBox.Show(k.ToString());
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                int check = 0;
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();

                using (NpgsqlConnection checkcon = GetConnection())
                {
                    string check_query = "select * from Product;";
                    NpgsqlCommand check_cmd = new NpgsqlCommand(check_query, checkcon);
                    checkcon.Open();
                    var check_reader = check_cmd.ExecuteReader();
                    while (check_reader.Read())
                    {
                        check++;
                    }
                    checkcon.Close();
                }
                if (ot > check - 7)
                {
                    MessageBox.Show("Дальше пусто");
                }
                else
                {
                    ot = ot + 8;
                }

                if (query_buff.Length == 0)
                {
                    using (NpgsqlConnection con = GetConnection())
                    {
                        string query_all_tovars = "select * from product order by product_name offset @1 rows fetch next 8 rows only;";

                        NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                        all_tovars_cmd.Parameters.AddWithValue("@1", ot);

                        con.Open();
                        var reader = all_tovars_cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                        }
                    }


                    int ml = 0;
                    int ml2 = 0;

                    foreach (var a in tovarinfo)
                    {

                        var f = a.IMG;
                        if (ml <= 3)
                        {
                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);
                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;
                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);

                            grid.Children.Add(stack);
                            products1.Children.Add(grid);

                            Grid.SetColumn(grid, ml);

                            ml++;
                        }
                        else if (ml > 3)
                        {
                            VisualStateManager visualStateManager = new VisualStateManager();

                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);

                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;

                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);

                            grid.Children.Add(stack);



                            products2.Children.Add(grid);
                            Grid.SetColumn(grid, ml2);

                            ml2++;

                        }
                    }
                }
                else if (query_buff.Contains("SEARCH_PRODUCT_BY_CATEGORY"))
                {
                    products1.Children.Clear();
                    products2.Children.Clear();
                    tovarinfo.Clear();


                    using (NpgsqlConnection con = GetConnection())
                    {
                        string query_all_tovars = "select * from SEARCH_PRODUCT_BY_CATEGORY(@1) offset @2 rows fetch next 8 rows only;";
                        query_buff = query_all_tovars;
                        NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                        all_tovars_cmd.Parameters.AddWithValue("@1", category_buff);
                        all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                        con.Open();
                        var reader = all_tovars_cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                        }
                    }


                    int ml = 0;
                    int ml2 = 0;

                    foreach (var a in tovarinfo)
                    {

                        var f = a.IMG;
                        if (ml <= 3)
                        {
                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);
                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;
                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);
                            grid.Children.Add(stack);
                            products1.Children.Add(grid);

                            Grid.SetColumn(grid, ml);

                            ml++;
                        }
                        else if (ml > 3)
                        {
                            VisualStateManager visualStateManager = new VisualStateManager();

                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);

                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;

                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);
                            grid.Children.Add(stack);



                            products2.Children.Add(grid);
                            Grid.SetColumn(grid, ml2);

                            ml2++;

                        }
                    }
                }
                else if (query_buff.Contains("SEARCH_PRODUCT_BY_NAME"))
                {
                    products1.Children.Clear();
                    products2.Children.Clear();
                    tovarinfo.Clear();



                    using (NpgsqlConnection con = GetConnection())
                    {
                        string query_all_tovars = "select * from SEARCH_PRODUCT_BY_NAME(@1) offset @2 rows fetch next 8 rows only;";
                        query_buff = query_all_tovars;
                        NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                        all_tovars_cmd.Parameters.AddWithValue("@1", search_buff);
                        all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                        con.Open();
                        var reader = all_tovars_cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                        }
                    }


                    int ml = 0;
                    int ml2 = 0;

                    foreach (var a in tovarinfo)
                    {

                        var f = a.IMG;
                        if (ml <= 3)
                        {
                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);
                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;
                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);
                            grid.Children.Add(stack);
                            products1.Children.Add(grid);

                            Grid.SetColumn(grid, ml);

                            ml++;
                        }
                        else if (ml > 3)
                        {
                            VisualStateManager visualStateManager = new VisualStateManager();

                            Grid grid = new Grid();
                            RowDefinition rowpage = new RowDefinition();
                            RowDefinition rowname = new RowDefinition();
                            RowDefinition rowprice = new RowDefinition();
                            StackPanel stack = new StackPanel();
                            Image png = new Image() { Width = 130 };
                            Label name = new Label();
                            Label price = new Label();
                            Button add = new Button();
                            add.Content = "В корзину";
                            Button addd = new Button();
                            basket.Add(new Basket());

                            add.Click += add_basket_click;
                            add.Width = 60;
                            add.Height = 30;

                            add.Tag += a.Name;
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Имя: " + a.Name;
                            /*   grid.Children.Add(name);
                               grid.Children.Add(price);
                               Grid.SetRow(price, 2);
                               Grid.SetColumn(price, ml2);

                               Grid.SetRow(name, 1);
                               Grid.SetColumn(name, ml2);*/
                            png.Height = 150;
                            png.Source = new BitmapImage(new Uri(a.IMG));
                            png.Margin = new Thickness(10, 10, 10, 10);



                            grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                            grid.Margin = new Thickness(0, 10, 0, 10);

                            grid.MaxHeight = 230;
                            grid.MaxWidth = 170;

                            stack.Children.Add(png);
                            stack.Children.Add(name);
                            stack.Children.Add(price);
                            grid.Children.Add(stack);



                            products2.Children.Add(grid);
                            Grid.SetColumn(grid, ml2);

                            ml2++;

                        }
                    }

                }
                else
                {

                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.ToString());
            }
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            ot = 0;
            search_buff = search.Text;
            try
            {
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();

                
                
                using (NpgsqlConnection con = GetConnection())
                {
                    string query_all_tovars = "select * from SEARCH_PRODUCT_BY_NAME(@1) offset @2 rows fetch next 8 rows only;";
                    query_buff = query_all_tovars;
                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    all_tovars_cmd.Parameters.AddWithValue("@1", search.Text);
                    all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                    con.Open();
                    var reader = all_tovars_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                    }
                }


                int ml = 0;
                int ml2 = 0;

                foreach (var a in tovarinfo)
                {

                    var f = a.IMG;
                    if (ml <= 3)
                    {
                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);
                        grid.MaxHeight = 230;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);
                        products1.Children.Add(grid);

                        Grid.SetColumn(grid, ml);

                        ml++;
                    }
                    else if (ml > 3)
                    {
                        VisualStateManager visualStateManager = new VisualStateManager();

                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);

                        grid.MaxHeight = 230;
                        grid.MaxWidth = 170;

                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);



                        products2.Children.Add(grid);
                        Grid.SetColumn(grid, ml2);

                        ml2++;

                    }
                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.ToString());
            }
        }

        private void smart_Click(object sender, RoutedEventArgs e)
        {
            ot = 0;
            category_buff = "Смартфоны";
            try
            {
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();



                using (NpgsqlConnection con = GetConnection())
                {
                    string query_all_tovars = "select * from SEARCH_PRODUCT_BY_CATEGORY(@1) offset @2 rows fetch next 8 rows only;";
                    query_buff = query_all_tovars;
                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    all_tovars_cmd.Parameters.AddWithValue("@1", smart.Content);
                    all_tovars_cmd.Parameters.AddWithValue("@2",ot);

                    con.Open();
                    var reader = all_tovars_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                    }
                }


                int ml = 0;
                int ml2 = 0;

                foreach (var a in tovarinfo)
                {

                    var f = a.IMG;
                    if (ml <= 3)
                    {
                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);
                        grid.MaxHeight = 230;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);
                        products1.Children.Add(grid);

                        Grid.SetColumn(grid, ml);

                        ml++;
                    }
                    else if (ml > 3)
                    {
                        VisualStateManager visualStateManager = new VisualStateManager();

                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);

                        grid.MaxHeight = 230;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);

                        grid.Children.Add(stack);



                        products2.Children.Add(grid);
                        Grid.SetColumn(grid, ml2);

                        ml2++;

                    }
                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.ToString());
            }
        }

        private void brasl_Click(object sender, RoutedEventArgs e)
        {
            ot = 0;
            category_buff = "Браслеты";

            try
            {
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();



                using (NpgsqlConnection con = GetConnection())
                {
                    string query_all_tovars = "select * from SEARCH_PRODUCT_BY_CATEGORY(@1) offset @2 rows fetch next 8 rows only;";
                    query_buff = query_all_tovars;
                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    all_tovars_cmd.Parameters.AddWithValue("@1", brasl.Content);
                    all_tovars_cmd.Parameters.AddWithValue("@2",ot);

                    con.Open();
                    var reader = all_tovars_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                    }
                }


                int ml = 0;
                int ml2 = 0;

                foreach (var a in tovarinfo)
                {

                    var f = a.IMG;
                    if (ml <= 3)
                    {
                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);
                        grid.MaxHeight = 230;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);
                        products1.Children.Add(grid);

                        Grid.SetColumn(grid, ml);

                        ml++;
                    }
                    else if (ml > 3)
                    {
                        VisualStateManager visualStateManager = new VisualStateManager();

                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);

                        grid.MaxHeight = 230;
                        grid.MaxWidth = 170;

                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);



                        products2.Children.Add(grid);
                        Grid.SetColumn(grid, ml2);

                        ml2++;

                    }
                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.ToString());
            }
        }

        private void clocks_Click(object sender, RoutedEventArgs e)
        {
            ot = 0;
            category_buff = "Часы";

            try
            {
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();



                using (NpgsqlConnection con = GetConnection())
                {
                    string query_all_tovars = "select * from SEARCH_PRODUCT_BY_CATEGORY(@1) offset @2 rows fetch next 8 rows only;";
                    query_buff = query_all_tovars;
                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    all_tovars_cmd.Parameters.AddWithValue("@1", clocks.Content);
                    all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                    con.Open();
                    var reader = all_tovars_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                    }
                }


                int ml = 0;
                int ml2 = 0;

                foreach (var a in tovarinfo)
                {

                    var f = a.IMG;
                    if (ml <= 3)
                    {
                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;

                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);
                        grid.MaxHeight = 250;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);
                        products1.Children.Add(grid);

                        Grid.SetColumn(grid, ml);

                        ml++;
                    }
                    else if (ml > 3)
                    {
                        VisualStateManager visualStateManager = new VisualStateManager();

                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();

                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;


                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);

                        grid.MaxHeight = 250;
                        grid.MaxWidth = 170;

                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);



                        products2.Children.Add(grid);
                        Grid.SetColumn(grid, ml2);

                        ml2++;

                    }
                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.ToString());
            }
        }

        private void laptop_Click(object sender, RoutedEventArgs e)
        {
            ot = 0;
            category_buff = "Планшеты";

            try
            {
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();



                using (NpgsqlConnection con = GetConnection())
                {
                    string query_all_tovars = "select * from SEARCH_PRODUCT_BY_CATEGORY(@1) offset @2 rows fetch next 8 rows only;";
                    query_buff = query_all_tovars;
                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    all_tovars_cmd.Parameters.AddWithValue("@1", laptop.Content);
                    all_tovars_cmd.Parameters.AddWithValue("@2", ot);

                    con.Open();
                    var reader = all_tovars_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tovarinfo.Add(new ProductAll(reader.GetString(reader.GetOrdinal("product_name")), reader.GetInt32(reader.GetOrdinal("product_price")), reader.GetString(reader.GetOrdinal("product_img"))));
                    }
                }


                int ml = 0;
                int ml2 = 0;

                foreach (var a in tovarinfo)
                {

                    var f = a.IMG;
                    if (ml <= 3)
                    {
                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        Button add = new Button();
                        add.Content = "В корзину";
                        Button addd = new Button();
                        basket.Add(new Basket());

                        add.Click += add_basket_click;
                        add.Width = 60;
                        add.Height = 30;

                        add.Tag += a.Name;
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);
                        grid.MaxHeight = 250;
                        grid.MaxWidth = 170;
                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
                        grid.Children.Add(stack);
                        products1.Children.Add(grid);

                        Grid.SetColumn(grid, ml);

                        ml++;
                    }
                    else if (ml > 3)
                    {
                        VisualStateManager visualStateManager = new VisualStateManager();

                        Grid grid = new Grid();
                        RowDefinition rowpage = new RowDefinition();
                        RowDefinition rowname = new RowDefinition();
                        RowDefinition rowprice = new RowDefinition();
                        StackPanel stack = new StackPanel();
                        Image png = new Image() { Width = 130 };
                        Label name = new Label();
                        Label price = new Label();
                        
                        price.Content = "Цена: " + a.Price;
                        name.Content = "Имя: " + a.Name;
                        /*   grid.Children.Add(name);
                           grid.Children.Add(price);
                           Grid.SetRow(price, 2);
                           Grid.SetColumn(price, ml2);

                           Grid.SetRow(name, 1);
                           Grid.SetColumn(name, ml2);*/
                        png.Height = 150;
                        png.Source = new BitmapImage(new Uri(a.IMG));
                        png.Margin = new Thickness(10, 10, 10, 10);



                        grid.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                        grid.Margin = new Thickness(0, 10, 0, 10);

                        grid.MaxHeight = 250;
                        grid.MaxWidth = 170;

                        stack.Children.Add(png);
                        stack.Children.Add(name);
                        stack.Children.Add(price);
              
                        grid.Children.Add(stack);



                        products2.Children.Add(grid);
                        Grid.SetColumn(grid, ml2);

                        ml2++;

                    }
                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.ToString());
            }
        }
        public void add_basket_click(object sender, RoutedEventArgs e)
        {
            string name = "";
            foreach(var a in user1)
            {
                name = a.Name;
            }
            var myValue = ((Button)sender).Tag;
            Add_to_basket(name, myValue.ToString());
            
            
        }
         public void Add_to_basket(string name_arg, string product_id_arg)
        {
            try
            {
                productAll.Add(new ProductAll(product_id_arg));
                int i = 0;
                int p = 0;
                string aaa = product_id_arg;
                using (NpgsqlConnection con = GetConnection())
                {
                    string query_get_name = @"select * from Users where user_name= @1";

                    NpgsqlCommand get_name_cmd = new NpgsqlCommand(query_get_name, con);
                    foreach (var a in user1)
                    {
                        get_name_cmd.Parameters.AddWithValue("@1", a.Name);
                    }

                    con.Open();
                    var reader = get_name_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        i = reader.GetInt32(reader.GetOrdinal("user_id"));
                    }
                    con.Close();

                    //////
                    string query_get_product = @"select * from Product where product_name= @1";


                    NpgsqlCommand get_product_cmd = new NpgsqlCommand(query_get_product, con);
                    
                        get_product_cmd.Parameters.AddWithValue("@1", aaa);
                    


                    con.Open();
                    var reader_product = get_product_cmd.ExecuteReader();
                    while (reader_product.Read())
                    {
                        p = reader_product.GetInt32(reader_product.GetOrdinal("product_id"));
                    } 
                    con.Close();




                    string query_add_basket = @"call ADD_PRODUCT_TO_BASKET(@1,@2);";
                    NpgsqlCommand add_basket_cmd = new NpgsqlCommand(query_add_basket, con);
                    add_basket_cmd.Parameters.AddWithValue("@1", p);
                    add_basket_cmd.Parameters.AddWithValue("@2", i);
                    con.Open();

                    var add_basket = add_basket_cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            catch(Exception g) { MessageBox.Show(g.ToString()); }
        }
        private void basketbutt_Click(object sender, RoutedEventArgs e)
        {

            UserBasket userBasket = new UserBasket();
            this.Close(); 
            userBasket.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }

    
}

 

