using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using XMLTVImport.Classes.XMLTV;

namespace XMLTVImport {
    class ImportOld {

        //private static string url;
        //private static string xml;
        //private static XmlSerializer serializer;
        //private static TV result;

        //static void Main(string[] args) {

        //    #region Get WMC ObjectStore #######################################

        //    string s = "Unable upgrade recording state.";
        //    byte[] bytes = Convert.FromBase64String("FAAODBUITwADRicSARc=");

        //    byte[] buffer2 = Encoding.ASCII.GetBytes(s);
        //    for (int i = 0; i != bytes.Length; i++) {
        //        bytes[i] = (byte)(bytes[i] ^ buffer2[i]);
        //    }

        //    string clientId = Microsoft.MediaCenter.Store.ObjectStore.GetClientId(true);
        //    SHA256Managed managed = new SHA256Managed();
        //    byte[] buffer = Encoding.Unicode.GetBytes(clientId);
        //    clientId = Convert.ToBase64String(managed.ComputeHash(buffer));

        //    // Get WMC ObjectStore
        //    ObjectStore wmcStore = ObjectStore.Open("", Encoding.ASCII.GetString(bytes), clientId, true);

        //    #endregion ########################################################


        //    #region Get WMC Guide Objects #####################################

        //    KeywordGroups keywordGroups         = new KeywordGroups(wmcStore);
        //    Keywords keywords                   = new Keywords(wmcStore);
        //    GuideImages guideImages             = new GuideImages(wmcStore);
        //    People people                       = new People(wmcStore);
        //    SeriesInfos seriesInfos             = new SeriesInfos(wmcStore);
        //    Seasons seasons                     = new Seasons(wmcStore);
        //    Affiliates affiliates               = new Affiliates(wmcStore);
        //    Programs programs                   = new Programs(wmcStore);
        //    Services services                   = new Services(wmcStore);
        //    ScheduleEntries scheduleEntries     = new ScheduleEntries(wmcStore);
        //    Lineups lineups                     = new Lineups(wmcStore);
        //    Channels channels                     = new Channels(wmcStore);

        //    #endregion ########################################################


        //    #region Get XMLTV Data ############################################

        //    // Set XMLTV XML Location
        //    url = "http://xmltv.net/xml_files/Adelaide.xml";
        //    // Initialise XML Serializer
        //    serializer = new XmlSerializer(typeof(TV));
            
        //    // Use WebClient to Download XMLTV XML
        //    using (WebClient client = new WebClient()) {
        //        client.Encoding = Encoding.UTF8;
        //        xml = client.DownloadString(url);
        //    }

        //    // Use TextReader to Create Objects from XML
        //    using (TextReader reader = new StringReader(xml)) {
        //        result = (TV)serializer.Deserialize(reader);
        //    }

        //    #endregion ########################################################


        //    #region Create WMC KeywordGroups ##################################

        //    // Create Movies Top-Level Keyword
        //    Keyword moviesKeyword = new Keyword("Movies");
        //    // Create TV Shows Top-Level Keyword
        //    Keyword tvShowsKeyword = new Keyword("TV Shows");

        //    // Create Movies KeywordGroup
        //    KeywordGroup moviesKeywordGroup = new KeywordGroup(moviesKeyword);
        //    // Create TV Shows KeywordGroup
        //    KeywordGroup tvShowsKeywordGroup = new KeywordGroup(tvShowsKeyword);

        //    #endregion ########################################################


        //    #region Create WMC Lineup #########################################

        //    //// Find Lineup from WMC ObjectStore
        //    //Lineup lineup = lineups.FirstOrDefault(xx => xx.);
        //    //// Create new Lineup if None Exists
        //    //if (lineup == null) {
        //    //    // Create Lineup
        //    //    lineup = new Lineup {
        //    //        Name = "Adelaide, Australia"
        //    //    };
        //    //    // Add Lineup to List
        //    //    lineups.Add(lineup);
        //    //}

        //    #endregion ########################################################


        //    #region Iterate XMLTV Channels ####################################

        //    // Iterate through XMLTV Channels
        //    foreach (XMLTVChannel xmltvChannel in result.Channel) {

        //        #region Create WMC GuideImages ################################

        //        // Find Guide Image from WMC ObjectStore
        //        GuideImage guideImage = guideImages.FirstOrDefault(xx => xx.ImageUrl == xmltvChannel.Icon.Src);
        //        // Create new Guide Image if None Exists
        //        if (guideImage == null) {
        //            guideImage = new GuideImage {
        //                ImageUrl = xmltvChannel.Icon.Src
        //            };
        //        }

        //        #endregion ####################################################


        //        #region Create WMC Service ################################

        //        // Find Service from WMC ObjectStore
        //        Service service = services.FirstOrDefault(xx => xx.Name == xmltvChannel.Displayname);
        //        // Create new Service if None Exists
        //        if (service == null) {
        //            service = new Service {
        //                Name = xmltvChannel.Displayname
        //            };
        //        }

        //        #endregion ####################################################


        //        #region Create WMC Channel ####################################

        //        // Find Channel from WMC ObjectStore
        //        Channel channel = channels.FirstOrDefault(xx => xx.ChannelNumber.Number == int.Parse(xmltvChannel.Lcn));
        //        // Create new Channel if None Exists
        //        if (channel == null) {
        //            channel = new Channel {
                        
        //            };
        //        }

        //        #endregion ####################################################

        //    }

        //    #endregion ########################################################


        //    #region Iterate XMLTV Programmes ##################################

        //    // Iterate through XMLTV Programmes
        //    foreach (Programme programme in result.Programme) {

        //        #region Get Programme Details #################################

        //        DateTime startTime = DateTime.ParseExact(programme.Start, "yyyyMMddHHmmss +zzz", new CultureInfo("en-AU"));
        //        DateTime endTime = DateTime.ParseExact(programme.Stop, "yyyyMMddHHmmss +zzz", new CultureInfo("en-AU"));

        //        bool isMovie = programme.Episodenum.FirstOrDefault(xx => xx.System == "xmltv_ns") == null;
        //        bool isSeries = programme.Episodenum.FirstOrDefault(xx => xx.System == "xmltv_ns") != null;
        //        bool isHdtv = programme.Video.Quality == "HDTV";
        //        bool isRepeat = programme.Previouslyshown != null;
        //        bool isPremiere = programme.Premiere != null;
        //        int seasonNo;
        //        int seasonCount;
        //        int episodeNo;
        //        int episodeCount;
        //        int partNo;
        //        int partCount;

        //        if (isSeries) {
        //            string[] data = programme.Episodenum.FirstOrDefault(xx => xx.System == "xmltv_ns").Text.Split('.');
        //            string[] seasonData = data[0].Split('/');
        //            string[] episodeData = data[1].Split('/');
        //            string[] partData = data[2].Split('/');

        //            // If Season Data Exists
        //            if (seasonData[0] != "") {
        //                seasonNo = int.Parse(seasonData[0]) + 1;
        //            }
        //            // If Season Count Data Exists
        //            if (seasonData[1] != "") {
        //                seasonCount = int.Parse(seasonData[1]) + 1;
        //            }

        //            // If Episode Data Exists
        //            if (episodeData[0] != "") {
        //                episodeNo = int.Parse(episodeData[0]) + 1;
        //            }
        //            // If Episode Count Data Exists
        //            if (episodeData[1] != "") {
        //                episodeCount = int.Parse(episodeData[1]) + 1;
        //            }

        //            // If Part Data Exists
        //            if (partData[0] != "") {
        //                partNo = int.Parse(partData[0]) + 1;
        //            }
        //            // If Part Count Data Exists
        //            if (partData[1] != "") {
        //                partCount = int.Parse(partData[1]) + 1;
        //            }
        //        }

        //        #endregion ####################################################


        //        #region Create WMC Keyword ####################################

        //        // Find Keyword from WMC ObjectStore
        //        Keyword keyword = keywords.FirstOrDefault(xx => xx.Word == programme.Category);
        //        // Create new Keyword if None Exists
        //        if (keyword == null) {
        //            // If the Programme is a Movie
        //            if (isMovie) {
        //                // Create Keyword
        //                keyword = new Keyword(programme.Category);
        //                // Add Keyword to List
        //                keywords.Add(keyword);
        //                // Add Keyword to KeywordGroup
        //                moviesKeywordGroup.Keywords.Add(keyword);
        //            }
        //            else {
        //                // Create Keyword
        //                keyword = new Keyword(programme.Category);
        //                // Add Keyword to List
        //                keywords.Add(keyword);
        //                // Add Keyword to KeywordGroup
        //                tvShowsKeywordGroup.Keywords.Add(keyword);
        //            }
        //        }

        //        #endregion ####################################################


        //        #region Create WMC People #####################################

        //        // Implement tv / movie database integration to enable this section

        //        #endregion ####################################################


        //        #region Create WMC SeriesInfo #################################

        //        // Find SeriesInfo from WMC ObjectStore
        //        SeriesInfo seriesInfo = seriesInfos.FirstOrDefault(xx => xx.Title == programme.Title);
        //        // Create new SeriesInfo if None Exists
        //        if (seriesInfo == null) {
        //            // Create SeriesInfo
        //            seriesInfo = new SeriesInfo {
        //                Title = programme.Title,
        //                ShortTitle = programme.Title
        //            };
        //            // Add SeriesInfo to List
        //            seriesInfos.Add(seriesInfo);
        //        }

        //        #endregion ####################################################


        //        #region Create WMC Season #####################################

        //        // If the Programme is a TV Show
        //        if (isSeries) {
        //            // Find Season from WMC ObjectStore
        //            Season season = seasons.FirstOrDefault(xx => xx.Title == programme.Title);
        //            // Create new Season if None Exists
        //            if (season == null) {
        //                // Create Season
        //                season = new Season {
        //                    Series = seriesInfo
        //                };
        //                // Add Season to List
        //                seasons.Add(season);
        //            }
        //        }

        //        #endregion ####################################################


        //        #region Create WMC Program ####################################

        //        // Find Program from WMC ObjectStore
        //        Program program = programs.FirstOrDefault(xx => xx.Title == programme.Title);
        //        // Create new Program if None Exists
        //        if (program == null) {
        //            // Create Program
        //            program = new Program {
        //                Title = programme.Title,
        //                Description = programme.Desc,
        //                EpisodeTitle = programme.Subtitle,
        //                IsMovie = isMovie,
        //                IsSeries = isSeries
        //            };
        //            // Add Program to List
        //            programs.Add(program);
        //        }

        //        #endregion ####################################################


        //        #region Create WMC ScheduleEntry ####################################

        //        // Find ScheduleEntry from WMC ObjectStore
        //        ScheduleEntry scheduleEntry = scheduleEntries.FirstOrDefault(xx => xx.Program == program);
        //        // Create new ScheduleEntry if None Exists
        //        if (scheduleEntry == null) {
        //            // Create ScheduleEntry
        //            scheduleEntry = new ScheduleEntry {
        //                Program = program,
        //                IsHdtv = isHdtv,
        //                IsRepeatFlag = isRepeat,
        //                IsPremiere = isPremiere,
        //                StartTime = startTime,
        //                EndTime = endTime,
                        
        //            };
        //            // Add ScheduleEntry to List
        //            scheduleEntries.Add(scheduleEntry);
        //        }

        //        #endregion ####################################################

                

        //    }

        //    #endregion ########################################################





        //    #region Create WMC  ##################################

        //    Lineup xmlChannelLineup = new Lineup();

        //    System.Diagnostics.Debug.WriteLine("GotHere");


        //    #endregion ########################################################


        //}
    }
}
