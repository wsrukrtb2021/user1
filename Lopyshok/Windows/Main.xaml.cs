using Lopyshok.Classes;
using Lopyshok.Controls;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Lopyshok.Windows
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        internal void Load_data(string s)
        {
            list.Children.Clear();
            using (SqlConnection connection = new SqlConnection(Connection.String))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT ProductType.Title, 
                                                              Product.Title AS Expr1, 
                                                              Product.ArticleNumber, 
                                                              Product.Image, 
                                                              Product.ProductionPersonCount,                                                               
                                                              Product.ProductionWorkshopNumber, 
                                                              Product.MinCostForAgent, 
                                                              Product.Description, 
                                                              MaterialType.Title AS Expr2, 
                                                              Material.Cost,
                                                              Product.ID

                                                       FROM   Product INNER JOIN
                                                              ProductMaterial ON Product.ID = ProductMaterial.ProductID INNER JOIN
                                                              Material ON ProductMaterial.MaterialID = Material.ID INNER JOIN
                                                              MaterialType ON Material.MaterialTypeID = MaterialType.ID INNER JOIN
                                                              ProductType ON Product.ProductTypeID = ProductType.ID

                                                              WHERE (Product.Title like '%{Search.Text}%' or Product.Description like '%{Search.Text}%') 
                                                              AND ProductType.Title like '{(Filtr.SelectedIndex == 0 ? "" : ((ComboBoxItem)Filtr.SelectedItem).Content)}%'
                                                       ORDER BY {(Sort.SelectedIndex == 0 ? "Product.ID" : (Sort.SelectedIndex == 1 ? "Expr1" 
                                                                                                         : (Sort.SelectedIndex == 2? "Expr1 DESC" 
                                                                                                         : (Sort.SelectedIndex == 3 ? "ProductionWorkshopNumber" 
                                                                                                         : "MinCostForAgent"))))}" + s, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product_Control product = new Product_Control();
                        product.Type.Content = reader[0];
                        product.Name.Content = reader[1].ToString();
                        product.Article.Content = reader[2];
                        try { product.Photo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + reader[3].ToString())); } catch { }
                        product.Person.Content = reader[4];
                        product.Number.Content = reader[5];
                        product.Minimum.Content = reader[6];
                        product.Description.Content = reader[7];
                        product.Materials.Content = reader[8];
                        product.Price.Content = reader[9];
                        product.ID.Text = reader[10].ToString();
                        product.Main = this;
                        list.Children.Add(product);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_data("");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Second edit = new Second();
            edit.Main = this;
            edit.Show();
            this.Hide();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Pokaz.PageUp();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Pokaz.PageDown();
        }

        private void Pokaz_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            Seacrh_block.Visibility = Visibility.Collapsed;
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    Seacrh_block.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
