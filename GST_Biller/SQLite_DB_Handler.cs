using System;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;

namespace GST_Biller
{
    class SQLite_DB_Handler
    {
        public SQLiteConnection SQLite_Connection;
        public SQLiteCommand SQLite_Command;
        public SQLiteTransaction SQLite_Transaction;

        public static string Database_URL = "Data Source=GST_Biller.db;Version=3;";


        public void Open_SQL_Connection()
        {
            try
            {
                if (SQLite_Connection.State == ConnectionState.Closed)
                {
                    SQLite_Connection.Open();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error " + exception.ToString());
            }

        }

        public void Close_SQL_Connection()
        {
            try
            {
                if (SQLite_Connection.State == ConnectionState.Open)
                {
                    SQLite_Connection.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error " + exception.ToString());
            }

        }

        public DataTable General_ReturnDataTable()
        {
            try
            {
                DataTable Data_Table = new DataTable();
                SQLiteDataAdapter SQLite_Data_Adapter = new SQLiteDataAdapter(SQLite_Command);
                SQLite_Data_Adapter.Fill(Data_Table);
                return Data_Table;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception" + exception.ToString());

                return null;
            }

        }



        DataTable data_table;
        SQLiteDataAdapter data_adapter;


        public SQLite_DB_Handler()
        {
            SQLite_Connection = new SQLiteConnection(Database_URL);
        }

        public DataTable GetTable(String query)
        {
            Open_SQL_Connection();
            data_table = new DataTable();
            try
            {
                data_adapter = new SQLiteDataAdapter(query, SQLite_Connection);
                data_table = new DataTable();
                data_adapter.Fill(data_table);

            }
            catch (Exception e)
            {
                MessageBox.Show("Database Error, Reason : " + e.Message);

            }
            Close_SQL_Connection();
            return data_table;

        }

        public DataSet ret(string s)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = SQLite_Connection;
            cmd.CommandText = s;
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public int max_plus(string s)
        {
            int id;
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = SQLite_Connection;
            cmd.CommandText = s;
            try
            {
                SQLite_Connection.Open();
                id = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

            }
            catch
            {
                id = 1;
            }
            finally
            {
                SQLite_Connection.Close();
            }
            return id;
        }


        public decimal sum(string s)
        {
            decimal id;
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = SQLite_Connection;
            cmd.CommandText = s;
            try
            {
                SQLite_Connection.Open();
                id = Convert.ToDecimal(cmd.ExecuteScalar());

            }
            catch
            {
                id = 0;
            }
            finally
            {
                SQLite_Connection.Close();
            }
            return id;
        }

        public long get_single_field_row_count_with_condition(string field, string table, string condition_field, string condition)
        {
            return Convert.ToInt64(GetValue(query_builder("SELECT COUNT", field, table, condition_field, condition)));
        }

        public string query_builder(string type, string field, string table, string condition_field, string condition)
        {
            string result = "";
            switch (type)
            {
                case "SELECT COUNT":
                    result = "SELECT COUNT(" + field + ") FROM " + table + " WHERE (" + condition_field + " =" + condition + ");";
                    break;
                case "Another": break;
            }
            //MessageBox.Show(result);
            return result;
        }

        public string GetValue(String query)
        {

            SQLiteCommand cmd = new SQLiteCommand();
            SQLite_Connection.Open();

            string str;
            try
            {
                cmd = new SQLiteCommand(query, SQLite_Connection);
                str = cmd.ExecuteScalar().ToString();
            }
            catch (Exception x)
            {
                str = "0";
            }
            SQLite_Connection.Close();

            return str;
        }

        public bool Ins_Up_Del(String query)
        {
            bool result = false;
            try
            {
                SQLite_Connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, SQLite_Connection);
                cmd.ExecuteNonQuery();
                SQLite_Connection.Close();
                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Database Error, Reason : " + e.Message);

            }
            return result;

        }

    }
}
