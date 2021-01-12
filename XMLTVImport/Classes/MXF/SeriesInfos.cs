using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "SeriesInfo")]
	public class SeriesInfo {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "title")]
		public string Title { get; set; }
		[XmlAttribute(AttributeName = "shortTitle")]
		public string ShortTitle { get; set; }
		[XmlAttribute(AttributeName = "description")]
		public string Description { get; set; }
		[XmlAttribute(AttributeName = "shortDescription")]
		public string ShortDescription { get; set; }
		[XmlAttribute(AttributeName = "startAirdate")]
		public string StartAirdate { get; set; }
		[XmlAttribute(AttributeName = "endAirdate")]
		public string EndAirdate { get; set; }
		[XmlAttribute(AttributeName = "guideImage")]
		public string GuideImage { get; set; }
	}

	[XmlRoot(ElementName = "SeriesInfos")]
	public class SeriesInfos {
		[XmlElement(ElementName = "SeriesInfo")]
		public List<SeriesInfo> SeriesInfo { get; set; }

		public SeriesInfos() {
			SeriesInfo = new List<SeriesInfo>();
		}

		public void Add(SeriesInfo seriesInfo) {
			// Set SeriesInfo Current Count
			int currentCount = SeriesInfo.Count + 1;
			// Set SeriesInfo UID
			seriesInfo.Uid = $"!Series!{currentCount}";
			// Set SeriesInfo ID
			seriesInfo.Id = $"si{currentCount}";
			// Add SeriesInfo to List
			SeriesInfo.Add(seriesInfo);
		}
	}
}