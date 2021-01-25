using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Service")]
	public class Service {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "callSign")]
		public string CallSign { get; set; }
		[XmlAttribute(AttributeName = "affiliate")]
		public string Affiliate { get; set; }
		[XmlAttribute(AttributeName = "logoImage")]
		public string LogoImage { get; set; }
	}

	[XmlRoot(ElementName = "Services")]
	public class Services {
		[XmlElement(ElementName = "Service")]
		public List<Service> Service { get; set; }

		public Services() {
			Service = new List<Service>();
		}

		public void AddNew(Service service, int uid) {
			// Set Service UID
			service.Uid = $"!Service!{uid}";
			// Set Service Current Count
			int currentCount = Service.Count + 1;
			// Set Service ID
			service.Id = $"s{currentCount}";
			// Add Service to List
			Service.Add(service);
		}

		public void AddExisting(Service service) {
			// Set Service Current Count
			int currentCount = Service.Count + 1;
			// Set Service ID
			service.Id = $"s{currentCount}";
			// Add Service to List
			Service.Add(service);
		}
	}
}