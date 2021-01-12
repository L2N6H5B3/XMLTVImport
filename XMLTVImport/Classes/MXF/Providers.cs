using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Provider")]
	public class Provider {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "displayName")]
		public string DisplayName { get; set; }
		[XmlAttribute(AttributeName = "copyright")]
		public string Copyright { get; set; }
	}

	[XmlRoot(ElementName = "Providers")]
	public class Providers {
		[XmlElement(ElementName = "Provider")]
		public List<Provider> Provider { get; set; }

		public Providers() {
			Provider = new List<Provider>();
		}

		public void Add(Provider provider) {
			// Set Provider Current Count
			int currentCount = Provider.Count + 1;
			// Set Provider ID
			provider.Id = $"provider{currentCount}";
			// Add Provider to List
			Provider.Add(provider);
		}
	}
}