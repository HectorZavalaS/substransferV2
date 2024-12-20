using substransferV2.Class;
using substransferV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace substransferV2.Controllers
{
    /// <summary>
    /// Descripción breve de insertBox
    /// </summary>
    public class insertBox : IHttpHandler
    {
        siixsem_sys_lblPrint_dbEntities m_db = new siixsem_sys_lblPrint_dbEntities();
        COracle m_oracle = new COracle();
        CMySQL m_dbMysql = new CMySQL();
        public void ProcessRequest(HttpContext context)
        {
            String BOX_BARCODE = context.Request.Form["BOX_BARCODE"];
            String BOX_NAME = context.Request.Form["BOX_NAME"];
            int BOX_NUMBER = Convert.ToInt32(context.Request.Form["BOX_NUMBER"]);
            int BOX_QUANTITY = Convert.ToInt32(context.Request.Form["BOX_QUANTITY"]);
            String MODEL_NAME = context.Request.Form["MODEL_NAME"];
            String BOX_INTERNAL_PN = context.Request.Form["BOX_INTERNAL_PN"];
            String BOX_CLIENT_PN = context.Request.Form["BOX_CLIENT_PN"];
            String BIN = context.Request.Form["BIN"];
            String DATEE = context.Request.Form["DATEE"];
            int STPACK = Convert.ToInt32(context.Request.Form["STPACK"]);
            string json = "{";

            try
            {
                bool results = m_oracle.existsBox(BOX_BARCODE);
                //existsBox_Result res = results.First();
                if(results == false)
                {
                    m_oracle.insertBox(BOX_BARCODE, BOX_NAME, BOX_NUMBER.ToString(), BOX_QUANTITY.ToString(), MODEL_NAME, BOX_INTERNAL_PN, BOX_CLIENT_PN, BIN, DATEE, STPACK == 1 ? "Y": "N");
                    results = m_oracle.existsBox(BOX_BARCODE);
                    if (results == true)
                    {
                        json += "\"result\":\"true\"";
                    }
                    else
                    {
                        json += "\"result\":\"false\",";
                        json += "\"Message\":\"No se pudo insertar la caja a Simos. Intentalo nuevamente.\"";
                        m_dbMysql.deleteBox(BOX_BARCODE);
                    }

                }
                else
                {
                    json += "\"result\":\"false\",";
                    json += "\"Message\":\"" + "Ya se libero la caja." + "\"";
                }
            }
            catch(Exception ex) {
                json += "\"result\":\"false\",";
                json += "\"Message\":\"" + ex.Message + "\"";
            }
            json += "}";

            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}