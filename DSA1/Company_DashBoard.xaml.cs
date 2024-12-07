using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DSA1
{
    /// <summary>
    /// Interaction logic for Company_DashBoard.xaml
    /// </summary>
    public partial class Company_DashBoard : Window
    {
        public Company_DashBoard()
        {
            InitializeComponent();
            Applicant_search.As_WindowTracker = 2;
            Company_Profile.WindowNumber = 2;
        }
        
        public static List<Jobs> Job_DataBase()
        {
            List<Jobs> jobFeed = new List<Jobs>();
            using (MySqlConnection connection = new MySqlConnection("Server=127.0.0.1;" +
            "Database=project_database;UserName= root;" +
            "Password=SQLD"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM jobs_db";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Jobs item = new Jobs
                            {
                                Job_Position = reader["Job_Position"].ToString(),
                                Job_Description = reader["Job_Desctription"].ToString(),
                                IsFilled = reader["is_filled"].ToString(),
                                Job_id = Convert.ToInt32(reader["id"]),
                                Company_userNumber = Convert.ToInt32(reader["Company_userNumber"])
                            };
                            jobFeed.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return jobFeed;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Click_Profile(object sender, RoutedEventArgs e)
        {
            Company_Profile cp = new Company_Profile();
            this.Hide();
            cp.Show();
        }

        private void applicantsearch_btn(object sender, RoutedEventArgs e)
        {

        }

        private void application_btn(object sender, RoutedEventArgs e)
        {
            Applicant_search ap = new Applicant_search();
            this.Hide();
            ap.Show();
        }

        private void Log_out_btn(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
