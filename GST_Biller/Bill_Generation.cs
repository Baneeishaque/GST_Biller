using System;
using System.Data;
using System.Windows.Forms;

namespace GST_Biller
{
    public partial class Bill_Generation : Form
    {
        SQLite_DB_Handler database = new SQLite_DB_Handler();

        public Bill_Generation()
        {
            InitializeComponent();

            comboBox_hsn.DataSource = database.GetTable("select [id],[hcn] from [products]");
            comboBox_hsn.DisplayMember = "hcn";
            comboBox_hsn.ValueMember = "id";

        }

        private void Button_products_Click(object sender, EventArgs e)
        {
            new Product_Manipulation().ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_tax.Text = database.GetValue("select [tax] from [products] WHERE [id]=" + comboBox_hsn.SelectedValue + "");
            label_item.Text = database.GetValue("select [name] from [products] WHERE [id]=" + comboBox_hsn.SelectedValue + "");
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_total_TextChanged(object sender, EventArgs e)
        {
            if(!textBox_total.Text.Equals(""))
            {
                label_per_total.Text = (Double.Parse(textBox_total.Text) / (double)numericUpDown_qty.Value).ToString();
                textBox_unit_price.Text = ((100 * Double.Parse(textBox_total.Text)) / (100 + Double.Parse(label_tax.Text))).ToString();
                label_qty.Text = numericUpDown_qty.Value.ToString();
                label_amount.Text = (Double.Parse(textBox_unit_price.Text) * (Double)numericUpDown_qty.Value).ToString();
            }
        }
    }
}
