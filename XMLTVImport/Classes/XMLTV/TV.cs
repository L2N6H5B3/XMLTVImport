using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XMLTVImport.Classes.XMLTV {

    [XmlRoot(ElementName = "tv")]
    public class TV {
        [XmlElement(ElementName = "channel")]
        public List<Channel> Channel { get; set; }
        [XmlElement(ElementName = "programme")]
        public List<Programme> Programme { get; set; }
        [XmlAttribute(AttributeName = "source-info-name")]
        public string Sourceinfoname { get; set; }
        [XmlAttribute(AttributeName = "generator-info-url")]
        public string Generatorinfourl { get; set; }
    }

    [XmlRoot(ElementName = "icon")]
    public class Icon {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel {
        [XmlElement(ElementName = "display-name")]
        public string Displayname { get; set; }
        [XmlElement(ElementName = "lcn")]
        public string Lcn { get; set; }
        [XmlElement(ElementName = "icon")]
        public Icon Icon { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
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

    [XmlRoot(ElementName = "programme")]
    public class Programme {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "sub-title")]
        public string Subtitle { get; set; }
        [XmlElement(ElementName = "desc")]
        public string Desc { get; set; }
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
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
}