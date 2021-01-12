using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.XMLTV {

	[XmlRoot(ElementName = "tv")]
	public class TV {
		[XmlElement(ElementName = "channel")]
		public List<XMLTVChannel> Channel { get; set; }
		[XmlElement(ElementName = "programme")]
		public List<Programme> Programme { get; set; }
		[XmlAttribute(AttributeName = "source-info-name")]
		public string Sourceinfoname { get; set; }
		[XmlAttribute(AttributeName = "generator-info-url")]
		public string Generatorinfourl { get; set; }
	}
}