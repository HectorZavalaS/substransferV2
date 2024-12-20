using substransferV2.Class;
using substransferV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace substransferV2.Controllers
{
    /// <summary>
    /// Descripción breve de validateBox
    /// </summary>
    public class validateBox : IHttpHandler
    {
        CMySQL m_db = new CMySQL();
        COracle m_oracle = new COracle();
        public void ProcessRequest(HttpContext context)
        {
            String model_name = context.Request.Form["model_name"];
            String box_serial = context.Request.Form["box_serial"];
            String pcb_serial = context.Request.Form["pcb_serial"];
            String user = context.Request.Form["user"];
            bool response = false;
            String json = "{";
            int isPartial = box_serial.Contains("PARTIAL") == true ? 1 : 0;
            int isEngC = box_serial.Contains("ENG. CHANGE") == true ? 1 : 0;
            CBox box = new CBox();
            siixsem_upacking_config_dbEntities m_setup =  new siixsem_upacking_config_dbEntities();
            siixsem_upacking_data_dbEntities m_data = new siixsem_upacking_data_dbEntities();
            try
            {
                // bool existsBoxPick = m_oracle.existsBoxPick(box_serial);
                bool results = m_oracle.existsBox(box_serial);
                if (results)
                {
                        box.RESULT = "Esta caja ya existe en SIMOS(SERIAL DUPLICADO).";
                        json += "\"Message\":\"" + "Esta caja ya existe en SIMOS(SERIAL DUPLICADO)." + "\",";
                        json += "\"result\":\"false\"";
                }
                else
                if (!m_db.isDuplicate(model_name, box_serial, ref box)) { 
                    //if (!existsBoxPick)
                    //{
                        response = m_db.validateBox(model_name, box_serial, pcb_serial, user, isPartial, isEngC, ref box);

                         if (box.RESULT.Contains("No hay PCB ligada a este Cover"))
                        {
                            String []cust = box.RESULT.Split('_');
                            getModelByCustomerNo_Result resCust = m_setup.getModelByCustomerNo(cust[1]).First();
                            if (resCust != null)
                            {
                                if(!resCust.CUSTOMER_NO.Equals("NOT_FOUND")){
                                    if (!box_serial.Contains(resCust.INTERNAL_NO)) {
                                        box.RESULT = "Número de parte Interno incorrecto. UP";
                                        response = false;
                                    }
                                    else { 
                                        if (!box_serial.Contains(resCust.CUSTOMER_NO)) {
                                            box.RESULT = "Número de parte de Cliente incorrecto. UP";
                                            response = false;
                                        }
                                        else{
                                            getSerialPCBByCov_Result pcb = m_data.getSerialPCBByCov(pcb_serial, resCust.TABLE_COVER).First();

                                            if (pcb.PCB_SERIAL.Equals("NOT_EXISTS")) {
                                                box.RESULT = "No hay PCB ligada a este Cover. UP";
                                            }
                                            else { 
                                                if(pcb.PCB_SERIAL.Contains(resCust.TRACEABILITY))
                                                {
                                                    box.BOX_BARCODE = box_serial;
                                                    box.BOX_NAME = box_serial;
                                                    box.BOX_NUMBER = Convert.ToInt32(cust[2]);
                                                    box.BOX_QUANTITY = Convert.ToInt32(cust[3]);
                                                    box.MODEL_NAME = cust[4];
                                                    box.BOX_INTERNAL_PN = resCust.INTERNAL_NO;
                                                    box.BOX_CLIENT_PN = resCust.CUSTOMER_NO;
                                                    box.BIN = cust[6];
                                                    box.DATEE = cust[5];
                                                    box.STPACK = box_serial.Contains("PARTIAL") == true ? 1 : 0;
                                                    box.RESULT = "";
                                                    if (m_db.insertSerialBox(box, cust[7], pcb_serial, user))
                                                    {
                                                        response = true;

                                                    }
                                                    else {
                                                        response = false;
                                                        if (!box.RESULT.Contains("Esta caja ya se libero"))
                                                            box.RESULT = "Ocurrio un error al insertar serial de Caja en MySql. UP";
                                                    }
                                                }
                                                else{
                                                    response = false;
                                                    box.RESULT = "Número de trazabilidad incorrecto. UP";
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        if (response)
                            json += "\"result\":\"true\",";
                        else
                        {
                            results = m_oracle.existsBox(box_serial);
                            
                            if (box.RESULT.Contains("La etiqueta ya existe en MySQL"))
                            {
                                    box.RESULT = "La etiqueta ya existe en MySQL.";
                                    json += "\"result\":\"false\",";
                            }
                            else
                            if (box.RESULT.Contains("Esta caja ya ha sido liberada"))
                            {
                                if (!results)
                                {
                                    m_db.deleteBox(box_serial);
                                    m_db.validateBox(model_name, box_serial, pcb_serial, user, isPartial, isEngC, ref box);
                                    json += "\"result\":\"true\",";
                                    //box.RESULT = "No se pudo insertar la caja a SIMOS. Intentalo nuevamente.";
                                }
                                else
                                {
                                    box.RESULT = "Esta caja ya existe en SIMOS, esta lista para Boxtransfer.";
                                    json += "\"result\":\"false\",";
                                }
                            }
                            else json += "\"result\":\"false\",";

                        }

                        json += "\"BOX_BARCODE\":\"" + box.BOX_BARCODE + "\",";
                        json += "\"BOX_NAME\":\"" + box.BOX_NAME + "\",";
                        json += "\"BOX_NUMBER\":\"" + box.BOX_NUMBER + "\",";
                        json += "\"BOX_QUANTITY\":\"" + box.BOX_QUANTITY + "\",";
                        json += "\"MODEL_NAME\":\"" + box.MODEL_NAME + "\",";
                        json += "\"BOX_INTERNAL_PN\":\"" + box.BOX_INTERNAL_PN + "\",";
                        json += "\"BOX_CLIENT_PN\":\"" + box.BOX_CLIENT_PN + "\",";
                        json += "\"BIN\":\"" + box.BIN + "\",";
                        json += "\"DATEE\":\"" + box.DATEE + "\",";
                        json += "\"STPACK\":\"" + box.STPACK + "\",";
                        json += "\"Message\":\"" + box.RESULT + "\"";
                //    }
                //    else
                //    {
                //        json += "\"result\":\"false\",";
                //        json += "\"Message\":\"" + "CAJA DUPLICADA" + "\"";
                //    }
                }
                else
                {
                    json += "\"result\":\"false\",";
                    json += "\"Message\":\"" + "CAJA DUPLICADA" + "\"";
                }
            }
            catch(Exception ex)
            {
                //if(model_name.Contains("SIT"))
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