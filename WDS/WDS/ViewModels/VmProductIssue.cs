using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Models;

namespace WDS.ViewModels
{
    public class VmProductIssue
    {
        [Required]
        public long RetailerId { get; set; }
        [Required]
        public long SalesRepresentativeId { get; set; }
        [Required]
        public decimal GrandTotal { get; set; }
        public string PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        [Required]
        public DateTime SaleDate { get; set; }
        [Required]
        public List<ImeiDeliveryModel> ImeiDeliveryModels { get; set; }
        public List<RetailerOrderDetail> RetailerOrderDetails { get; set; }

        public VmProductIssue()
        {
            ImeiDeliveryModels = new List<ImeiDeliveryModel>();
            RetailerOrderDetails = new List<RetailerOrderDetail>();
        }
    }
}