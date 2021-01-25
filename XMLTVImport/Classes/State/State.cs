using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace XMLTVImport.Classes.State {

	[XmlRoot(ElementName = "State")]
	public class State {
		[XmlElement(ElementName = "KeywordGroups")]
		public KeywordGroups KeywordGroups { get; set; }
		[XmlElement(ElementName = "SeriesInfos")]
		public SeriesInfos SeriesInfos { get; set; }
		[XmlElement(ElementName = "Seasons")]
		public Seasons Seasons { get; set; }
		[XmlElement(ElementName = "Programs")]
		public Programs Programs { get; set; }
		[XmlElement(ElementName = "Services")]
		public Services Services { get; set; }
	}

	[XmlRoot(ElementName = "KeywordGroup")]
	public class KeywordGroup {
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "KeywordGroups")]
	public class KeywordGroups {
		[XmlElement(ElementName = "KeywordGroup")]
		public List<KeywordGroup> KeywordGroup = new List<KeywordGroup>();

		public void Add(Classes.MXF.KeywordGroup keywordGroup) {
			// Add the KeywordGroup
			KeywordGroup.Add(new KeywordGroup { Uid = keywordGroup.Uid,	Name = keywordGroup.Name });
        }

		public int GetNextAvailableUid() {
			if (KeywordGroup.Count == 0) {
				return 1;
            } else {
				int lastUid = KeywordGroup.Select(xx => int.Parse(xx.Uid.Replace("!KeywordGroup!k", ""))).OrderBy(xx => xx).Last();
				return lastUid + 1;
			}
		}
	}

	[XmlRoot(ElementName = "SeriesInfo")]
	public class SeriesInfo {
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "shortName")]
		public string ShortName { get; set; }
	}

	[XmlRoot(ElementName = "SeriesInfos")]
	public class SeriesInfos {
		[XmlElement(ElementName = "SeriesInfo")]
		public List<SeriesInfo> SeriesInfo = new List<SeriesInfo>();

		public void AddNew(Classes.MXF.SeriesInfo seriesInfo) {
			// Create new State SeriesInfo
			SeriesInfo.Add(new SeriesInfo { Uid = seriesInfo.Uid, Name = seriesInfo.Title, ShortName = seriesInfo.ShortTitle });
		}

		public int GetNextAvailableUid() {
			if (SeriesInfo.Count == 0) {
				return 1;
			} else {
				int lastUid = SeriesInfo.Select(xx => int.Parse(xx.Uid.Replace("!Series!", ""))).OrderBy(xx => xx).Last();
				return lastUid + 1;
			}
		}
	}

	[XmlRoot(ElementName = "Season")]
	public class Season {
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
		public List<Season> Season = new List<Season>();

		public void Add(Classes.MXF.Season season) {
			// Create new State Season
			Season.Add(new Season { Uid = season.Uid, Series = season.SeriesUid, Title = season.Title, Year = season.Year });
		}

		public int GetNextAvailableUid() {
			if (Season.Count == 0) {
				return 1;
			} else {
				int lastUid = Season.Select(xx => int.Parse(xx.Uid.Replace("!Season!", "")) ).OrderBy(xx => xx).Last();
				return lastUid + 1;
			}
        }
	}

	[XmlRoot(ElementName = "Program")]
	public class Program {
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "series")]
		public string Series { get; set; }
		[XmlAttribute(AttributeName = "season")]
		public string Season { get; set; }
		[XmlAttribute(AttributeName = "title")]
		public string Title { get; set; }
		[XmlAttribute(AttributeName = "year")]
		public string Year { get; set; }
		[XmlAttribute(AttributeName = "seasonNo")]
		public int SeasonNo { get; set; }
		[XmlAttribute(AttributeName = "episodeNo")]
		public int EpisodeNo { get; set; }
	}

	[XmlRoot(ElementName = "Programs")]
	public class Programs {
		[XmlElement(ElementName = "Program")]
		public List<Program> Program = new List<Program>();

		public void Add(Classes.MXF.Program program) {
			// Create new State Program
			Program.Add(new Program { Uid = program.Uid, Series = program.SeriesUid, Season = program.SeasonUid, Title = program.Title, Year = program.Year, SeasonNo = program.SeasonNumber, EpisodeNo = program.EpisodeNumber });
		}

		public int GetNextAvailableUid() {
			if (Program.Count == 0) {
				return 1;
            } else {
				int lastUid = Program.Select(xx => int.Parse(xx.Uid.Replace("!Program!", ""))).OrderBy(xx => xx).Last();
				return lastUid + 1;
			}
		}
	}

	[XmlRoot(ElementName = "Service")]
	public class Service {
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "callSign")]
		public string CallSign { get; set; }
	}

	[XmlRoot(ElementName = "Services")]
	public class Services {
		[XmlElement(ElementName = "Service")]
		public List<Service> Service = new List<Service>();

		public void Add(Classes.MXF.Service service) {
			// Create new State Service
			Service.Add(new Service { Uid = service.Uid, Name = service.Name, CallSign = service.CallSign });
		}

		public int GetNextAvailableUid() {
			if (Service.Count == 0) {
				return 1;
			} else {
				int lastUid = Service.Select(xx => int.Parse(xx.Uid.Replace("!Service!", ""))).OrderBy(xx => xx).Last();
				return lastUid + 1;
			}
		}
	}
}
