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
    /// Логика взаимодействия для UserBasket.xaml
    /// </summary>
    public partial class UserBasket : Window
    {
        
        List<User> user1 = new List<User>();
        List<ProductAll> tovarinfo = new List<ProductAll>();
        int ot = 0;
        List<Basket> basketproducts = new List<Basket>();
        List<UserId> useridlist = new List<UserId>();
        List<Comments> comments = new List<Comments>();
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Host=localhost;Username=user1;Password=12345;Database=HiTech");
        }
        public UserBasket()
        {
            InitializeComponent();
            XmlSerializer xml = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = File.OpenRead("username.xml"))
            {
                List<User> bufflist = new List<User>();
                bufflist = xml.Deserialize(fs) as List<User>;
                foreach (User a in bufflist)
                {
                    user1.Add(a);

                }
            }

            using (NpgsqlConnection con = GetConnection())
            {
                string query_id_tovars = "select * from CHECK_BASKET_FUNC(@1) order by product_name offset @2 rows fetch next 8 rows only;";
                string get_id_user = "select * from Users where user_name =@1;";

                NpgsqlCommand get_user_cmd = new NpgsqlCommand(get_id_user, con);
                foreach (var a in user1)
                {
                    get_user_cmd.Parameters.AddWithValue("@1", a.Name);

                }
                
                con.Open();
                var reader = get_user_cmd.ExecuteReader();
                while (reader.Read())
                {
                    useridlist.Add(new UserId(reader.GetInt32(reader.GetOrdinal("user_id"))));
                }
                con.Close();
                NpgsqlCommand id_tovars_cmd = new NpgsqlCommand(query_id_tovars, con);

                foreach (var a in useridlist)
                {
                    id_tovars_cmd.Parameters.AddWithValue("@1", a.userid);

                }
                id_tovars_cmd.Parameters.AddWithValue("@2", ot);

                con.Open();

                var readerbasket = id_tovars_cmd.ExecuteReader();

                while (readerbasket.Read())
                {
                    tovarinfo.Add(new ProductAll(readerbasket.GetString(readerbasket.GetOrdinal("product_name")), readerbasket.GetInt32(readerbasket.GetOrdinal("product_price")), readerbasket.GetString(readerbasket.GetOrdinal("product_img")) ));
                }
                con.Close();


                

                
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
                    Button comment = new Button();
                    Button select_comment = new Button();
                    select_comment.Content = "Отзывы";
                    select_comment.Click += select_comment_Click;
                    select_comment.Width = 50;
                    select_comment.Height = 30;
                    select_comment.Tag = a.Name;
                    select_comment.HorizontalAlignment = HorizontalAlignment.Right;
                    select_comment.VerticalAlignment = VerticalAlignment.Bottom;

                    comment.Content = "Оценить";
                    comment.VerticalAlignment = VerticalAlignment.Bottom;
                    comment.Click += Comment_Click;
                    comment.Width = 50;
                    comment.Height = 30;
                    comment.Tag = a.Name;
                price.Content = "Цена: "+a.Price;
                        name.Content = "Название: " + a.Name;
               
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
                    grid.Children.Add(comment);
                    grid.Children.Add(select_comment);

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
                    Button comment = new Button();
                    Button select_comment = new Button();
                    select_comment.Content = "Отзывы";
                    select_comment.Click += select_comment_Click;
                    select_comment.Width = 50;
                    select_comment.Height = 30;
                    select_comment.Tag = a.Name;
                    comment.Content = "Оценить";
                    comment.Click += Comment_Click;
                    comment.Width = 50;
                    comment.Height = 30;
                    comment.Tag = a.Name;
                    price.Content = "Цена: " + a.Price;
                    name.Content = "Название: " + a.Name;
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
                    stack.Children.Add(comment);
                    stack.Children.Add(select_comment);
                    grid.Children.Add(stack);



                    products2.Children.Add(grid);
                    Grid.SetColumn(grid, ml2);

                    ml2++;

                }
            }



        }

        private void select_comment_Click(object sender, RoutedEventArgs e)
        {
            basketproducts.Clear();
            gridforcomments.Children.Clear();
            try
            {
                List<Grid> grids = new List<Grid>();
                comments.Clear();
                using (NpgsqlConnection con = GetConnection())
                {
                    string comment_select_query = "select * from SELECT_COMMENTS(@1);";
                    string get_product_id = "select * from Product where product_name = @1;";
                    NpgsqlCommand product_cmd = new NpgsqlCommand(get_product_id, con);

                    NpgsqlCommand comment_select_cmd = new NpgsqlCommand(comment_select_query, con);
                    product_cmd.Parameters.AddWithValue("@1", ((Button)sender).Tag.ToString());

                    con.Open();
                    var readerproduct = product_cmd.ExecuteReader();
                    while (readerproduct.Read())
                    {
                        basketproducts.Add(new Basket(readerproduct.GetInt32(readerproduct.GetOrdinal("product_id"))));
                    }
                    con.Close();
                    foreach (var a in basketproducts)
                    {
                        comment_select_cmd.Parameters.AddWithValue("@1", a.product_id);

                    }
                    con.Open();
                    var reader = comment_select_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comments.Add(new Comments(reader.GetString(reader.GetOrdinal("user_name")), reader.GetString(reader.GetOrdinal("comments_text"))));
                    }
                    con.Close();
                    gridforcomments.Visibility = Visibility.Visible;

                    // Создание экземпляра ScrollViewer
                    ScrollViewer scrollViewer = new ScrollViewer();

                    // Создание экземпляра Grid
                    Grid grid = new Grid();
                    grid.Children.Clear();
                    // Создание экземпляров класса Label

                    double verticalMargin = 10; // Задайте желаемый вертикальный промежуток между метками

                    // Создание RowDefinition для каждой метки
                    for (int i = 0; i < comments.Count; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());
                    }

                    // Переменная для отслеживания текущей строки
                    int currentRow = 0;

                    foreach (var a  in comments)
                    {
                        // Создание экземпляра Label
                        Label label = new Label();
                        Label label1 = new Label();
                        
                        label1.Content = "Имя: " + a.user_name;
                     

                        label.Content = "Отзыв: " + a.comment_text;
                        label.BorderThickness = new Thickness(1);
                        label.BorderBrush = Brushes.Black;
                        // Установка промежутков между метками
                       
                        label1.Margin = new Thickness(0, verticalMargin + 10, 0, 0);

                        // Добавление Label в Grid
                        grid.Children.Add(label);
                        grid.Children.Add(label1);

                        // Определение позиции Label в Grid
                        Grid.SetColumn(label, 0);
                        Grid.SetRow(label, currentRow);
                        Grid.SetColumn(label1, 0);
                        Grid.SetRow(label1, currentRow);
                        // Увеличение текущей строки
                        currentRow++;
                    }

                    // Установка содержимого ScrollViewer
                    scrollViewer.Content = grid;
                    gridforcomments.Children.Add(scrollViewer);
                    // Добавление ScrollViewer на окно или контейнер
                    // Например, если это окно WPF, вы можете использовать:
                    // mainWindow.Content = scrollViewer;                   

                }
            }
            catch(Exception h)
            {
                MessageBox.Show(h.ToString());
            }
        }

        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    string get_product_id = "select * from Product where product_name = @1;";
                    string comment_query = "call ADD_COMMENT(@1, @2,@3);";


                    NpgsqlCommand comment_cmd = new NpgsqlCommand(comment_query, con);
                    NpgsqlCommand product_cmd = new NpgsqlCommand(get_product_id, con);
                    product_cmd.Parameters.AddWithValue("@1", ((Button)sender).Tag.ToString());
                    con.Open();
                    var reader = product_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        basketproducts.Add(new Basket(reader.GetInt32(reader.GetOrdinal("product_id"))));
                    }
                    con.Close();

                    foreach (var a in basketproducts)
                    {
                        comment_cmd.Parameters.AddWithValue("@2", a.product_id);

                    }
                    foreach (var a in useridlist)
                    {
                        comment_cmd.Parameters.AddWithValue("@1", a.userid);

                    }
                    comment_cmd.Parameters.AddWithValue("@3", comment.Text);



                    con.Open();
                    var reader1 = comment_cmd.ExecuteNonQuery();

                    con.Close();

                }
            }catch(Exception h)
            {
                MessageBox.Show(h.ToString());
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
                using (NpgsqlConnection con = GetConnection())
                {
                    string query_id_tovars = "select * from CHECK_BASKET_FUNC(@1);";
                    string get_id_user = "select * from Users where user_name =@1;";

                    NpgsqlCommand get_user_cmd = new NpgsqlCommand(get_id_user, con);
                    foreach (var a in user1)
                    {
                        get_user_cmd.Parameters.AddWithValue("@1", a.Name);

                    }

                    con.Open();
                    var reader = get_user_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        useridlist.Add(new UserId(reader.GetInt32(reader.GetOrdinal("user_id"))));
                    }
                    con.Close();
                    NpgsqlCommand id_tovars_cmd = new NpgsqlCommand(query_id_tovars, con);

                    foreach (var a in useridlist)
                    {
                        id_tovars_cmd.Parameters.AddWithValue("@1", a.userid);

                    }

                    con.Open();

                    var readerbasket = id_tovars_cmd.ExecuteReader();

                    while (readerbasket.Read())
                    {
                        basketproducts.Add(new Basket(readerbasket.GetInt32(readerbasket.GetOrdinal("product_ids"))));
                    }
                    con.Close();


                    string query_all_tovars = "select * from product where product_id = @1 order by product_name offset @2 rows fetch next 8 rows only;";

                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    foreach (var a in basketproducts)
                    {
                        all_tovars_cmd.Parameters.AddWithValue("@1", a.product_id);
                        all_tovars_cmd.Parameters.AddWithValue("@2", ot);
                        con.Open();
                        var reader_all = all_tovars_cmd.ExecuteReader();
                        while (reader_all.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader_all.GetString(reader_all.GetOrdinal("product_name")), reader_all.GetInt32(reader_all.GetOrdinal("product_price")), reader_all.GetString(reader_all.GetOrdinal("product_img"))));
                        }
                        con.Close();
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
                        add.Content = a.Name;


                        price.Content = "Цена: " + a.Price;
                        name.Content = "Название: " + a.Name;
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
                        stack.Children.Add(add);

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
                        name.Content = "Название: " + a.Name;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                int check = 0;
                products1.Children.Clear();
                products2.Children.Clear();
                tovarinfo.Clear();
                using (NpgsqlConnection con = GetConnection())
                {
                    string query_id_tovars = "select * from CHECK_BASKET_FUNC(@1);";
                    string get_id_user = "select * from Users where user_name =@1;";

                    NpgsqlCommand get_user_cmd = new NpgsqlCommand(get_id_user, con);
                    foreach (var a in user1)
                    {
                        get_user_cmd.Parameters.AddWithValue("@1", a.Name);

                    }

                    con.Open();
                    var reader = get_user_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        useridlist.Add(new UserId(reader.GetInt32(reader.GetOrdinal("user_id"))));
                    }
                    con.Close();
                    NpgsqlCommand id_tovars_cmd = new NpgsqlCommand(query_id_tovars, con);

                    foreach (var a in useridlist)
                    {
                        id_tovars_cmd.Parameters.AddWithValue("@1", a.userid);

                    }

                    con.Open();

                    var readerbasket = id_tovars_cmd.ExecuteReader();

                    while (readerbasket.Read())
                    {
                        basketproducts.Add(new Basket(readerbasket.GetInt32(readerbasket.GetOrdinal("product_ids"))));
                    }
                    con.Close();


                    string query_all_tovars = "select * from product where product_id = @1 order by product_name offset @2 rows fetch next 8 rows only;";

                    NpgsqlCommand all_tovars_cmd = new NpgsqlCommand(query_all_tovars, con);
                    foreach (var a in basketproducts)
                    {
                        all_tovars_cmd.Parameters.AddWithValue("@1", a.product_id);
                        all_tovars_cmd.Parameters.AddWithValue("@2", ot);
                        con.Open();
                        var reader_all = all_tovars_cmd.ExecuteReader();
                        while (reader_all.Read())
                        {
                            tovarinfo.Add(new ProductAll(reader_all.GetString(reader_all.GetOrdinal("product_name")), reader_all.GetInt32(reader_all.GetOrdinal("product_price")), reader_all.GetString(reader_all.GetOrdinal("product_img"))));
                        }
                        con.Close();
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
                        add.Content = a.Name;



                        price.Content = "Цена: " + a.Price;
                        name.Content = "Название: " + a.Name;
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
                        stack.Children.Add(add);

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
                        name.Content = "Название: " + a.Name;
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
            catch (Exception g)
            {
                MessageBox.Show(g.ToString());
            }
        }
        public class UserId
        {
            public UserId() { }
            public UserId(int a) { userid = a; }
            public int userid { get; set; }
        }

        private void tomain_Click(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            this.Close();

        }

        private void clearbacket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    string delete_query = "delete from Basket where basket_id = @1";
                    string query_id_tovars = "select * from CHECK_BASKET_FUNC(@1) order by product_name offset @2 rows fetch next 8 rows only;";

                    NpgsqlCommand delete_basket_cmd = new NpgsqlCommand(delete_query, con);
                    foreach (var a in useridlist)
                    {
                        delete_basket_cmd.Parameters.AddWithValue("@1", a.userid);
                        
                    }

                    con.Open();
                        var reader_all = delete_basket_cmd.ExecuteNonQuery();

                        con.Close();

                    products1.Children.Clear();
                    products2.Children.Clear();
                    tovarinfo.Clear();



                    NpgsqlCommand id_tovars_cmd = new NpgsqlCommand(query_id_tovars, con);

                    foreach (var a in useridlist)
                    {
                        id_tovars_cmd.Parameters.AddWithValue("@1", a.userid);

                    }
                    id_tovars_cmd.Parameters.AddWithValue("@2", ot);

                    con.Open();

                    var readerbasket = id_tovars_cmd.ExecuteReader();

                    while (readerbasket.Read())
                    {
                        tovarinfo.Add(new ProductAll(readerbasket.GetString(readerbasket.GetOrdinal("product_name")), readerbasket.GetInt32(readerbasket.GetOrdinal("product_price")), readerbasket.GetString(readerbasket.GetOrdinal("product_img"))));
                    }
                    con.Close();



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
                            add.Content = a.Name;



                            price.Content = "Цена: " + a.Price;
                            name.Content = "Название: " + a.Name;
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
                            price.Content = "Цена: " + a.Price;
                            name.Content = "Название: " + a.Name;
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
            }catch(Exception g)
            {
                MessageBox.Show(g.ToString());
            }
        }
    }
    public class Comments
    {
        public Comments() { }
        public Comments(string user_name_arg, string comment_text_arg )
        {
            user_name = user_name_arg;
            comment_text = comment_text_arg;
        }
        public string user_name { get; set; }
        public string comment_text { get; set; }

    }

}

