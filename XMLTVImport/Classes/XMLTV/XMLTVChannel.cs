using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.XMLTV {

	[XmlRoot(ElementName = "channel")]
	public class XMLTVChannel {
		[XmlElement(ElementName = "display-name")]
		public string Displayname { get; set; }
		[XmlElement(ElementName = "lcn")]
		public string Lcn { get; set; }
		[XmlElement(ElementName = "icon")]
		public Icon Icon { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "icon")]
	public class Icon {
		[XmlAttribute(AttributeName = "src")]
		public string Src { get; set; }
	}
}