using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data
{
    public class DropData
    {
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public string type;
        [XmlAttribute]
        public int exp;
    }
}
