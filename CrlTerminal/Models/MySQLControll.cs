﻿using Prism.Mvvm;
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
                                Number_cabinet = rdr.GetString("number_cabinet"),
                                SpecMail = rdr.GetString("specmail")
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

        public void SpecTimeLoad(ObservableCollection<AppointmentTime> ttfsp, DateTime regDate, int specId)
        {
            ttfsp.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "SELECT * FROM `enx4w_ttfsp` WHERE dttime=@regDate AND idspec = @idspec";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@regDate", regDate.Date);
                    cmd.Parameters.AddWithValue("@idspec", specId);

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

        public void InsertAppointment(User user, AppointmentTime appointmentTime, Spec spec)
        {
            try
            {
                string tempOrder = GetNumberOrder();
                string[] tempArray = tempOrder.Split(new char[] { '-' });
                tempOrder = tempArray[0];

                string numberOrder = (Convert.ToInt32(tempOrder) + 1).ToString() + "-TER";

                updateIntoDatabase(user, appointmentTime, spec);

                //string[] _dateTemp = appointmentTime.Dttime.Date.ToShortDateString().Split(new char[] { '.' });
                //string _date = _dateTemp[2] + "-" + _dateTemp[1] + "-" + _dateTemp[0];
                string _date = appointmentTime.Dttime.ToString("yyyy-MM-dd");
                string _hours = appointmentTime.Hrtime;
                string _minutes = appointmentTime.Mntime;
                string _office_address = "м.Шостка, вул. Щедріна, 1 Телефони:\r+ 38(05449) 3 - 28 - 95,\r+38(05449) 3-23-52";

                string _info = _date + " " + _hours + ":" + _minutes + " <br /><u>ПІБ: </u> " +
                               user.Name + " <br /><u>Телефон: </u>" + user.Phone;

                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "INSERT INTO enx4w_ttfsp_dop (iduser, id_specialist, rfio, rphone, info, ipuser, rmail, number_order, cdate, date, hours, minutes, office_name, specializations_name, specialist_name, specialist_email, order_password, office_address, number_cabinet) " +
                                "VALUES(@iduser, @id_specialist, @rfio, @rphone, @info, @ipuser, @rmail, @number_order, @cdate, @date, @hours, @minutes, @office_name, @specializations_name, @specialist_name, @specialist_email, @order_password, @office_address, @number_cabinet)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@iduser", user.Id);
                    cmd.Parameters.AddWithValue("@id_specialist", appointmentTime.Idspec);
                    cmd.Parameters.AddWithValue("@rfio", user.Name);
                    cmd.Parameters.AddWithValue("@rphone", user.Phone);
                    cmd.Parameters.AddWithValue("@info", _info);
                    cmd.Parameters.AddWithValue("@ipuser", "localterminal");
                    cmd.Parameters.AddWithValue("@rmail", user.Email);
                    cmd.Parameters.AddWithValue("@number_order", numberOrder);
                    cmd.Parameters.AddWithValue("@cdate", 1485730860);
                    cmd.Parameters.AddWithValue("@date", _date);
                    cmd.Parameters.AddWithValue("@hours", _hours);
                    cmd.Parameters.AddWithValue("@minutes", _minutes);
                    cmd.Parameters.AddWithValue("@office_name", "Полікліники №4");
                    cmd.Parameters.AddWithValue("@specializations_name", spec.Specialization);
                    cmd.Parameters.AddWithValue("@specialist_name", spec.Name);
                    cmd.Parameters.AddWithValue("@specialist_email", spec.SpecMail);
                    cmd.Parameters.AddWithValue("@order_password", "V9EFJP");
                    cmd.Parameters.AddWithValue("@office_address", _office_address);
                    cmd.Parameters.AddWithValue("@number_cabinet", spec.Number_cabinet);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetNumberOrder()
        {
            string temp = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "SELECT number_order FROM enx4w_ttfsp_dop ORDER BY id DESC LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    conn.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            temp = rdr.GetString("number_order");
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return temp;
        }

        private void updateIntoDatabase(User user, AppointmentTime appointmentTime, Spec spec)
        {
            try
            {
                string _date = appointmentTime.Dttime.ToString("yyyy-MM-dd");
                _date = _date.Replace(".", "-");
                string _hours = appointmentTime.Hrtime;
                string _minutes = appointmentTime.Mntime;
                
                string _info = _date + " " + _hours + ":" + _minutes + " <br /><u>ПІБ: </u> " +
                               user.Name + " <br /><u>Телефон: </u>" + user.Phone;

                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = "UPDATE enx4w_ttfsp SET iduser=@iduser, reception='1', rfio=@rfio, rphone=@rphone, info=@info, ipuser=@ipuser, rmail=@rmail" +
                                " WHERE idspec=@idspec AND dttime=@dttime AND hrtime=@hrtime AND mntime=@mntime";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@iduser", user.Id);
                    cmd.Parameters.AddWithValue("@rfio", user.Name);
                    cmd.Parameters.AddWithValue("@rphone", user.Phone);
                    cmd.Parameters.AddWithValue("@info", _info);
                    cmd.Parameters.AddWithValue("@ipuser", "localterminal");
                    cmd.Parameters.AddWithValue("@rmail", user.Email);
                    cmd.Parameters.AddWithValue("@idspec", spec.Id);
                    cmd.Parameters.AddWithValue("@dttime", _date);
                    cmd.Parameters.AddWithValue("@hrtime", _hours);
                    cmd.Parameters.AddWithValue("@mntime", _minutes);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}