﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace substransferV2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class siixsem_sys_lblPrint_dbEntities : DbContext
    {
        public siixsem_sys_lblPrint_dbEntities()
            : base("name=siixsem_sys_lblPrint_dbEntities")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 380;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<existsBox_Result> existsBox(string serial)
        {
            var serialParameter = serial != null ?
                new ObjectParameter("serial", serial) :
                new ObjectParameter("serial", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<existsBox_Result>("existsBox", serialParameter);
        }
    
        public virtual int insertPackingHDR(string bOX_BARCODE, string bOX_NAME, string bOX_NUMBER, string bOX_QUANTITY, string mODEL_NAME, string bOX_INTERNAL_PN, string bOX_CLIENT_PN, string bIN, string dATEE, string sTPACK)
        {
            var bOX_BARCODEParameter = bOX_BARCODE != null ?
                new ObjectParameter("BOX_BARCODE", bOX_BARCODE) :
                new ObjectParameter("BOX_BARCODE", typeof(string));
    
            var bOX_NAMEParameter = bOX_NAME != null ?
                new ObjectParameter("BOX_NAME", bOX_NAME) :
                new ObjectParameter("BOX_NAME", typeof(string));
    
            var bOX_NUMBERParameter = bOX_NUMBER != null ?
                new ObjectParameter("BOX_NUMBER", bOX_NUMBER) :
                new ObjectParameter("BOX_NUMBER", typeof(string));
    
            var bOX_QUANTITYParameter = bOX_QUANTITY != null ?
                new ObjectParameter("BOX_QUANTITY", bOX_QUANTITY) :
                new ObjectParameter("BOX_QUANTITY", typeof(string));
    
            var mODEL_NAMEParameter = mODEL_NAME != null ?
                new ObjectParameter("MODEL_NAME", mODEL_NAME) :
                new ObjectParameter("MODEL_NAME", typeof(string));
    
            var bOX_INTERNAL_PNParameter = bOX_INTERNAL_PN != null ?
                new ObjectParameter("BOX_INTERNAL_PN", bOX_INTERNAL_PN) :
                new ObjectParameter("BOX_INTERNAL_PN", typeof(string));
    
            var bOX_CLIENT_PNParameter = bOX_CLIENT_PN != null ?
                new ObjectParameter("BOX_CLIENT_PN", bOX_CLIENT_PN) :
                new ObjectParameter("BOX_CLIENT_PN", typeof(string));
    
            var bINParameter = bIN != null ?
                new ObjectParameter("BIN", bIN) :
                new ObjectParameter("BIN", typeof(string));
    
            var dATEEParameter = dATEE != null ?
                new ObjectParameter("DATEE", dATEE) :
                new ObjectParameter("DATEE", typeof(string));
    
            var sTPACKParameter = sTPACK != null ?
                new ObjectParameter("STPACK", sTPACK) :
                new ObjectParameter("STPACK", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insertPackingHDR", bOX_BARCODEParameter, bOX_NAMEParameter, bOX_NUMBERParameter, bOX_QUANTITYParameter, mODEL_NAMEParameter, bOX_INTERNAL_PNParameter, bOX_CLIENT_PNParameter, bINParameter, dATEEParameter, sTPACKParameter);
        }
    }
}
