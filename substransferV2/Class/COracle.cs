using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace substransferV2.Class
{
    public class COracle
    {
        String m_server;
        String m_SID;
        private String m_user;
        private String m_pass;
        OracleConnection m_OracleDB;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string Server { get => m_server; set => m_server = value; }
        public string SID { get => m_SID; set => m_SID = value; }

        public COracle()
        {
            m_server = "192.168.0.23";
            m_SID = "SEMPROD";
            m_user = "APPS";
            m_pass = "apps";
            m_OracleDB = GetDBConnection(Server, 0, SID, m_user, m_pass);
            m_OracleDB.Open();
        }
        public COracle(String serv, String Sid)
        {
            m_server = serv;
            m_SID = Sid;
            m_user = "APPS";
            m_pass = "apps";
            m_OracleDB = GetDBConnection(Server, 0, SID, m_user, m_pass);
            m_OracleDB.Open();
        }

        private OracleConnection GetDBConnection(string host, int port, String sid, String user, String password)
        {


            Console.WriteLine("Getting Connection ...");

            // 'Connection string' to connect directly to Oracle.
            string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + Server + ")(PORT = " + "1521" + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + SID + ")));Password=" + m_pass + ";User ID=" + m_user + ";Enlist=false;Pooling=true";

            OracleConnection conn = new OracleConnection();
            try
            {
                conn.ConnectionString = connString;
            }
            catch (Exception ex)
            {
                conn = null;
                logger.Error(ex, "Error al conectarse a base de datos de Oracle");
            }

            return conn;
        }
        public bool QuerySerial(String serial, ref int resultTest)
        {
            bool result = false;
            string sql = "SELECT * FROM insp_result_summary_info where board_barcode in ('" + serial.ToUpper() + "')"; ;

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;

                        while (reader.Read())
                        {
                            int IRCODEIndex = reader.GetOrdinal("INSP_RESULT_CODE");
                            int VLRCODEIndex = reader.GetOrdinal("VC_LAST_RESULT_CODE");

                            long? INSP_RESULT_CODE = null;
                            long? VC_LAST_RESULT_CODE = null;

                            if (!reader.IsDBNull(IRCODEIndex))
                                INSP_RESULT_CODE = Convert.ToInt64(reader.GetValue(IRCODEIndex));
                            if (!reader.IsDBNull(VLRCODEIndex))
                                VC_LAST_RESULT_CODE = Convert.ToInt64(reader.GetValue(VLRCODEIndex));

                            if (INSP_RESULT_CODE == 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 1;   //// OK
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE != 0)
                                resultTest = 2;   //// NG
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == 0)
                                resultTest = 3;   //// FALSE CALL (OK)
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 4;   //// NO JUZGADA

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error en serial: " + serial);
                result = false;
            }
            return result;

        }

        public bool QuerySerials(List<String> serials, ref int resultTest)
        {
            bool result = false;
            String qSerials = "";

            foreach (String serial in serials)
            {
                qSerials += "'" + serial.ToUpper() + "',";
            }

            string sql = "SELECT * FROM insp_result_summary_info where board_barcode in (" + qSerials.Substring(0, qSerials.Length - 1) + ")"; ;

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;

                        while (reader.Read())
                        {
                            int IRCODEIndex = reader.GetOrdinal("INSP_RESULT_CODE");
                            int VLRCODEIndex = reader.GetOrdinal("VC_LAST_RESULT_CODE");

                            long? INSP_RESULT_CODE = null;
                            long? VC_LAST_RESULT_CODE = null;

                            if (!reader.IsDBNull(IRCODEIndex))
                                INSP_RESULT_CODE = Convert.ToInt64(reader.GetValue(IRCODEIndex));
                            if (!reader.IsDBNull(VLRCODEIndex))
                                VC_LAST_RESULT_CODE = Convert.ToInt64(reader.GetValue(VLRCODEIndex));

                            if (INSP_RESULT_CODE == 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 1;   //// OK
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE != 0)
                                resultTest = 2;   //// NG
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == 0)
                                resultTest = 3;   //// FALSE CALL (OK)
                            if (INSP_RESULT_CODE != 0 && VC_LAST_RESULT_CODE == null)
                                resultTest = 4;   //// NO JUZGADA
                            //break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[QuerySerial] Error ");
                result = false;
            }
            return result;

        }
        public bool existsBox(String SerialBox)
        {
            bool result = false;


            String query = "SELECT BOX_BARCODE FROM SIIXSEM.PACKING_HDR WHERE BOX_BARCODE = '" + SerialBox + "'";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = query;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                    else result = false;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "[getSimosOnHand] Error ");
                result = false;
            }
            return result;

        }
        public bool existsBoxPick(String SerialBox)
        {
            bool result = false;


            String query = "SELECT SCANNED_BOX_NAME FROM SIIXSEM.BOX_PICKING WHERE SCANNED_BOX_NAME = '" + SerialBox + "'";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = query;


                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                    else result = false;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "[getSimosOnHand] Error ");
                result = false;
            }
            return result;

        }

        //@BOX_BARCODE NVARCHAR(150),
        //@BOX_NAME NVARCHAR(70),
        //@BOX_NUMBER AS NVARCHAR(10),
        //@BOX_QUANTITY AS Nvarchar(10),
        //@MODEL_NAME AS NVARCHAR(70),
        //@BOX_INTERNAL_PN AS NVARCHAR(50),
        //@BOX_CLIENT_PN AS NVARCHAR(50),
        //@BIN AS NVARCHAR(30),
        //@DATEE AS NVARCHAR(70),
        //@STPACK AS NVARCHAR(10)

        public bool insertBox(String BOX_BARCODE, String BOX_NAME, String BOX_NUMBER, String BOX_QUANTITY, String MODEL_NAME, String BOX_INTERNAL_PN, String BOX_CLIENT_PN, String BIN, String DATEE, String STPACK)
        {
            bool result = false;


            String query = "INSERT INTO SIIXSEM.PACKING_HDR (BOX_BARCODE, BOX_NAME, BOX_NUMBER, BOX_QUANTITY,MODEL_NAME,BOX_INTERNAL_PN,BOX_CLIENT_PN,	BIN, CREATED_DT,PRODUCTION_DT) VALUES('" +  BOX_BARCODE + "', '"+ BOX_NAME + "', " + BOX_NUMBER + ", " + BOX_QUANTITY + ", '" + MODEL_NAME + "', '" + BOX_INTERNAL_PN + "', '" + BOX_CLIENT_PN + "', '" + BIN + "',SYSDATE, '" + DATEE + "') ";

            try
            {
                // Create command.
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = query;


                cmd.ExecuteNonQuery();
                //query = "COMMIT";
                //cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //logger.Error(ex, "[getSimosOnHand] Error ");
                result = false;
            }
            return result;

        }


        public void Close()
        {
            m_OracleDB.Dispose();
            m_OracleDB.Close();
            OracleConnection.ClearPool(m_OracleDB);
        }
    }
}