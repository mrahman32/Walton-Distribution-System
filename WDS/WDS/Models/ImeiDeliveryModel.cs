using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class ImeiDeliveryModel
    {
        [Required]
        public string Model { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string ImeiOne { get; set; }
        [Required]
        public string ImeiTwo { get; set; }
    }
}