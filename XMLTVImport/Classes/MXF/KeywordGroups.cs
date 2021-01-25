using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "KeywordGroup")]
	public class KeywordGroup {
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; }
		[XmlAttribute(AttributeName = "groupName")]
		public string GroupName { get; set; }
		[XmlIgnore]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "keywords")]
		public string Keywords = "";
		[XmlIgnore]
		public List<Keyword> KeywordList = new List<Keyword>();
		[XmlIgnore]
		public int counter = 0;

		public void Add(Keyword keyword) {
			if (Keywords.Length == 0) {
				// Add KeywordGroup to String
				Keywords += keyword.Id;
			} else {
				// Add KeywordGroup to String
				Keywords += $",{keyword.Id}";
			}
			// Increment Counter
			counter++;
			// Add Keyword to List
			KeywordList.Add(keyword);
		}
	}

	[XmlRoot(ElementName = "KeywordGroups")]
	public class KeywordGroups {
		[XmlElement(ElementName = "KeywordGroup")]
		public List<KeywordGroup> KeywordGroup { get; set; }
		[XmlIgnore]
		public int counter = 1;

		public KeywordGroups() {
			KeywordGroup = new List<KeywordGroup>();
		}

		public void AddNew(KeywordGroup keywordGroup, int uid) {
			// Set KeywordGroup UID
			keywordGroup.Uid = $"!KeywordGroup!k{uid}";
			// Set KeywordGroup Current Count
			counter++;
			// Set KeywordGroup Name
			keywordGroup.GroupName = $"k{counter}";
			// Set KeywordGroup Counter
			keywordGroup.counter = counter * 100;
			// Add KeywordGroup to List
			KeywordGroup.Add(keywordGroup);
		}

		public void AddExisting(KeywordGroup keywordGroup) {
			// Set KeywordGroup Current Count
			counter++;
			// Set KeywordGroup Name
			keywordGroup.GroupName = $"k{counter}";
			// Set KeywordGroup Counter
			keywordGroup.counter = counter * 100;
			// Add KeywordGroup to List
			KeywordGroup.Add(keywordGroup);
		}
	}
}