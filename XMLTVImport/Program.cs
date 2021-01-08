using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using XMLTVImport.Classes.XMLTV;

namespace XMLTVImport {
    class Program {
        static void Main(string[] args) {


            String url = "http://xmltv.net/xml_files/Adelaide.xml";
            String xml;

            using (WebClient client = new WebClient()) {
                client.Encoding = Encoding.UTF8;
                xml = client.DownloadString(url);
            }

            var serializer = new XmlSerializer(typeof(TV));
            TV result;

            using (TextReader reader = new StringReader(xml)) {
                result = (TV)serializer.Deserialize(reader);
                System.Diagnostics.Debug.WriteLine("End...");
            }
        }
    }
}
