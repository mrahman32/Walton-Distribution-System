using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class MOC_ChequesModel
    {
        public long Id { get; set; }
        public long? MOCId { get; set; }
        public string ChequeNo { get; set; }
        public decimal ChecqueAmount { get; set; }
        public string ChequeFileUrl { get; set; }
        public System.DateTime AddedDate { get; set; }
        public long AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdateName { get; set; }


        public HttpPostedFileBase ChequeFileBase { get; set; }
        public string ChequeWebUrl { get; set; }
    }
}