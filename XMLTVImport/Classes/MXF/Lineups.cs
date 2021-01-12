using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Lineup")]
	public class Lineup {
		[XmlElement(ElementName = "channels")]
		public Channels Channels { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlIgnore]
		public string ShortName { get; set; }
		[XmlAttribute(AttributeName = "primaryProvider")]
		public string PrimaryProvider { get; set; }
	}

	[XmlRoot(ElementName = "Lineups")]
	public class Lineups {
		[XmlElement(ElementName = "Lineup")]
		public List<Lineup> Lineup { get; set; }

		public Lineups() {
			Lineup = new List<Lineup>();
		}

		public void Add(Lineup lineup) {
			// Set Lineup Current Count
			int currentCount = Lineup.Count + 1;
			// Set Lineup UID
			lineup.Uid = $"!Lineup!{lineup.ShortName}";
			// Set Lineup ID
			lineup.Id = $"l{currentCount}";
			// Add Lineup to List
			Lineup.Add(lineup);
		}
	}
}