using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Person")]
	public class Person {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
	}

	[XmlRoot(ElementName = "People")]
	public class People {
		[XmlElement(ElementName = "Person")]
		public List<Person> Person { get; set; }

		public People() {
			Person = new List<Person>();
		}
	}
}