using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using XMLTVImport.Classes.XMLTV;
using XMLTVImport.Classes.MXF;
using Microsoft.MediaCenter.Store;

namespace XMLTVImport {
    class Import {

        private static string url;
        private static string xml;
        private static XmlSerializer serializer;
        private static TV result;
        private static MXF baseMxf;

        static void Main(string[] args) {

            #region Get WMC ObjectStore #######################################

            string s = "Unable upgrade recording state.";
            byte[] bytes = Convert.FromBase64String("FAAODBUITwADRicSARc=");

            byte[] buffer2 = Encoding.ASCII.GetBytes(s);
            for (int i = 0; i != bytes.Length; i++) {
                bytes[i] = (byte)(bytes[i] ^ buffer2[i]);
            }

            string clientId = Microsoft.MediaCenter.Store.ObjectStore.GetClientId(true);
            SHA256Managed managed = new SHA256Managed();
            byte[] buffer = Encoding.Unicode.GetBytes(clientId);
            clientId = Convert.ToBase64String(managed.ComputeHash(buffer));

            // Get WMC ObjectStore
            ObjectStore wmcStore = ObjectStore.Open("", Encoding.ASCII.GetString(bytes), clientId, true);

            #endregion ########################################################


            #region Get WMC Guide Objects #####################################

            baseMxf                         = new MXF();
            baseMxf.Assembly                = new List<Assembly>();
            baseMxf.Providers               = new Classes.MXF.Providers();
            baseMxf.With                    = new With();
            baseMxf.With.KeywordGroups      = new KeywordGroups();
            baseMxf.With.Keywords           = new Keywords();
            baseMxf.With.GuideImages        = new GuideImages();
            baseMxf.With.People             = new People();
            baseMxf.With.SeriesInfos        = new SeriesInfos();
            baseMxf.With.Seasons            = new Seasons();
            baseMxf.With.Affiliates         = new Affiliates();
            baseMxf.With.Programs           = new Programs();
            baseMxf.With.Services           = new Services();
            baseMxf.With.ScheduleEntries    = new List<ScheduleEntries>();
            baseMxf.With.Lineups            = new Lineups();

            #endregion ########################################################


            #region Get XMLTV Data ############################################

            // Set XMLTV XML Location
            url = "http://xmltv.net/xml_files/Adelaide.xml";
            // Initialise XML Serializer
            serializer = new XmlSerializer(typeof(TV));

            // Use WebClient to Download XMLTV XML
            using (WebClient client = new WebClient()) {
                client.Encoding = Encoding.UTF8;
                xml = client.DownloadString(url);
            }

            // Use TextReader to Create Objects from XML
            using (TextReader reader = new StringReader(xml)) {
                result = (TV)serializer.Deserialize(reader);
            }

            #endregion ########################################################


            #region Create WMC KeywordGroups ##################################

            // Create All Top-Level Keyword
            Keyword allKeyword = new Keyword {
                Word = "All"
            };
            // Create Movies Top-Level Keyword
            Keyword moviesKeyword = new Keyword {
                Word = "Movies"
            };
            // Create TV Shows Top-Level Keyword
            Keyword tvShowsKeyword = new Keyword {
                Word = "TV Shows"
            };

            baseMxf.With.Keywords.Add(allKeyword);
            baseMxf.With.Keywords.Add(moviesKeyword);
            baseMxf.With.Keywords.Add(tvShowsKeyword);

            // Create Movies KeywordGroup
            KeywordGroup moviesKeywordGroup = new KeywordGroup {
                GroupName = moviesKeyword.Id
            };
            moviesKeywordGroup.Add(allKeyword);
            baseMxf.With.KeywordGroups.Add(moviesKeywordGroup);
            // Create TV Shows KeywordGroup
            KeywordGroup tvShowsKeywordGroup = new KeywordGroup {
                GroupName = tvShowsKeyword.Id
            };
            tvShowsKeywordGroup.Add(allKeyword);
            baseMxf.With.KeywordGroups.Add(tvShowsKeywordGroup);

            #endregion ########################################################


            #region Create MXF Base ###########################################

            #region Create Assembly ###########################################

            // Create mcepg Assembly for WMC BaseMXF
            Assembly mcepg = new Assembly {
                PublicKey = "0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9",
                Name = "mcepg",
                Version = "6.0.6000.0"
            };
            // Add Assembly to List
            baseMxf.Assembly.Add(mcepg);

            // Create mcstore Assembly for WMC BaseMXF
            Assembly mcstore = new Assembly {
                PublicKey = "0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9",
                Name = "mcstore",
                Version = "6.0.6000.0"
            };
            // Add Assembly to List
            baseMxf.Assembly.Add(mcstore);

            #endregion ########################################################


            #region Create NameSpaces #########################################

            // Create mcepg Namespace for WMC BaseMXF
            mcepg.NameSpace = new NameSpace {
                Name = "Microsoft.MediaCenter.Guide",
                Type = new List<Classes.MXF.Type>()
            };

            // Create mcstore Namespace for WMC BaseMXF
            mcstore.NameSpace = new NameSpace {
                Name = "Microsoft.MediaCenter.Store",
                Type = new List<Classes.MXF.Type>()
            };

            #endregion ########################################################


            #region Create Types ##############################################

            // Create Types for mcepg Assembly
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Lineup" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Channel", ParentFieldName = "lineup" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Service" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "ScheduleEntry", GroupName = "ScheduleEntries" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Keyword" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "KeywordGroup" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Person", GroupName = "People" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "ActorRole", GroupName = "program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "DirectorRole", GroupName = "program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "WriterRole", GroupName = "program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "HostRole", GroupName = "program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "GuestActorRole", GroupName = "program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "ProducerRole", GroupName = "program" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "GuideImage" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Affiliate" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "SeriesInfo" });
            mcepg.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Season" });

            // Create Types for mcstore Assembly
            mcstore.NameSpace.Type.Add(new Classes.MXF.Type { Name = "Provider" });
            mcstore.NameSpace.Type.Add(new Classes.MXF.Type { Name = "UId", ParentFieldName = "target" });

            #endregion ########################################################

            #endregion ########################################################


            #region Create WMC Provider #######################################

            // Create Provider for WMC BaseMXF
            Classes.MXF.Provider provider = new Classes.MXF.Provider {
                DisplayName = "XMLTV",
                Name = "XMLTV",
                Copyright = "Copyright 2021 - XMLTV, Luke Bradtke"
            };
            // Add Provider to List
            baseMxf.Providers.Add(provider);
            // Add Provider to With
            baseMxf.With.Provider = provider.Id;

            #endregion ########################################################


            #region Create WMC Lineup #########################################

            // Create Lineup for WMC BaseMXF
            Lineup lineup  = new Lineup {
                Name = "Adelaide, Australia",
                PrimaryProvider = "!MCLineup!MainLineup",
                ShortName = "Adelaide",
                Channels = new Channels()
            };
            // Add Lineup to List
            baseMxf.With.Lineups.Add(lineup);

            #endregion ########################################################


            #region Iterate XMLTV Channels ####################################

            // Iterate through XMLTV Channels
            foreach (XMLTVChannel xmltvChannel in result.Channel.OrderBy(xx => int.Parse(xx.Lcn))) {

                #region Create WMC GuideImages ################################

                // Find Guide Image from WMC BaseMXF
                GuideImage guideImage = baseMxf.With.GuideImages.GuideImage.FirstOrDefault(xx => xx.ImageUrl == xmltvChannel.Icon.Src);
                // Create new Guide Image if None Exists
                if (guideImage == null) {
                    guideImage = new GuideImage {
                        ImageUrl = xmltvChannel.Icon.Src
                    };
                    // Add GuideImage to List
                    baseMxf.With.GuideImages.Add(guideImage);
                }

                #endregion ####################################################


                #region Create WMC Service ################################

                // Find Service from WMC BaseMXF
                Service service = baseMxf.With.Services.Service.FirstOrDefault(xx => xx.Name == xmltvChannel.Displayname);
                // Create new Service if None Exists
                if (service == null) {
                    service = new Service {
                        Name = xmltvChannel.Displayname,
                        LogoImage = guideImage.Id,
                        CallSign = xmltvChannel.Lcn
                    };
                    // Add Service to List
                    baseMxf.With.Services.Add(service);
                }

                #endregion ####################################################


                #region Create WMC ScheduleEntries ############################

                // Find ScheduleEntries from WMC BaseMXF
                ScheduleEntries scheduleEntries = baseMxf.With.ScheduleEntries.FirstOrDefault(xx => xx.Service == service.Id);
                // Create new ScheduleEntry if None Exists
                if (scheduleEntries == null) {
                    // Create ScheduleEntry
                    scheduleEntries = new ScheduleEntries {
                       Service = service.Id
                    };
                    // Add ScheduleEntries to List
                    baseMxf.With.ScheduleEntries.Add(scheduleEntries);
                }

                #endregion ####################################################


                #region Create WMC Channel ####################################

                // Find Channel from WMC BaseMXF
                Channel channel = baseMxf.With.Lineups.Lineup.First().Channels.Channel.FirstOrDefault(xx => xx.Number == int.Parse(xmltvChannel.Lcn));
                // Create new Channel if None Exists
                if (channel == null) {
                    channel = new Channel {
                        Number = int.Parse(xmltvChannel.Lcn),
                        Service = service.Id,
                        Id = xmltvChannel.Id,
                        Lineup = lineup.Id,
                        MatchName = ConfigurationManager.AppSettings.Get(xmltvChannel.Lcn)
                    };
                    // Add Channel to List
                    baseMxf.With.Lineups.Lineup.First().Channels.Add(channel, lineup);
                }

                #endregion ####################################################

            }

            #endregion ########################################################


            #region Iterate XMLTV Programmes ##################################

            // Iterate through XMLTV Programmes
            foreach (Programme programme in result.Programme) {

                #region Get Programme Details #################################

                DateTime startTime = DateTime.ParseExact(programme.Start, "yyyyMMddHHmmss zzz", new CultureInfo("en-AU"));
                DateTime endTime = DateTime.ParseExact(programme.Stop, "yyyyMMddHHmmss zzz", new CultureInfo("en-AU"));
                DateTime originalAirDate = DateTime.ParseExact("2000-01-01 12:00", "yyyy-MM-dd HH:mm", new CultureInfo("en-AU"));
                // Get the Previously Aired Status
                bool isRepeat = programme.Previouslyshown != null;
                // Get the Previously Aired Date
                bool hasPreviousAiringDate = programme.Episodenum.FirstOrDefault(xx => xx.System == "original-air-date") != null;
                // If the Programme has Previous Airing Date
                if (hasPreviousAiringDate) {
                    // Set the Actual Date
                    originalAirDate = DateTime.ParseExact(programme.Episodenum.FirstOrDefault(xx => xx.System == "original-air-date").Text, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-AU"));
                }


                bool isMovie = programme.Episodenum.FirstOrDefault(xx => xx.System == "xmltv_ns") == null;
                bool isSeries = programme.Episodenum.FirstOrDefault(xx => xx.System == "xmltv_ns") != null;
                bool isShort = false;
                bool isMiniseries = false;
                bool isYearly = false;
                bool isPremiere = programme.Premiere != null;
                bool isFinale = false;
                bool isKids = false;
                bool isNews = false;
                bool isReality = false;
                bool isSpecial = false;
                bool isSports = false;
                bool isCC = false;
                bool isSubtitled = false;
                bool isDelay = false;
                bool isLive = false;
                bool isLiveSports = false;
                bool isTape = false;
                bool hasImage = false;
                string year = "0";
                int audioFormat = 0;
                int seasonNo = 0;
                int seasonCount = 0;
                int episodeNo = 0;
                int episodeCount = 0;
                int partNo = 0;
                int partCount = 0;
                bool isHdtv = false;
                if (programme.Video != null && programme.Video.Quality == "HDTV") {
                    isHdtv = true;
                }
                if (isSeries) {
                    string[] data = programme.Episodenum.FirstOrDefault(xx => xx.System == "xmltv_ns").Text.Split('.');
                    string[] seasonData = data[0].Split('/');
                    string[] episodeData = data[1].Split('/');
                    string[] partData = data[2].Split('/');

                    // If Season Data Exists
                    if (seasonData.Length > 0 && seasonData[0] != "") {
                        seasonNo = int.Parse(seasonData[0]) + 1;
                        if (seasonNo.ToString().Length == 4) {
                            year = seasonNo.ToString();
                            isYearly = true;
                        }
                    }
                    // If Season Count Data Exists
                    if (seasonData.Length > 1 && seasonData[1] != "") {
                        seasonCount = int.Parse(seasonData[1]) + 1;
                    }

                    // If Episode Data Exists
                    if (episodeData.Length > 0 && episodeData[0] != "") {
                        episodeNo = int.Parse(episodeData[0]) + 1;
                    }
                    // If Episode Count Data Exists
                    if (episodeData.Length > 1 && episodeData[1] != "") {
                        episodeCount = int.Parse(episodeData[1]) + 1;
                    }

                    // If Part Data Exists
                    if (partData.Length > 0 && partData[0] != "") {
                        partNo = int.Parse(partData[0]) + 1;
                    }
                    // If Part Count Data Exists
                    if (partData.Length > 1 && partData[1] != "") {
                        partCount = int.Parse(partData[1]) + 1;
                    }
                }

                #endregion ####################################################


                #region Create WMC Keyword ####################################

                // Create Keywords List
                List<Keyword> keywords = new List<Keyword>();

                // Iterate through each Keyword in Programme Category
                foreach (string word in programme.Category) {
                    // Find Keyword from WMC BaseMXF
                    Keyword keyword = baseMxf.With.Keywords.Keyword.FirstOrDefault(xx => xx.Word == word);
                    // Create new Keyword if None Exists
                    if (keyword == null) {
                        // If the Programme is a Movie
                        if (isMovie) {
                            // Create Keyword
                            keyword = new Keyword {
                                Word = word
                            };
                            // Add Keyword to List
                            baseMxf.With.Keywords.Add(keyword, moviesKeywordGroup);
                            // Add Keyword to KeywordGroup
                            moviesKeywordGroup.Add(keyword);
                        }
                        else {
                            // Create Keyword
                            keyword = new Keyword {
                                Word = word
                            };
                            // Add Keyword to List
                            baseMxf.With.Keywords.Add(keyword, tvShowsKeywordGroup);
                            // Add Keyword to KeywordGroup
                            tvShowsKeywordGroup.Add(keyword);
                        }
                    }
                    // Add Keyword to List
                    keywords.Add(keyword);
                }

                // Iterate through Program Keywords
                foreach (Keyword k in keywords) {
                    if (k.Word.Contains("News")) {
                        isNews = true;
                    }
                    if (k.Word.ToLower().Contains("children") || 
                        k.Word.ToLower().Contains("kids") ||
                        k.Word.ToLower().Contains("school")) {
                        isKids = true;
                    }
                    if (k.Word.ToLower().Contains("sport") || 
                        k.Word.ToLower().Contains("motorsport") || 
                        k.Word.ToLower().Contains("racing") ||
                        k.Word.ToLower().Contains("surfing") ||
                        k.Word.ToLower().Contains("afl") ||
                        k.Word.ToLower().Contains("football") ||
                        k.Word.ToLower().Contains("rugby") ||
                        k.Word.ToLower().Contains("cricket") || 
                        k.Word.ToLower().Contains("basketball") || 
                        k.Word.ToLower().Contains("baseball") || 
                        k.Word.ToLower().Contains("softball")) {
                        isSports = true;
                    }
                    if (k.Word.ToLower().Contains("reality")) {
                        isReality = true;
                    }
                    if (k.Word.ToLower().Contains("special")) {
                        isSpecial = true;
                    }
                    if (k.Word.ToLower().Contains("short")) {
                        isShort = true;
                    }
                    if (k.Word.ToLower().Contains("mini series") || 
                        k.Word.ToLower().Contains("miniseries") || 
                        k.Word.ToLower().Contains("mini-series")) {
                        isMiniseries = true;
                    }

                }

                #endregion ####################################################


                #region Create WMC GuideImages ################################

                GuideImage guideImage = new GuideImage();

                // Check if Programme has an Icon
                if (programme.Icon != null) {
                    // Set Programme has Image
                    hasImage = true;
                    // Find Guide Image from WMC BaseMXF
                    guideImage = baseMxf.With.GuideImages.GuideImage.FirstOrDefault(xx => xx.ImageUrl == programme.Icon.Src);
                    // Create new Guide Image if None Exists
                    if (guideImage == null) {
                        guideImage = new GuideImage {
                            ImageUrl = programme.Icon.Src
                        };
                        // Add GuideImage to List
                        baseMxf.With.GuideImages.Add(guideImage);
                    }
                }

                #endregion ####################################################


                #region Create WMC People #####################################

                // Implement tv / movie database integration to enable this section

                #endregion ####################################################


                #region Create WMC SeriesInfo #################################

                // Find SeriesInfo from WMC BaseMXF
                SeriesInfo seriesInfo = baseMxf.With.SeriesInfos.SeriesInfo.FirstOrDefault(xx => xx.Title == programme.Title);
                // If the Programme is a TV Show
                if (isSeries) {
                    // Create new SeriesInfo if None Exists
                    if (seriesInfo == null) {
                        // Create SeriesInfo
                        seriesInfo = new SeriesInfo {
                            Title = programme.Title,
                            ShortTitle = programme.Title
                        };
                        // Add SeriesInfo to List
                        baseMxf.With.SeriesInfos.Add(seriesInfo);
                    }
                }

                

                #endregion ####################################################


                #region Create WMC Season #####################################

                // If the Programme is a TV Show
                if (isSeries) {
                    // Find Season from WMC BaseMXF
                    Season season = baseMxf.With.Seasons.Season.FirstOrDefault(xx => xx.Series == seriesInfo.Id);
                    // Create new Season if None Exists
                    if (season == null) {
                        // Create Season
                        season = new Season {
                            Series = seriesInfo.Id,
                            Title = $"{programme.Title}",
                            Year = year
                        };
                        // Check if this is not a Yearly Programme
                        if (!isYearly) {
                            season.Title = $"{programme.Title}: Season {seasonNo}";
                        }
                        // Add Season to List
                        baseMxf.With.Seasons.Add(season);
                    }
                }

                #endregion ####################################################


                #region Create WMC Program ####################################

                // Find Program from WMC BaseMXF
                Program program = baseMxf.With.Programs.Program.FirstOrDefault(xx => xx.Title == programme.Title && xx.EpisodeTitle == programme.Subtitle && xx.Description == programme.Desc);
                // Create new Program if None Exists
                if (program == null) {
                    // Create Program
                    program = new Program {
                        Title = programme.Title,
                        Description = programme.Desc,
                        ShortDescription = programme.Desc,
                        EpisodeTitle = programme.Subtitle,
                        IsMovie = isMovie,
                        IsSeries = isSeries,
                        IsShort = isShort,
                        IsMiniseries = isMiniseries,
                        IsNews = isNews,
                        IsKids = isKids,
                        IsReality = isReality,
                        IsSpecial = isSpecial,
                        IsSports = isSports,
                        Keywords = string.Join(",", keywords.Select(xx => xx.Id)), 
                        Year = year,
                        SeasonNumber = seasonNo,
                        EpisodeNumber = episodeNo
                    };

                    // If the Programme is a TV Show
                    if (isSeries) {
                        program.Series = seriesInfo.Id;
                    }
                    // If the Programme is a Repeat
                    if (isRepeat) {
                        program.OriginalAirdate = originalAirDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    // If the Program has a Guide Image
                    if (hasImage) {
                        program.GuideImage = guideImage.Id;
                    }
                    // Add Program to List
                    baseMxf.With.Programs.Add(program);
                }

                #endregion ####################################################


                #region Create WMC ScheduleEntry ##############################

                // Find Channel from WMC BaseMXF
                Channel channel = baseMxf.With.Lineups.Lineup.First().Channels.Channel.FirstOrDefault(xx => xx.Id == programme.Channel);
                // Find ScheduleEntries from WMC BaseMXF
                ScheduleEntries scheduleEntries = baseMxf.With.ScheduleEntries.FirstOrDefault(xx => xx.Service == channel.Service);
                // Create ScheduleEntry
                ScheduleEntry scheduleEntry = new ScheduleEntry {
                    Program = program.Id,
                    IsHdtv = isHdtv,
                    IsPremiere = isPremiere,
                    IsFinale = isFinale,
                    IsLive = isLive,
                    IsLiveSports = isLiveSports,
                    IsCC = isCC,
                    IsSubtitled = isSubtitled,
                    IsDelay = isDelay,
                    IsTape = isTape,
                    AudioFormat = audioFormat,
                    Part = partNo,
                    Parts = partCount,
                    Duration = (endTime - startTime).TotalSeconds.ToString()
                };
                if (scheduleEntries.ScheduleEntry.Count == 0) {
                    scheduleEntry.StartTime = startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
                }
                // Add ScheduleEntry to List
                scheduleEntries.Add(scheduleEntry);

                #endregion ####################################################

            }

            #endregion ########################################################


            #region Create WMC MXF XML  #######################################

            // Create XML Serializer
            XmlSerializer writer = new XmlSerializer(typeof(MXF));
            // Add NameSpaces to XML
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("sql", "urn:schemas-microsoft-com:XML-sql");
            xmlSerializerNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Set the XML Output Path
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//latest-guide.xml";

            // Write the data to XML
            using (FileStream file = File.Create(path)) {
                writer.Serialize(file, baseMxf, xmlSerializerNamespaces);
                file.Close();
            }

            #endregion ########################################################


            #region Import WMC MXF  ###########################################

            // Retrieve the XML data
            FileStream mxf = File.OpenRead(path);
            // Import the MXF Guide into WMC
            Microsoft.MediaCenter.Store.MXF.MxfImporter.Import(mxf, wmcStore);

            #endregion ########################################################


            #region Map EPG Data ##############################################

            // Get list of MergedLineups
            var mergedLineups = new Microsoft.MediaCenter.Guide.MergedLineups(wmcStore);

            // Iterate through each Merged ineup
            foreach (Microsoft.MediaCenter.Guide.MergedLineup mergedLineup in mergedLineups) {
                // Merge the Lineup
                mergedLineup.FullMerge();
            }

            #endregion ########################################################

        }
    }
}
