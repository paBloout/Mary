using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Valdez_MaryAnneACTGUI
{
    public partial class EditForm : Form
    {
        // ✅ FIXED: Declare connectionString at the class level
        private string connectionString = "Server=localhost;Database=userprofiledb;User ID=root;Password=your_password;";

        public EditForm()
        {
            InitializeComponent();

            // Set NameTextBox to read-only
            NameTextBox.ReadOnly = true;

            // Populate Course and Year dropdowns
            CourseComboBox.Items.AddRange(new string[] { "ABEL", "BSBA", "BSIT", "BPA" });
            YearComboBox.Items.AddRange(new string[] { "First", "Second", "Third", "Fourth" });

            // Load current user profile data
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString)) // ✅ No more error
            {
                conn.Open();
                string query = "SELECT * FROM UserProfile WHERE ID = 1"; // Modify as needed

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        NameTextBox.Text = reader["Name"].ToString();
                        AgeTextBox.Text = reader["Age"].ToString();
                        AddressTextBox.Text = reader["Address"].ToString();
                        ContactNumberTextBox.Text = reader["ContactNumber"].ToString();
                        EmailTextBox.Text = reader["Email"].ToString();
                        CourseComboBox.SelectedItem = reader["Course"].ToString();
                        YearComboBox.SelectedItem = reader["Year"].ToString();
                        ParentNameTextBox.Text = reader["ParentName"].ToString();
                        ParentContactTextBox.Text = reader["ParentContact"].ToString();
                        HobbiesTextBox.Text = reader["Hobbies"].ToString();
                        NicknameTextBox.Text = reader["Nickname"].ToString();
                    }
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(AgeTextBox.Text) ||
                string.IsNullOrEmpty(AddressTextBox.Text) || string.IsNullOrEmpty(ContactNumberTextBox.Text) ||
                string.IsNullOrEmpty(CourseComboBox.SelectedItem?.ToString()) ||
                string.IsNullOrEmpty(YearComboBox.SelectedItem?.ToString()) ||
                string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(ParentNameTextBox.Text) ||
                string.IsNullOrEmpty(ParentContactTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString)) // ✅ No more error
            {
                conn.Open();
                string query = "UPDATE UserProfile SET Name=@Name, Age=@Age, Address=@Address, ContactNumber=@ContactNumber, " +
                               "Email=@Email, Course=@Course, Year=@Year, ParentName=@ParentName, ParentContact=@ParentContact, " +
                               "Hobbies=@Hobbies, Nickname=@Nickname WHERE ID=1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Age", int.Parse(AgeTextBox.Text));
                    cmd.Parameters.AddWithValue("@Address", AddressTextBox.Text);
                    cmd.Parameters.AddWithValue("@ContactNumber", ContactNumberTextBox.Text);
                    cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                    cmd.Parameters.AddWithValue("@Course", CourseComboBox.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Year", YearComboBox.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ParentName", ParentNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@ParentContact", ParentContactTextBox.Text);
                    cmd.Parameters.AddWithValue("@Hobbies", HobbiesTextBox.Text);
                    cmd.Parameters.AddWithValue("@Nickname", NicknameTextBox.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Profile successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
