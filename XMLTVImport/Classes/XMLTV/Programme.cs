using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.XMLTV {

	[XmlRoot(ElementName = "programme")]
	public class Programme {
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "sub-title")]
		public string Subtitle { get; set; }
		[XmlElement(ElementName = "desc")]
		public string Desc { get; set; }
		[XmlElement(ElementName = "category")]
		public List<string> Category { get; set; }
		[XmlElement(ElementName = "icon")]
		public ProgrammeIcon Icon { get; set; }
		[XmlElement(ElementName = "episode-num")]
		public List<Episodenum> Episodenum { get; set; }
		[XmlElement(ElementName = "previously-shown")]
		public string Previouslyshown { get; set; }
		[XmlElement(ElementName = "video")]
		public Video Video { get; set; }
		[XmlElement(ElementName = "rating")]
		public Rating Rating { get; set; }
		[XmlElement(ElementName = "premiere")]
		public string Premiere { get; set; }
		[XmlAttribute(AttributeName = "start")]
		public string Start { get; set; }
		[XmlAttribute(AttributeName = "stop")]
		public string Stop { get; set; }
		[XmlAttribute(AttributeName = "channel")]
		public string Channel { get; set; }
	}

	[XmlRoot(ElementName = "episode-num")]
	public class Episodenum {
		[XmlAttribute(AttributeName = "system")]
		public string System { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "video")]
	public class Video {
		[XmlElement(ElementName = "quality")]
		public string Quality { get; set; }
	}

	[XmlRoot(ElementName = "rating")]
	public class Rating {
		[XmlElement(ElementName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "icon")]
	public class ProgrammeIcon {
		[XmlAttribute(AttributeName = "src")]
		public string Src { get; set; }
	}
}