using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace substransferV2.Class
{
    public class CBox
    {
		String m_RESULT;
        String m_BOX_BARCODE;
        String m_BOX_NAME;
        int m_BOX_NUMBER;
        int m_BOX_QUANTITY;
        String m_MODEL_NAME;
        String m_BOX_INTERNAL_PN;
        String m_BOX_CLIENT_PN;
        String m_BIN;
        String m_DATEE;
        int m_STPACK;
        String m_total_boxes;

        public string RESULT { get => m_RESULT; set => m_RESULT = value; }
        public string BOX_BARCODE { get => m_BOX_BARCODE; set => m_BOX_BARCODE = value; }
        public string BOX_NAME { get => m_BOX_NAME; set => m_BOX_NAME = value; }
        public int BOX_NUMBER { get => m_BOX_NUMBER; set => m_BOX_NUMBER = value; }
        public int BOX_QUANTITY { get => m_BOX_QUANTITY; set => m_BOX_QUANTITY = value; }
        public string MODEL_NAME { get => m_MODEL_NAME; set => m_MODEL_NAME = value; }
        public string BOX_INTERNAL_PN { get => m_BOX_INTERNAL_PN; set => m_BOX_INTERNAL_PN = value; }
        public string BOX_CLIENT_PN { get => m_BOX_CLIENT_PN; set => m_BOX_CLIENT_PN = value; }
        public string BIN { get => m_BIN; set => m_BIN = value; }
        public string DATEE { get => m_DATEE; set => m_DATEE = value; }
        public int STPACK { get => m_STPACK; set => m_STPACK = value; }
        public string Total_boxes { get => m_total_boxes; set => m_total_boxes = value; }

    }
}