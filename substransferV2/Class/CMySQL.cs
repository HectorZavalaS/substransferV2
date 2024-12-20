using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace substransferV2.Class
{
    public class CMySQL
    {
        private MySqlConnection m_conn;
        private DataTable m_data;
        private MySqlDataAdapter m_da;
        private MySqlCommand m_cb;
        private String m_cadC;
        private String m_cadJpack;
        private MySqlCommand m_cbJPack;
        private MySqlConnection m_connPack;

        public CMySQL()
        {
            m_cadC = String.Format("server={0};user id={1}; password={2}; database=siixsem_main_db; pooling=false",
                            "192.168.3.13", "root", "S3m4dm1n2017!");
            m_cb = new MySqlCommand();

            m_cadJpack = String.Format("server={0};user id={1}; password={2}; database=jpacking; pooling=false",
                            "192.168.3.13", "root", "S3m4dm1n2017!");
            m_cbJPack = new MySqlCommand();
        }

        public bool connect()
        {
            bool result = false;
            try
            {
                m_conn = new MySqlConnection(m_cadC);
                m_conn.Open();
                result = true;
            }
            catch (MySqlException ex)
            {
               // MessageBox.Show("Error connecting to the server: " + ex.Message);
            }
            return result;
        }

        public bool connectJPack()
        {
            bool result = false;
            try
            {
                m_connPack = new MySqlConnection(m_cadJpack);
                m_connPack.Open();
                result = true;
            }
            catch (MySqlException ex)
            {
                // MessageBox.Show("Error connecting to the server: " + ex.Message);
            }
            return result;
        }

        public bool executeSPWhitoutP(String nameProc)
        {
            bool result = false;
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;

                m_cb.CommandText = nameProc;
                m_cb.CommandType = CommandType.StoredProcedure;

                m_cb.ExecuteNonQuery();

                //Console.WriteLine("Employee number: " + cmd.Parameters["@empno"].Value);
                //Console.WriteLine("Birthday: " + cmd.Parameters["@bday"].Value);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }

        public bool getModels(ref DataTable models)
        {
            bool result = false;
            try
            {
                models.Columns.Add("ID");
                models.Columns.Add("NAME");
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;

                int dia = DateTime.Now.DayOfYear;

                string query = "SELECT id_model AS ID, model AS NAME FROM siixsem_main_db.siixsem_master_information WHERE model is not null";

                MySqlCommand mycomand = new MySqlCommand(query, m_cb.Connection);


                MySqlDataReader myreader = mycomand.ExecuteReader();

                if (myreader.HasRows)
                {
                    while (myreader.Read())
                    {
                        DataRow workRow = models.NewRow();
                        workRow["ID"] = myreader["ID"].ToString();
                        workRow["NAME"] = myreader["NAME"].ToString(); ;
                        models.Rows.Add(workRow);
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
        public bool getLastSerial150(String djGroup, ref String lastSerial)
        {
            bool result = false;
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;

                int dia = DateTime.Now.DayOfYear;

                string query = "SELECT Max(contador) as contador FROM siixsem_xml_laser.siixsem_150b_ldm_t where julian_date = ?jd";

                MySqlCommand mycomand = new MySqlCommand(query, m_cb.Connection);
                mycomand.Parameters.AddWithValue("?jd", dia.ToString());


                MySqlDataReader myreader = mycomand.ExecuteReader();
                //myreader.Read();

                if (myreader.HasRows)
                {
                    while (myreader.Read())
                    {
                        lastSerial = myreader["contador"].ToString();
                    }
                    result = true;
                    if (String.IsNullOrEmpty(lastSerial)) lastSerial = "1";
                }
                else
                {
                    lastSerial = "1";
                    result = true;
                }

                //return datos;

                //Console.WriteLine("Employee number: " + cmd.Parameters["@empno"].Value);
                //Console.WriteLine("Birthday: " + cmd.Parameters["@bday"].Value);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
        public bool insertLastSerial150(String contador, String julianDay, String batchQty, String djGroup)
        {
            bool result = false;
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;

                int dia = DateTime.Now.DayOfYear;

                string query = "INSERT INTO siixsem_xml_laser.siixsem_150b_ldm_t (contador, julian_date,cantidad,dj) VALUES (?cont,?jd,?cant,?dj)";

                MySqlCommand mycomand = new MySqlCommand(query, m_cb.Connection);
                mycomand.Parameters.AddWithValue("?cont", contador);
                mycomand.Parameters.AddWithValue("?jd", julianDay);
                mycomand.Parameters.AddWithValue("?cant", batchQty);
                mycomand.Parameters.AddWithValue("?dj", djGroup);


                MySqlDataReader myreader = mycomand.ExecuteReader();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
        public bool insertSerialBox(CBox box, String JD, String serial, String user)
        {
            bool result = false;
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;

                int dia = DateTime.Now.DayOfYear;

                //string query = "INSERT INTO siixsem_xml_laser.siixsem_150b_ldm_t (contador, julian_date,cantidad,dj) VALUES (?cont,?jd,?cant,?dj)";
                string query = "INSERT INTO siixsem_oqc_validated (box_barcode, model, cust_part_no, int_part_no, box_qty, julian_date, print_date, box_no, attribute1,attribute5, created_by) VALUES (?p_boxserial,?p_model,?PNE,?PNI,?ST,?JD,?DA,?CNO,?BINN,?p_serial,?p_user)";
                MySqlCommand mycomand = new MySqlCommand(query, m_cb.Connection);
                mycomand.Parameters.AddWithValue("?p_boxserial", box.BOX_BARCODE);
                mycomand.Parameters.AddWithValue("?p_model", box.MODEL_NAME);
                mycomand.Parameters.AddWithValue("?PNE", box.BOX_CLIENT_PN);
                mycomand.Parameters.AddWithValue("?PNI", box.BOX_INTERNAL_PN);
                mycomand.Parameters.AddWithValue("?ST", box.BOX_QUANTITY);
                mycomand.Parameters.AddWithValue("?JD", JD);
                mycomand.Parameters.AddWithValue("?DA", box.DATEE);
                mycomand.Parameters.AddWithValue("?CNO", box.BOX_NUMBER);
                mycomand.Parameters.AddWithValue("?BINN", box.BIN);
                mycomand.Parameters.AddWithValue("?p_serial", serial);
                mycomand.Parameters.AddWithValue("?p_user", user);

                MySqlDataReader myreader = mycomand.ExecuteReader();
                result = true;

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                if (ex.Message.Contains("Duplicate entry"))
                    box.RESULT = "Esta caja ya se libero.";
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
        public bool validateBox(String model_name,String boxSerial, String pcbSerial,String user, int param1, int param2,ref CBox box)
        {
            bool result = false;
            CBox m_box = new CBox();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;


                m_cb.CommandText = "siixsem_queryInf_sp";
                m_cb.CommandType = CommandType.StoredProcedure;
                m_cb.Parameters.AddWithValue("p_model", model_name);
                m_cb.Parameters.AddWithValue("p_boxserial", boxSerial);
                m_cb.Parameters.AddWithValue("p_serial", pcbSerial);
                m_cb.Parameters.AddWithValue("p_user", user);
                m_cb.Parameters.AddWithValue("PARAMETRO1", param1);
                m_cb.Parameters.AddWithValue("PARAMETRO2", param2);

                //m_cb.ExecuteNonQuery();
                MySqlDataReader reader = m_cb.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                     {
                        m_box.RESULT = reader.GetString(0);
                        if (m_box.RESULT.Equals("100"))
                        {
                           //'100' AS RESULT,
                           //p_boxserial AS BOX_BARCODE,
                           //p_boxserial AS BOX_NAME,
                           //CNO AS BOX_NUMBER,
                           //ST AS BOX_QUANTITY,
                           //p_model AS MODEL_NAME,
                           //PNI AS BOX_INTERNAL_PN,
                           //PNE AS BOX_CLIENT_PN,
                           //BINN AS BIN,
                           //DA AS DATEE,
                           //ISPartial AS STPACK;

                            m_box.BOX_BARCODE = reader.GetString(1);
                            m_box.BOX_NAME = reader.GetString(2);
                            m_box.BOX_NUMBER = reader.GetInt32(3);
                            m_box.BOX_QUANTITY = reader.GetInt32(4);
                            m_box.MODEL_NAME = reader.GetString(5);
                            m_box.BOX_INTERNAL_PN = reader.GetString(6);
                            m_box.BOX_CLIENT_PN = reader.GetString(7);
                            m_box.BIN = reader.GetString(8);
                            m_box.DATEE = reader.GetString(9);
                            m_box.STPACK = reader.GetString(10) == "Y" ? 1 : 0;
                            result = true;
                        }
                        else
                        {
                            m_box.BOX_BARCODE = "NA";
                            m_box.BOX_NAME = "NA";
                            m_box.BOX_NUMBER = 0;
                            m_box.BOX_QUANTITY = 0;
                            m_box.MODEL_NAME = "NA";
                            m_box.BOX_INTERNAL_PN = "NA";
                            m_box.BOX_CLIENT_PN = "NA";
                            m_box.BIN = "NA";
                            m_box.DATEE = "NA";
                            m_box.STPACK = 0;
                            result = false;
                        }
                        box = m_box;
                    }
                }


                //Console.WriteLine("Employee number: " + cmd.Parameters["@empno"].Value);
                //Console.WriteLine("Birthday: " + cmd.Parameters["@bday"].Value);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    m_box.RESULT = "La etiqueta ya existe en MySQL";
                    m_box.BOX_BARCODE = "NA";
                    m_box.BOX_NAME = "NA";
                    m_box.BOX_NUMBER = 0;
                    m_box.BOX_QUANTITY = 0;
                    m_box.MODEL_NAME = "NA";
                    m_box.BOX_INTERNAL_PN = "NA";
                    m_box.BOX_CLIENT_PN = "NA";
                    m_box.BIN = "NA";
                    m_box.DATEE = "NA";
                    m_box.STPACK = 0;
                    box = m_box;
                }
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
        public bool isDuplicate(String model, String boxSerial, ref CBox box)
        {
            bool result = false;
            String side = "";
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;

                int dia = DateTime.Now.DayOfYear;

                string query = "SELECT * FROM siixsem_main_db.siixsem_oqc_validated where box_barcode = ?bs";

                MySqlCommand mycomand = new MySqlCommand(query, m_cb.Connection);
                mycomand.Parameters.AddWithValue("?bs", boxSerial);

                if (boxSerial.Contains("LH")) { side = "LH"; }
                else if (boxSerial.Contains("RH")) { side = "RH"; }
                     else side = "NA";



                MySqlDataReader reader = mycomand.ExecuteReader();
                //myreader.Read();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        box.BOX_NUMBER = reader.GetInt32(8);
                        box.DATEE = reader.GetString(6);
                        box.BOX_CLIENT_PN = reader.GetString(3);

                        query = "SELECT * FROM siixsem_main_db.siixsem_master_information where model = ?model";

                        mycomand.Parameters.Clear();

                        mycomand.CommandText = query;
                        //MySqlCommand comand = new MySqlCommand(query, m_cb.Connection);
                        mycomand.Parameters.AddWithValue("?model", model);
                        reader.Close();
                        reader = mycomand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read()) {
                                String tableName = reader.GetString(18);
                                String stpack = reader.GetString(14);

                                if (side.Equals("NA"))
                                    query = "SELECT count(*) AS NUM_REG FROM " + tableName + " where julian_date = ?jd and c_no = ?bxNo";
                                else
                                    query = "SELECT count(*) AS NUM_REG FROM " + tableName + " where julian_date = ?jd and c_no = ?bxNo and attribute1 = ?side";

                                connectJPack();
                                m_cbJPack.Connection = m_connPack;

                                MySqlCommand comandPack = new MySqlCommand(query, m_cbJPack.Connection);
                                comandPack.Parameters.AddWithValue("?jd", box.DATEE);
                                comandPack.Parameters.AddWithValue("?bxNo", box.BOX_NUMBER);
                                if (!side.Equals("NA"))
                                    comandPack.Parameters.AddWithValue("?side", side);

                                MySqlDataReader jpackReader = comandPack.ExecuteReader();

                                if (jpackReader.HasRows)
                                {
                                    while (jpackReader.Read()) {
                                        int numPCB = jpackReader.GetInt32(0);
                                        if (numPCB > Convert.ToInt32(stpack)) result = true;
                                        else result = false;
                                    }
                                }
                            }
                        }
                        else result = false;
                        m_connPack.Close();
                        break;
                    }
                    //result = true;
                }
                else
                {
                    //lastSerial = "1";
                    result = false;
                }

                //return datos;

                //Console.WriteLine("Employee number: " + cmd.Parameters["@empno"].Value);
                //Console.WriteLine("Birthday: " + cmd.Parameters["@bday"].Value);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
        public bool deleteBox(String boxSerial)
        {
            bool result = false;

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connect();
                m_cb.Connection = m_conn;
                CBox m_box = new CBox();

                m_cb.CommandText = "delete_box_oqc";
                m_cb.CommandType = CommandType.StoredProcedure;
                m_cb.Parameters.AddWithValue("p_box_serial", boxSerial);

                m_cb.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
                result = false;
            }
            m_conn.Close();
            Console.WriteLine("Done.");
            return result;
        }
    }
}