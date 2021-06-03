using Lopyshok.Classes;
using Lopyshok.Windows;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace Lopyshok.Controls
{
    public partial class Product_Control : UserControl
    {
        public Product_Control()
        {
            InitializeComponent();
        }
        public Main Main;

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            Second edit = new Second();
            edit.ID.Text = (string)ID.Text.ToString();
            edit.Article.Text = (string)Article.Content;
            edit.Photo.Source = Photo.Source;
            edit.Name.Text = Name.Content.ToString();
            edit.Type.SelectedItem = Type.Content;
            edit.Person.Text = Person.Content.ToString();
            edit.Number.Text = Number.Content.ToString();
            edit.Minimum.Text = Minimum.Content.ToString();
            edit.Description.Text = Description.Content.ToString();
            edit.Main = Main;
            Main.Hide();
            edit.ShowDialog();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Connection.String))
            {
                if (MessageBox.Show("Удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"Delete FROM Product WHERE ID = {ID.Text}", connection);
                    command.ExecuteNonQuery();
                    Main.Load_data("");
                }
                else { }
            }
        }
    }
}
