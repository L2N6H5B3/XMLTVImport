using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Channel")]
	public class Channel {
		[XmlIgnore]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "lineup")]
		public string Lineup { get; set; }
		[XmlAttribute(AttributeName = "service")]
		public string Service { get; set; }
		[XmlAttribute(AttributeName = "number")]
		public int Number { get; set; }
        [XmlAttribute(AttributeName = "matchName")]
        public string MatchName { get; set; }
    }

	[XmlRoot(ElementName = "channels")]
	public class Channels {
		[XmlElement(ElementName = "Channel")]
		public List<Channel> Channel { get; set; }

		public Channels() {
			Channel = new List<Channel>();
		}

		public void Add(Channel channel, Lineup lineup) {
			// Set Channel Current Count
			int currentCount = Channel.Count + 1;
			// Set Channel UID
			channel.Uid = $"!Channel!{lineup.ShortName}!{channel.Number}";
			// Add Channel to List
			Channel.Add(channel);
		}
	}
}