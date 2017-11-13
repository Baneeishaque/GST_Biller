using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GST_Biller
{
    public partial class Product_Manipulation : Form
    {
        SQLite_DB_Handler database = new SQLite_DB_Handler();
        DataTable dt = new DataTable();

        public Product_Manipulation()
        {
            InitializeComponent();

            //dt = new DataTable();
            //SQLiteDataAdapter da;
            //SQLiteConnection SQLite_Connection;

            //string Database_URL = ConfigurationSettings.AppSettings["Database_URL"];
            //string Database_URL = "Data Source = GST_Biller.db; Version = 3;" ;
            //SQLite_Connection = new SQLiteConnection(Database_URL);
            //SQLite_Connection.Open();

            //SQLiteCommand command = new SQLiteCommand("SELECT[Id],[name],[hcn],[tax] FROM[products]", SQLite_Connection);
            //SQLiteDataReader reader = command.ExecuteReader();
            //while (reader.Read())
            //    Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);

            //da = new SQLiteDataAdapter("SELECT[Id],[name],[hcn],[tax] FROM[products]", SQLite_Connection);
            //da.Fill(dt);
            //dataGridView6.DataSource = dt;

            Load_products();
        }

        void Load_products()
        {
            dataGridView_products.DataSource = database.GetTable("SELECT [name] AS [Commodity/Item],[hcn] AS [HSN/SAC],[tax] AS [Tax %] FROM [products] ORDER BY [name]");
            //dataGridView_products.DefaultCellStyle.SelectionBackColor = dataGridView_products.DefaultCellStyle.BackColor;
            //dataGridView_products.DefaultCellStyle.SelectionForeColor = dataGridView_products.DefaultCellStyle.ForeColor;
            //dataGridView_products.ClearSelection();
            groupBox2.Hide();
            groupBox1.Show();
        }

        private void Button_bill_generation_click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void Button5_Click(object sender, EventArgs e)
        {
            //new Batha_Report().ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //new License_Renew().ShowDialog();
            //dataGridView6.DataSource = db.GetTable("SELECT     [Employee ID], Name,  [Join date], [Mobile No.], Type ,Category, DOB, [Blood Group],Status FROM         Employee where Status != 'Removed'");
        }

        private void DataGridView6_Click(object sender, EventArgs e)
        {

            if (dataGridView_products.RowCount != 0)
            {
                display_product_details();
            }

        }

        private void display_product_details()
        {
            groupBox1.Hide();
            groupBox2.Location = groupBox1.Location;
            groupBox2.Text = dataGridView_products.SelectedRows[0].Cells[0].Value.ToString();
            label_hcn.Text = "HCN : " + dataGridView_products.SelectedRows[0].Cells[1].Value.ToString();
            label_tax.Text = "Tax % : " + dataGridView_products.SelectedRows[0].Cells[2].Value.ToString();
            groupBox2.Show();
        }

        ErrorProvider error_provider = new ErrorProvider();
        private bool update_flag = false;
        private string name_to_update;

        bool Is_empty_group(Control[] component_collection)
        {
            bool return_value = true;
            for (int i = 0; i < component_collection.Length; i++)
            {
                error_provider.SetError(component_collection[i], "");
                if (component_collection[i].Text.Equals(""))
                {
                    error_provider.SetError(component_collection[i], "Required Value...");
                    component_collection[i].Focus();
                    return_value = false;
                    break;
                }
            }
            return return_value;
        }

        private void Button_submit_click(object sender, EventArgs e)
        {

            if (Add_product_form_validation())
            {
                if (update_flag)
                {
                    if (database.Ins_Up_Del("DELETE FROM products WHERE ([name] = '" + dataGridView_products.SelectedRows[0].Cells[0].Value.ToString() + "')"))
                    {
                        if (database.Ins_Up_Del("INSERT INTO [products] ([name],[hcn],[tax]) VALUES ('" + textBox_name.Text + "','" + textBox_hcn.Text + "'," + textBox_tax.Text + ")"))
                        {
                            MessageBox.Show("OK", "GST Biller");
                            Load_products();
                            Clear_form(new Control[] { textBox_name, textBox_hcn, textBox_tax });
                        }
                    }
                }
                else
                {
                    if (database.Ins_Up_Del("INSERT INTO [products] ([name],[hcn],[tax]) VALUES ('" + textBox_name.Text + "','" + textBox_hcn.Text + "'," + textBox_tax.Text + ")"))
                    {
                        MessageBox.Show("OK", "GST Biller");
                        Load_products();
                        Clear_form(new Control[] { textBox_name, textBox_hcn, textBox_tax });
                    }
                }
            }

        }

        private void Clear_form(Control[] component_collection)
        {
            for (int i = 0; i < component_collection.Length; i++)
            {
                component_collection[i].Text = "";
            }
        }

        bool Add_product_form_validation()
        {
            return Is_empty_group(new Control[3] { textBox_name, textBox_hcn, textBox_tax }) && Is_decimal_value(textBox_tax, "Invalid Value...");
        }

        bool Is_decimal_value(TextBox textbox, String error_text)
        {
            bool return_value = true;
            try
            {
                error_provider.SetError(textbox, "");
                Decimal i = Convert.ToDecimal(textbox.Text);
                if (i == 0)
                {
                    error_provider.SetError(textbox, error_text);
                    textbox.Focus();
                    return_value = false;
                }
            }
            catch (FormatException format_exception)
            {
                error_provider.SetError(textbox, error_text);
                textbox.Focus();
                return_value = false;
            }
            return return_value;
        }
        private void Button_delete_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Do you want to delete Product : " + dataGridView_products.SelectedRows[0].Cells[0].Value.ToString() + "", "GST Biller", MessageBoxButtons.YesNo).ToString().Equals("Yes")))
            {
                try
                {
                    if (database.Ins_Up_Del("DELETE FROM products WHERE ([name] = '" + dataGridView_products.SelectedRows[0].Cells[0].Value.ToString() + "')"))
                    {
                        MessageBox.Show("Product : " + dataGridView_products.SelectedRows[0].Cells[0].Value.ToString() + " Deleted SuccessFully", "GST Biller");
                        Load_products();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Try Again\nReason : " + ex.Message + "", "GST Biller");
                }
            }

        }

        private void Button_edit_Click(object sender, EventArgs e)
        {
            button_submit.Text = "Update Details";
            groupBox2.Hide();
            textBox_name.Text = dataGridView_products.SelectedRows[0].Cells[0].Value.ToString();
            textBox_hcn.Text = dataGridView_products.SelectedRows[0].Cells[1].Value.ToString();
            textBox_tax.Text = dataGridView_products.SelectedRows[0].Cells[2].Value.ToString();
            groupBox1.Show();

            name_to_update = dataGridView_products.SelectedRows[0].Cells[0].Value.ToString(); ;
            update_flag = true;
        }
    }
}
