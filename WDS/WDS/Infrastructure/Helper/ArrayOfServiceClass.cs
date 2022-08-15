using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WDS.Infrastructure.Helper
{
    [XmlRoot("ArrayOfServiceClass")]
    public class ArrayOfServiceClass
    {
        [XmlElement("ServiceClass")]
        public List<ServiceClass> ServiceClasses { get; set; }
    }
}
