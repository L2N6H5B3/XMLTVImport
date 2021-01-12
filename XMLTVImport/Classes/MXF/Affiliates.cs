using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Affiliate")]
	public class Affiliate {
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "logoImage")]
		public string LogoImage { get; set; }
	}

	[XmlRoot(ElementName = "Affiliates")]
	public class Affiliates {
		[XmlElement(ElementName = "Affiliate")]
		public List<Affiliate> Affiliate { get; set; }

		public Affiliates() {
			Affiliate = new List<Affiliate>();
		}
	}
}