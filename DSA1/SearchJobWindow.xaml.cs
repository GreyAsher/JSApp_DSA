using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DSA1
{
    /// <summary>
    /// Interaction logic for SearchJobWindow.xaml
    /// </summary>
    ///
    public partial class SearchJobWindow : Window
    {
        
        public SearchJobWindow()
        {
            InitializeComponent();
            PopulateListBox();
        }
        private List<Jobs> allJobs = new List<Jobs>();
        private void PopulateListBox()
        {
            allJobs = Job_DataBase();
            JobList.ItemsSource = allJobs; 
        }
        public static List<Jobs> Job_DataBase()
        {
            List<Jobs> jobFeed = new List<Jobs>();
            using (MySqlConnection connection = new MySqlConnection("Server=127.0.0.1;" 
                + "Database=project_database;UserName= root;" + "Password=SQLDatabase404"))
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

        Jobs TheSelectedJobs;
  
        private void SearchBox_txtchange(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            string searchText = SearchBox.Text.ToLower();
            var filteredList = allJobs.Where(c =>
                c.Job_Position.ToLower().Contains(searchText) ||
                c.Job_Description.ToLower().Contains(searchText)
            ).ToList();

            JobList.ItemsSource = filteredList;
        }

        private int Company_Number = 0;
        int Resume_Number = 1;
        private void getJobs()
        {
            try
            {
                string query = "INSERT INTO resume_db (Profile_Name, Profile_Number, Entry_Time, Status, CompanyId, ResumeNumber) " +
                               "VALUES (@Profile_Name, @Profile_Number, @Entry_Time, @Status, @CompanyId, @ResumeNumber)";

                string name = MainWindow.applicantUserMapList2[MainWindow.userId].First_Name + " " +
                    MainWindow.applicantUserMapList2[MainWindow.userId].Last_Name;

                Resume_Class Resume = new Resume_Class()
                {
                    userProfile = name,
                    CompanyID_Number = Company_Number,
                    Applicant_Number = MainWindow.userId,
                    Submitted_Date = DateTime.Now,
                    Status = "Pending",
                    Resume_Number = Resume_Number,
                };

                using (MySqlConnection connection = new MySqlConnection("Server=127.0.0.1;Database=project_database;UserName=root;Password=SQLDatabase404"))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Profile_Name", Resume.userProfile);
                        command.Parameters.AddWithValue("@Profile_Number", Resume.Applicant_Number);
                        command.Parameters.AddWithValue("@Entry_Time", Resume.Submitted_Date);
                        command.Parameters.AddWithValue("@Status", Resume.Status);
                        command.Parameters.AddWithValue("@CompanyId", Resume.CompanyID_Number);
                        command.Parameters.AddWithValue("@ResumeNumber", Resume.Resume_Number);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if data was inserted
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data inserted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No data was inserted.");
                        }
                    }
                }
            }
            catch (Exception e) { MessageBox.Show("Error: " + e.Message); }
        }

            private void Search_btn(object sender, RoutedEventArgs e)
        {
            if (JobList.SelectedItem is Jobs selectedCompany)
            {
                getJobs();
            }
        }

        private void Search_selectionChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (JobList.SelectedItem is Jobs selectedJobs)
            {
                Company_Number = selectedJobs.Company_userNumber;
                TheSelectedJobs = selectedJobs;
            }
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            DB db = new DB();
            this.Hide();
            db.Show();
        }
    }
}
