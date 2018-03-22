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

        public void SpecListLoad (ObservableCollection<SprSpec> sprSpec, ObservableCollection<Spec> spec)
        {
            SpecialisationList(sprSpec);

            SpecialistsList(spec);
        }

        private void SpecialisationList(ObservableCollection<SprSpec> sprSpec)
        {
            sprSpec.Clear();

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
                            sprSpec.Add(new SprSpec
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

        private void SpecialistsList(ObservableCollection<Spec> spec)
        {
            spec.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySQLProperties.ConnStr))
                {
                    conn.Open();

                    string sql = "SELECT * FROM `enx4w_ttfsp_spec`";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            spec.Add(new Spec
                            {
                                Id = rdr.GetInt32("id"),
                                Idsprspec = rdr.GetString("idsprspec"),
                                Idsprsect = rdr.GetInt32("idsprsect"),
                                Idsprtime = rdr.GetInt32("idsprtime"),
                                Name = rdr.GetString("name"),
                                Photo = rdr.GetString("photo"),
                                Idusr = rdr.GetInt32("idusr"),
                                Number_cabinet = rdr.GetString("number_cabinet")
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