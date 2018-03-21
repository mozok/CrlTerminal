using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace CrlTerminal.Models
{
    public class MySQLControll : BindableBase
    {
        private MySQLProperties mySQLProperties;
        
        public MySQLControll()
        {
            mySQLProperties = new MySQLProperties();
        }

        public void SpecListLoad (ObservableCollection<sprSpec> sprSpec, ObservableCollection<spec> spec)
        {
            sprSpec.Clear();
            spec.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySQLProperties.ConnStr))
                {
                    conn.Open();

                    string sql = "SELECT * FROM `enx4w_ttfsp_sprspec`";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sprSpec.Add(new sprSpec
                            {
                                Id = rdr.GetInt32("id"),
                                Name = rdr.GetString("name"),
                                Desc = rdr.GetString("desc")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}