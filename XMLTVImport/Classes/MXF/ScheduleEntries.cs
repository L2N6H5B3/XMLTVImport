using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "ScheduleEntry")]
	public class ScheduleEntry {
		[XmlAttribute(AttributeName = "program")]
		public string Program { get; set; }
		[XmlAttribute(AttributeName = "startTime")]
		public string StartTime { get; set; }
		[XmlAttribute(AttributeName = "duration")]
		public int Duration { get; set; }
        [XmlIgnore]
        public DateTime StartDateTime { get; set; }
        [XmlIgnore]
        public DateTime EndDateTime { get; set; }
        [XmlAttribute(AttributeName = "isCC")]
		public bool IsCC { get; set; }
		[XmlAttribute(AttributeName = "isHdtv")]
		public bool IsHdtv { get; set; }
		[XmlAttribute(AttributeName = "isPremiere")]
		public bool IsPremiere { get; set; }
		[XmlAttribute(AttributeName = "isFinale")]
		public bool IsFinale { get; set; }
		[XmlAttribute(AttributeName = "isLive")]
		public bool IsLive { get; set; }
		[XmlAttribute(AttributeName = "isLiveSports")]
		public bool IsLiveSports { get; set; }
		[XmlAttribute(AttributeName = "isTape")]
		public bool IsTape { get; set; }
		[XmlAttribute(AttributeName = "isDelay")]
		public bool IsDelay { get; set; }
		[XmlAttribute(AttributeName = "isSubtitled")]
		public bool IsSubtitled { get; set; }
		[XmlAttribute(AttributeName = "audioFormat")]
		public int AudioFormat { get; set; }
		[XmlAttribute(AttributeName = "part")]
		public int Part { get; set; }
		[XmlAttribute(AttributeName = "parts")]
		public int Parts { get; set; }
	}

	[XmlRoot(ElementName = "ScheduleEntries")]
	public class ScheduleEntries {
		[XmlElement(ElementName = "ScheduleEntry")]
		public List<ScheduleEntry> ScheduleEntry { get; set; }
		[XmlAttribute(AttributeName = "service")]
		public string Service { get; set; }

		public ScheduleEntries() {
			ScheduleEntry = new List<ScheduleEntry>();
		}

		public void Add(ScheduleEntry scheduleEntry) {
			// Add ScheduleEntry to List
			ScheduleEntry.Add(scheduleEntry);
		}
	}
}