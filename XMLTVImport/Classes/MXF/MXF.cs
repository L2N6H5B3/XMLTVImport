using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "MXF")]
	public class MXF {
		[XmlElement(ElementName = "Assembly")]
		public List<Assembly> Assembly { get; set; }
		[XmlElement(ElementName = "Providers")]
		public Providers Providers { get; set; }
		[XmlElement(ElementName = "With")]
		public With With { get; set; }
	}

	[XmlRoot(ElementName = "Type")]
	public class Type {
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "parentFieldName")]
		public string ParentFieldName { get; set; }
		[XmlAttribute(AttributeName = "groupName")]
		public string GroupName { get; set; }
	}

	[XmlRoot(ElementName = "NameSpace")]
	public class NameSpace {
		[XmlElement(ElementName = "Type")]
		public List<Type> Type { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "Assembly")]
	public class Assembly {
		[XmlElement(ElementName = "NameSpace")]
		public NameSpace NameSpace { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "cultureInfo")]
		public string CultureInfo { get; set; }
		[XmlAttribute(AttributeName = "publicKey")]
		public string PublicKey { get; set; }
	}
}