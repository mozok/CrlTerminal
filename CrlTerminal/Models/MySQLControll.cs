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
        private string ConnStr;

        public MySQLControll()
        {
            mySQLProperties = new MySQLProperties();

            ConnStr = mySQLProperties.ConnStrClone;
        }

        public void SpecListLoad(ObservableCollection<Specialization> sprSpec, Collection<Spec> spec)
        {
            SpecialisationList(sprSpec);

            SpecialistsList(spec);

            //sprSpec.Clear();

            //try
            //{
            //    using (MySqlConnection conn = new MySqlConnection(mySQLProperties.ConnStr))
            //    {
            //        conn.Open();

            //        string sql = "SELECT * FROM `enx4w_ttfsp_sprspec`";
            //        //string sql = "SELECT enx4w_ttfsp_spec.*, enx4w_ttfsp_sprspec.*  LEFT JOIN enx4w_ttfsp_spec ON enx4w_ttfsp_sprspec.id = ,enx4w_ttfsp_spec.idsprspec,";
            //        MySqlCommand cmd = new MySqlCommand(sql, conn);

            //        using (MySqlDataReader rdr = cmd.ExecuteReader())
            //        {
            //            while (rdr.Read())
            //            {
            //                sprSpec.Add(new SprSpec
            //                {
            //                    Id = rdr.GetInt32("id"),
            //                    Name = rdr.GetString("name"),
            //                    Desc = rdr.GetString("desc")
            //                });
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void SpecialisationList(ObservableCollection<Specialization> sprSpec)
        {
            sprSpec.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "SELECT * FROM `enx4w_ttfsp_sprspec`";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sprSpec.Add(new Specialization
                            {
                                Id = rdr.GetInt32("id"),
                                Name = rdr.GetString("name"),
                                Desc = rdr.GetString("desc")
                            });
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SpecialistsList(Collection<Spec> spec)
        {
            spec.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "SELECT * FROM `enx4w_ttfsp_spec`";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    conn.Open();

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

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SpecTimeLoad(ObservableCollection<AppointmentTime> ttfsp, DateTime regDate)
        {
            ttfsp.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "SELECT * FROM `enx4w_ttfsp` WHERE dttime=@regDate";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@regDate", regDate.Date);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ttfsp.Add(new AppointmentTime
                            {
                                Id = rdr.GetInt32("id"),
                                Idspec = rdr.GetInt32("idspec"),
                                Iduser = rdr.GetInt32("iduser"),
                                Reception = rdr.GetInt32("reception"),
                                Dttime = rdr.GetDateTime("dttime").Date,
                                Hrtime = rdr.GetString("hrtime"),
                                Mntime = rdr.GetString("mntime"),
                                Rfio = rdr.GetString("rfio"),
                                Rphone = rdr.GetString("rphone"),
                                Info = rdr.GetString("info"),
                                Rmail = rdr.GetString("rmail")
                            });
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UserListLoad(Collection<User> users)
        {
            users.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "SELECT * FROM `enx4w_users`";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            users.Add(new User
                            {
                                Id = rdr.GetInt32("id"),
                                Name = rdr.GetString("name"),
                                Username = rdr.GetString("username"),
                                Email = rdr.GetString("email"),
                                Fio = rdr.GetString("fio"),
                                Phone = rdr.GetString("phone")
                            });
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //public void 
    }
}