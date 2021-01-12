using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "Keyword")]
	public class Keyword {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "word")]
		public string Word { get; set; }
	}

	[XmlRoot(ElementName = "Keywords")]
	public class Keywords {
		[XmlElement(ElementName = "Keyword")]
		public List<Keyword> Keyword { get; set; }

		public Keywords() {
			Keyword = new List<Keyword>();
		}

		public void Add(Keyword keyword) {
			// Set Keyword Current Count
			int currentCount = Keyword.Count + 1;
			// Set Keyword ID
			keyword.Id = $"k{currentCount}";
			// Add Keyword to List
			Keyword.Add(keyword);
		}

		public void Add(Keyword keyword, KeywordGroup keywordGroup) {
			// Set Keyword Current Count
			int currentCount = keywordGroup.counter;
			// Set Keyword ID
			keyword.Id = $"k{currentCount}";
			// Add Keyword to List
			Keyword.Add(keyword);
		}

		public bool CheckKeyword(string keyword) {
			// Check Keyword status
			if (Keyword.FirstOrDefault(xx => xx.Word == keyword) == null) {
				return false;
            } else {
				return true;
			}
		}
	}
}