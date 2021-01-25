using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.MXF {

	[XmlRoot(ElementName = "GuideImage")]
	public class GuideImage {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "imageUrl")]
		public string ImageUrl { get; set; }
		[XmlIgnore]
		public string ChannelNo { get; set; }
	}

	[XmlRoot(ElementName = "GuideImages")]
	public class GuideImages {
		[XmlElement(ElementName = "GuideImage")]
		public List<GuideImage> GuideImage { get; set; }

		public GuideImages() {
			GuideImage = new List<GuideImage>();
        }

		public void Add(GuideImage guideImage) {
			// Set GuideImage Current Count
			int currentCount = GuideImage.Count + 1;
			// Set GuideImage ID
			guideImage.Id = $"i{currentCount}";
			// Add GuideImage to List
			GuideImage.Add(guideImage);
		}
	}
}