using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Program")]
	public class Program {
		[XmlElement(ElementName = "ActorRole")]
		public List<ActorRole> ActorRole { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "title")]
		public string Title { get; set; }
		[XmlAttribute(AttributeName = "description")]
		public string Description { get; set; }
		[XmlAttribute(AttributeName = "shortDescription")]
		public string ShortDescription { get; set; }
		[XmlAttribute(AttributeName = "episodeTitle")]
		public string EpisodeTitle { get; set; }
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }
		[XmlAttribute(AttributeName = "year")]
		public string Year { get; set; }
		[XmlAttribute(AttributeName = "seasonNumber")]
		public int SeasonNumber { get; set; }
		[XmlAttribute(AttributeName = "episodeNumber")]
		public int EpisodeNumber { get; set; }
		[XmlAttribute(AttributeName = "originalAirdate")]
		public string OriginalAirdate { get; set; }
		[XmlAttribute(AttributeName = "keywords")]
		public string Keywords { get; set; }
		[XmlAttribute(AttributeName = "series")]
		public string Series { get; set; }
		[XmlAttribute(AttributeName = "isSeries")]
		public bool IsSeries { get; set; }
		[XmlAttribute(AttributeName = "isMovie")]
		public bool IsMovie { get; set; }
		[XmlAttribute(AttributeName = "isShort")]
		public bool IsShort { get; set; }
		[XmlAttribute(AttributeName = "isMiniseries")]
		public bool IsMiniseries { get; set; }
		[XmlAttribute(AttributeName = "isKids")]
		public bool IsKids { get; set; }
		[XmlAttribute(AttributeName = "isSports")]
		public bool IsSports { get; set; }
		[XmlAttribute(AttributeName = "isSpecial")]
		public bool IsSpecial { get; set; }
		[XmlAttribute(AttributeName = "isNews")]
		public bool IsNews { get; set; }
		[XmlAttribute(AttributeName = "isReality")]
		public bool IsReality { get; set; }
		[XmlAttribute(AttributeName = "guideImage")]
		public string GuideImage { get; set; }
		[XmlElement(ElementName = "GuestActorRole")]
		public GuestActorRole GuestActorRole { get; set; }
		[XmlElement(ElementName = "DirectorRole")]
		public DirectorRole DirectorRole { get; set; }
		[XmlElement(ElementName = "ProducerRole")]
		public ProducerRole ProducerRole { get; set; }
		[XmlElement(ElementName = "WriterRole")]
		public List<WriterRole> WriterRole { get; set; }
        [XmlIgnore]
        public DateTime StartTime { get; set; }
        [XmlIgnore]
        public DateTime EndTime { get; set; }
        [XmlIgnore]
        public Channel Channel { get; set; }
    }

    [XmlRoot(ElementName = "ActorRole")]
	public class ActorRole {
		[XmlAttribute(AttributeName = "person")]
		public string Person { get; set; }
		[XmlAttribute(AttributeName = "rank")]
		public string Rank { get; set; }
	}

	[XmlRoot(ElementName = "GuestActorRole")]
	public class GuestActorRole {
		[XmlAttribute(AttributeName = "person")]
		public string Person { get; set; }
		[XmlAttribute(AttributeName = "rank")]
		public string Rank { get; set; }
	}

	[XmlRoot(ElementName = "DirectorRole")]
	public class DirectorRole {
		[XmlAttribute(AttributeName = "person")]
		public string Person { get; set; }
		[XmlAttribute(AttributeName = "rank")]
		public string Rank { get; set; }
	}

	[XmlRoot(ElementName = "ProducerRole")]
	public class ProducerRole {
		[XmlAttribute(AttributeName = "person")]
		public string Person { get; set; }
		[XmlAttribute(AttributeName = "rank")]
		public string Rank { get; set; }
	}

	[XmlRoot(ElementName = "WriterRole")]
	public class WriterRole {
		[XmlAttribute(AttributeName = "person")]
		public string Person { get; set; }
		[XmlAttribute(AttributeName = "rank")]
		public string Rank { get; set; }
	}

	[XmlRoot(ElementName = "Programs")]
	public class Programs {
		[XmlElement(ElementName = "Program")]
		public List<Program> Program { get; set; }

		public Programs() {
			Program = new List<Program>();
		}

		public void Add(Program program) {
			// Set Program Current Count
			int currentCount = Program.Count + 1;
			// Set Program UID
			program.Uid = $"!Program!{currentCount}";
			// Set Program ID
			program.Id = currentCount.ToString();
			// Add Program to List
			Program.Add(program);
		}
	}	
}