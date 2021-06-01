using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
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
    /// Логика взаимодействия для Material.xaml
    /// </summary>
    public partial class Material : UserControl
    {
        public MainWindow main;

        public Material()
        {
            InitializeComponent();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Connection.Stroka))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($@"DELETE FROM [dbo].[materials_k] WHERE [Наименование_материала] = '{nameLabel.Content}'", connection);

                command.ExecuteNonQuery();

                main.Load_data("");
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            EditMaterial editMaterial = new EditMaterial();

            editMaterial.invisibleLabel.Content = "1";
            editMaterial.invisibleNameLabel.Content = nameLabel.Content;

            editMaterial.nameTextBox.Text = nameLabel.Content.ToString();
            editMaterial.typeTextBox.Text = typeLabel.Content.ToString();
            editMaterial.costTextBox.Text = costLabel.Content.ToString();
            editMaterial.skladkolvoTextBox.Text = skladkolvoLabel.Content.ToString();
            editMaterial.minkolvoTextBox.Text = minkolvoLabel.Content.ToString();

            editMaterial.MainWindow = main;

            editMaterial.ShowDialog();
        }
    }
}
