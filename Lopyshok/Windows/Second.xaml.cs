using Lopyshok.Classes;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Lopyshok.Windows
{
    public partial class Second : Window
    {
        public Second()
        {
            InitializeComponent();
        }
        public Main Main;
        private string file_Name = "";

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Connection.String))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT [Title] FROM [dbo].[ProductType]", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Type.Items.Add(reader[0].ToString());
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ID.Text.ToString() != "ID")
            {
                using (SqlConnection connection = new SqlConnection(Connection.String))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($@"UPDATE [dbo].[Product]
                                                           SET ArticleNumber = {Article.Text},
                                                               Title = '{Name.Text}',
                                                               ProductTypeID = (SELECT ID FROM [dbo].[ProductType] WHERE Title = '{Type.SelectedItem}'),
                                                               ProductionPersonCount = {Person.Text},
                                                               ProductionWorkshopNumber = {Number.Text},
                                                               MinCostForAgent = {Minimum.Text.Replace(',','.')},
                                                               Description = '{Description.Text}'
                                                               {(file_Name != "" ? @",[Image] = 'products\" + file_Name + "' " : "")}
                                                           WHERE ID = {ID.Text}", connection);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Изменения внесены");
                        Main.Load_data("");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show("Заполните все поля!");
                    }
                }
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(Connection.String))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($@"INSERT INTO [dbo].[Product]
                                                                ([Title],
                                                                 [ProductTypeID],
                                                                 [ArticleNumber], 
                                                                 [Description],
                                                                 [Image],
                                                                 [ProductionPersonCount],
                                                                 [ProductionWorkshopNumber], 
                                                                 [MinCostForAgent]) 
                                                           VALUES ('{Name.Text}',
                                                                    {(Type.SelectedIndex +1).ToString()},
                                                                    {Article.Text},
                                                                   '{Description.Text}',
                                                                    {(file_Name != "" ? @"'products\" + file_Name + "'" : "'products\\picture.jpg'")},
                                                                    {Person.Text},
                                                                    {Number.Text},
                                                                    {Minimum.Text.Replace(',', '.')})", connection);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Продукт добавлен");
                        this.Close();
                        Main.Load_data("");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show("Заполните все поля!");
                    }
                }
            }
        }

        private void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog saveFile = new OpenFileDialog();
            saveFile.ShowDialog();
            if (saveFile.FileName != "")
            {
                Photo.Source = new BitmapImage(new Uri(saveFile.FileName));
                try
                {
                    File.Copy(saveFile.FileName, @".\products\" + saveFile.SafeFileName, true);
                    file_Name = saveFile.SafeFileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Выберите другую фотку!");
                }

            }
        }
    }
}
