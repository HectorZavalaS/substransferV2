using substransferV2.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace substransferV2.Controllers
{
    /// <summary>
    /// Descripción breve de getModels
    /// </summary>
    public class getModels : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            CMySQL m_db = new CMySQL();
            String json = "{";
            DataTable models = new DataTable();
            String html = "<option></option>";
            try
            {
                if(m_db.getModels(ref models))
                {
                    foreach(DataRow model in models.Rows)
                    {
                        html += "<option value='" + model["ID"].ToString()+  "'>";
                        html += model["NAME"].ToString();
                        html += "</option>";
                    }
                    json += "\"result\":\"true\",";
                    json += "\"html\":\"" + html + "\"";
                }
                else
                {
                    json += "\"result\":\"false\",";
                    json += "\"MessageError\":\"" + "No se pudieron obtener los modelos." + "\"";
                }

            }
            catch (Exception ex) {
                json += "\"result\":\"false\",";
                json += "\"MessageError\":\"" + ex.Message + "\"";
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