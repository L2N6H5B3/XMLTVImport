using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "With")]
	public class With {
		[XmlElement(ElementName = "Keywords")]
		public Keywords Keywords { get; set; }
		[XmlElement(ElementName = "KeywordGroups")]
		public KeywordGroups KeywordGroups { get; set; }
		[XmlElement(ElementName = "GuideImages")]
		public GuideImages GuideImages { get; set; }
		[XmlElement(ElementName = "People")]
		public People People { get; set; }
		[XmlElement(ElementName = "SeriesInfos")]
		public SeriesInfos SeriesInfos { get; set; }
		[XmlElement(ElementName = "Seasons")]
		public Seasons Seasons { get; set; }
		[XmlElement(ElementName = "Programs")]
		public Programs Programs { get; set; }
		[XmlElement(ElementName = "Affiliates")]
		public Affiliates Affiliates { get; set; }
		[XmlElement(ElementName = "Services")]
		public Services Services { get; set; }
		[XmlElement(ElementName = "ScheduleEntries")]
		public List<ScheduleEntries> ScheduleEntries { get; set; }
		[XmlElement(ElementName = "Lineups")]
		public Lineups Lineups { get; set; }
		[XmlAttribute(AttributeName = "provider")]
		public string Provider { get; set; }
	}
}