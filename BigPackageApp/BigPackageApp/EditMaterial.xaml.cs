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
using System.Windows.Shapes;

namespace BigPackageApp
{
    /// <summary>
    /// Логика взаимодействия для EditMaterial.xaml
    /// </summary>
    public partial class EditMaterial : Window
    {
        public EditMaterial()
        {
            InitializeComponent();
        }

        public MainWindow MainWindow;

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (invisibleLabel.Content.ToString() == "0")
            {
                using (SqlConnection connection = new SqlConnection(Connection.Stroka))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand($@"INSERT INTO [dbo].[materials_k]
                                                           ([Наименование_материала]
                                                           ,[Тип_материала]
                                                           ,[Цена]
                                                           ,[Количество_на_складе]
                                                           ,[Минимальное_количество])
                                                           VALUES
                                                           ('{nameTextBox.Text}'
                                                           ,'{typeTextBox.Text}'
                                                           ,'{costTextBox.Text}'
                                                           ,'{skladkolvoTextBox.Text}'
                                                           ,'{minkolvoTextBox.Text}')", connection);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Добавлен");

                        MainWindow.Load_data("");
                    }

                    catch
                    {
                        MessageBox.Show("Вы неправильно заполнили поля! Длина поля не должна превышать 50 символов");
                    }
                }
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(Connection.Stroka))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand($@"UPDATE [dbo].[materials_k]
                                                           SET [Наименование_материала] = '{nameTextBox.Text}'
                                                           ,[Тип_материала] = '{typeTextBox.Text}'
                                                           ,[Цена] = '{costTextBox.Text}'
                                                           ,[Количество_на_складе] = '{skladkolvoTextBox.Text}'
                                                           ,[Минимальное_количество] = '{minkolvoTextBox.Text}'
                                                           WHERE [Наименование_материала] = '{invisibleNameLabel.Content}'", connection);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Сохранен");

                        MainWindow.Load_data("");
                    }

                    catch
                    {
                        MessageBox.Show("Вы неправильно заполнили поля! Длина поля не должна превышать 50 символов");
                    }
                }
            }
        }

        private void nameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Char.IsDigit(e.Text, 0);
        }

        private void costTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.Load_data("");
        }
    }
}
