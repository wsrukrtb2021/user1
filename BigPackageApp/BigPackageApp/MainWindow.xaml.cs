using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace BigPackageApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal void Load_data(string a)
        {
            materialsList.Children.Clear();

            using (SqlConnection connection = new SqlConnection(Connection.Stroka))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"SELECT * FROM [dbo].[materials_k]" + a, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Material material = new Material();

                        material.nameLabel.Content = reader[0];
                        material.typeLabel.Content = reader[1];
                        material.costLabel.Content = reader[3];
                        material.skladkolvoLabel.Content = reader[4];
                        material.minkolvoLabel.Content = reader[5];
                        material.izmerLabel.Content = reader[7];

                        material.main = this;

                        materialsList.Children.Add(material);
                    }
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                Load_data($" WHERE [Наименование_материала] like '{searchTextBox.Text}%'");
            }

            else Load_data("");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_data("");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            EditMaterial editMaterial = new EditMaterial();

            editMaterial.invisibleLabel.Content = "0";

            editMaterial.MainWindow = this;

            editMaterial.ShowDialog();
        }
    }
}
