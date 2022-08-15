using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WDS.Infrastructure.Helper
{
    public class ServiceClass
    {
        [XmlElement("MessageId")]
        public long MessageId { get; set; }
        [XmlElement("Status")]
        public int Status { get; set; }
        [XmlElement("StatusText")]
        public string StatusText { get; set; }
        [XmlElement("ErrorCode")]
        public int ErrorCode { get; set; }
        [XmlElement("ErrorText")]
        public string ErrorText { get; set; }
        [XmlElement("SMSCount")]
        public int SmsCount { get; set; }
        [XmlElement("CurrentCredit")]
        public double CurrentCredit { get; set; }
    }
}
