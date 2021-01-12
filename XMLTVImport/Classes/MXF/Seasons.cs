using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Season")]
	public class Season {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "series")]
		public string Series { get; set; }
		[XmlAttribute(AttributeName = "title")]
		public string Title { get; set; }
		[XmlAttribute(AttributeName = "year")]
		public string Year { get; set; }
	}

	[XmlRoot(ElementName = "Seasons")]
	public class Seasons {
		[XmlElement(ElementName = "Season")]
		public List<Season> Season { get; set; }

		public Seasons() {
			Season = new List<Season>();
		}

		public void Add(Season season) {
			// Set Season Current Count
			int currentCount = Season.Count + 1;
			// Set Season UID
			season.Uid = $"!Season!{currentCount}";
			// Set Season ID
			season.Id = $"sn{currentCount}";
			// Add Season to List
			Season.Add(season);
		}
	}
}