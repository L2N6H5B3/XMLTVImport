﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using XMLTVImport.Classes.XMLTV;
using XMLTVImport.Classes.MXF;
using System.Diagnostics;

namespace XMLTVImport {
    class Import {

        private static string url;
        private static string xml;
        private static XmlSerializer serializer;
        private static TV xmltv;
        private static Classes.State.State state;
        private static MXF baseMxf;
        private static string workingDir;
        private static string mxfPath;
        private static string statePath;

        static void Main(string[] args) {

            #region Set Variables #############################################

            // Set Working Directory
            workingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Windows Media Center", "XMLTV");
            // Set XML Paths
            mxfPath = Path.Combine(workingDir, "latest-guide.xml");
            statePath = Path.Combine(workingDir, "current-state.xml");

            #endregion ########################################################


            #region Get WMC ObjectStore #######################################

            //string s = "Unable upgrade recording state.";
            //byte[] bytes = Convert.FromBase64String("FAAODBUITwADRicSARc=");

            //byte[] buffer2 = Encoding.ASCII.GetBytes(s);
            //for (int i = 0; i != bytes.Length; i++) {
            //    bytes[i] = (byte)(bytes[i] ^ buffer2[i]);
            //}

            //string clientId = Microsoft.MediaCenter.Store.ObjectStore.GetClientId(true);
            //SHA256Managed managed = new SHA256Managed();
            //byte[] buffer = Encoding.Unicode.GetBytes(clientId);
            //clientId = Convert.ToBase64String(managed.ComputeHash(buffer));

            //// Get WMC ObjectStore
            //ObjectStore wmcStore = ObjectStore.Open("", Encoding.ASCII.GetString(bytes), clientId, true);

            #endregion ########################################################


            #region Get WMC Guide Objects #####################################

            baseMxf = new MXF();
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
            url = ConfigurationManager.AppSettings.Get("xmltv-uri");
            // Initialise XML Serializer
            serializer = new XmlSerializer(typeof(TV));

            // Use WebClient to Download XMLTV XML
            using (WebClient client = new WebClient()) {
                // Set XML Encoding Type
                client.Encoding = Encoding.UTF8;
                // Download XMLTV File
                xml = client.DownloadString(url);
            }

            // Use TextReader to Create Objects from XML
            using (TextReader reader = new StringReader(xml)) {
                // Deserialise XML
                xmltv = (TV)serializer.Deserialize(reader);
            }

            #endregion ########################################################


            #region Get State #################################################

            if (File.Exists(statePath)) {
                // Use StreamReader to Create Objects from XML
                using (StreamReader reader = new StreamReader(statePath)) {
                    // Initialise XML Serializer
                    serializer = new XmlSerializer(typeof(Classes.State.State));
                    // Deserialise XML
                    state = (Classes.State.State)serializer.Deserialize(reader);
                }
            } else {
                // Create new State Object
                state = new Classes.State.State { 
                    KeywordGroups = new Classes.State.KeywordGroups(),
                    SeriesInfos = new Classes.State.SeriesInfos(),
                    Seasons = new Classes.State.Seasons(),
                    Programs = new Classes.State.Programs(),
                    Services = new Classes.State.Services()
                };
            }
                
            #endregion ########################################################


            #region Create WMC KeywordGroups ##################################

            // Create All Top-Level Keyword
            Keyword allKeyword = new Keyword {
                Word = "All"
            };
            // Add Keyword to List
            baseMxf.With.Keywords.Add(allKeyword);

            // Create Movies Top-Level Keyword
            Keyword moviesKeyword = new Keyword {
                Word = "Movies"
            };
            // Add Keyword to List
            baseMxf.With.Keywords.Add(moviesKeyword);

            // Create TV Shows Top-Level Keyword
            Keyword tvShowsKeyword = new Keyword {
                Word = "TV Shows"
            };
            // Add Keyword to List
            baseMxf.With.Keywords.Add(tvShowsKeyword);

            // Initialise TV Shows KeywordGroup Variable
            KeywordGroup moviesKeywordGroup;

            // Find KeywordGroup from State
            Classes.State.KeywordGroup stateMoviesKeywordGroup = state.KeywordGroups.KeywordGroup.FirstOrDefault(xx => xx.Name == moviesKeyword.Word);
            // Check if the State KeywordGroup Exists
            if (stateMoviesKeywordGroup != null) {
                // Create Movies KeywordGroup
                moviesKeywordGroup = new KeywordGroup {
                    GroupName = moviesKeyword.Id,
                    Name = moviesKeyword.Word,
                    Uid = stateMoviesKeywordGroup.Uid
                };
                // Add the All Keyword to the KeywordGroup
                moviesKeywordGroup.Add(allKeyword);
                // Add the KeywordGroup to the BaseMXF
                baseMxf.With.KeywordGroups.AddExisting(moviesKeywordGroup);
            } else {
                // Create Movies KeywordGroup
                moviesKeywordGroup = new KeywordGroup {
                    GroupName = moviesKeyword.Id,
                    Name = moviesKeyword.Word
                };
                // Add the All Keyword to the KeywordGroup
                moviesKeywordGroup.Add(allKeyword);
                // Add the KeywordGroup to the BaseMXF
                baseMxf.With.KeywordGroups.AddNew(moviesKeywordGroup, state.KeywordGroups.GetNextAvailableUid());
                // Add the KeywordGroup to the State
                state.KeywordGroups.Add(moviesKeywordGroup);
            }

            // Initialise TV Shows KeywordGroup Variable
            KeywordGroup tvShowsKeywordGroup;

            // Find KeywordGroup from State
            Classes.State.KeywordGroup stateTvShowsKeywordGroup = state.KeywordGroups.KeywordGroup.FirstOrDefault(xx => xx.Name == tvShowsKeyword.Word);
            // Check if the State KeywordGroup Exists
            if (stateTvShowsKeywordGroup != null) {
                // Create TV Shows KeywordGroup
                tvShowsKeywordGroup = new KeywordGroup {
                    GroupName = tvShowsKeyword.Id,
                    Name = tvShowsKeyword.Word,
                    Uid = stateTvShowsKeywordGroup.Uid
                };
                // Add the All Keyword to the KeywordGroup
                tvShowsKeywordGroup.Add(allKeyword);
                // Add the KeywordGroup to the BaseMXF
                baseMxf.With.KeywordGroups.AddExisting(tvShowsKeywordGroup);
            } else {
                // Create TV Shows KeywordGroup
                tvShowsKeywordGroup = new KeywordGroup {
                    GroupName = tvShowsKeyword.Id,
                    Name = tvShowsKeyword.Word
                };
                // Add the All Keyword to the KeywordGroup
                tvShowsKeywordGroup.Add(allKeyword);
                // Add the KeywordGroup to the BaseMXF
                baseMxf.With.KeywordGroups.AddNew(tvShowsKeywordGroup, state.KeywordGroups.GetNextAvailableUid());
                // Add the KeywordGroup to the State
                state.KeywordGroups.Add(tvShowsKeywordGroup);
            }

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
                Name = ConfigurationManager.AppSettings.Get("xmltv-location"),
                PrimaryProvider = "!MCLineup!MainLineup",
                ShortName = ConfigurationManager.AppSettings.Get("xmltv-name"),
                Channels = new Channels()
            };
            // Add Lineup to List
            baseMxf.With.Lineups.Add(lineup);

            #endregion ########################################################


            #region Iterate XMLTV Channels ####################################

            // Iterate through XMLTV Channels
            foreach (XMLTVChannel xmltvChannel in xmltv.Channel.OrderBy(xx => int.Parse(xx.Lcn))) {

                #region Create WMC GuideImages ################################

                string guideImageDir = Path.Combine(workingDir, "GuideImages");
                Directory.CreateDirectory(guideImageDir);

                // Find Guide Image from WMC BaseMXF
                GuideImage guideImage = baseMxf.With.GuideImages.GuideImage.FirstOrDefault(xx => xx.ChannelNo == xmltvChannel.Lcn);
                // Check if Guide Image Exists
                if (guideImage == null) {
                    // Using WebClient
                    using (var client = new WebClient()) {
                        // Set the WebClient Security Protocol
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        // Get the URI of the GuideImage Icon
                        string uri = ConfigurationManager.AppSettings.Get($"logo-{xmltvChannel.Lcn}");
                        // Download the GuideImage Icon to the local working directory
                        client.DownloadFile(uri, Path.Combine(guideImageDir, $"{xmltvChannel.Lcn}.png"));
                    }
                    // Create new Guide Image if None Exists
                    guideImage = new GuideImage {
                        ImageUrl = $"file://{Path.Combine(guideImageDir, $"{xmltvChannel.Lcn}.png")}",
                        ChannelNo = xmltvChannel.Lcn
                    };
                    // Add GuideImage to List
                    baseMxf.With.GuideImages.Add(guideImage);
                }

                #endregion ####################################################


                #region Create WMC Service ####################################

                // Find Service from WMC BaseMXF
                Service service = baseMxf.With.Services.Service.FirstOrDefault(xx => xx.Name == xmltvChannel.Displayname);
                // Create new Service if None Exists
                if (service == null) {

                    // Get the Service CallSign
                    string serviceCallSign = ConfigurationManager.AppSettings.Get($"callsign-{xmltvChannel.Lcn}");

                    // Find Service from State
                    Classes.State.Service stateService = state.Services.Service.FirstOrDefault(xx => xx.Name == xmltvChannel.Displayname && xx.CallSign == serviceCallSign);
                    // Check if the State Service Exists
                    if (stateService != null) {
                        service = new Service {
                            Name = xmltvChannel.Displayname,
                            LogoImage = guideImage.Id,
                            CallSign = serviceCallSign,
                            Uid = stateService.Uid
                        };
                        // Add Service to List
                        baseMxf.With.Services.AddExisting(service);
                    } else {
                        service = new Service {
                            Name = xmltvChannel.Displayname,
                            LogoImage = guideImage.Id,
                            CallSign = ConfigurationManager.AppSettings.Get($"callsign-{xmltvChannel.Lcn}")
                        };
                        // Add Service to List
                        baseMxf.With.Services.AddNew(service, state.Services.GetNextAvailableUid());
                        // Add the Service to the State
                        state.Services.Add(service);
                    }
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
                        ServiceUid = service.Uid,
                        Id = xmltvChannel.Id,
                        Lineup = lineup.Id,
                        MatchName = ConfigurationManager.AppSettings.Get($"callsign-{xmltvChannel.Lcn}")
                    };
                    // Add Channel to List
                    baseMxf.With.Lineups.Lineup.First().Channels.Add(channel, lineup);
                }

                #endregion ####################################################

            }

            #endregion ########################################################


            #region Iterate XMLTV Programmes ##################################

            // Iterate through XMLTV Programmes
            foreach (Programme programme in xmltv.Programme.OrderBy(xx => int.Parse(xmltv.Channel.First(xy => xy.Id == xx.Channel).Lcn)).ThenBy(xx => DateTime.ParseExact(xx.Start, "yyyyMMddHHmmss zzz", new CultureInfo("en-AU")))) {

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
               
                // If the Programme is a TV Show
                if (isSeries) {

                    // Find SeriesInfo from WMC BaseMXF
                    SeriesInfo seriesInfo = baseMxf.With.SeriesInfos.SeriesInfo.FirstOrDefault(xx => xx.Title == programme.Title);
                    // Create new SeriesInfo if None Exists
                    if (seriesInfo == null) {

                        // Find SeriesInfo from State
                        Classes.State.SeriesInfo stateSeriesInfo = state.SeriesInfos.SeriesInfo.FirstOrDefault(xx => xx.Name == programme.Title && xx.ShortName == programme.Title);
                        // Check if the State SeriesInfo Exists
                        if (stateSeriesInfo != null) {
                            // Create SeriesInfo
                            seriesInfo = new SeriesInfo {
                                Title = programme.Title,
                                ShortTitle = programme.Title,
                                Uid = stateSeriesInfo.Uid
                            };
                            // Add SeriesInfo to List
                            baseMxf.With.SeriesInfos.AddExisting(seriesInfo);
                        } else {
                            // Create SeriesInfo
                            seriesInfo = new SeriesInfo {
                                Title = programme.Title,
                                ShortTitle = programme.Title
                            };
                            // Add SeriesInfo to List
                            baseMxf.With.SeriesInfos.AddNew(seriesInfo, state.SeriesInfos.GetNextAvailableUid());
                            // Add the SeriesInfo to the State
                            state.SeriesInfos.AddNew(seriesInfo);
                        }
                    }
                }

                #endregion ####################################################


                #region Create WMC Season #####################################
                
                // If the Programme is a TV Show
                if (isSeries) {

                    // Find SeriesInfo from WMC BaseMXF
                    SeriesInfo seriesInfo = baseMxf.With.SeriesInfos.SeriesInfo.FirstOrDefault(xx => xx.Title == programme.Title);
                    // Find Season from WMC BaseMXF
                    Season season = baseMxf.With.Seasons.Season.FirstOrDefault(xx => xx.Series == seriesInfo.Id);
                    // Create new Season if None Exists
                    if (season == null) {

                        // Get the Season Title
                        string seasonTitle = programme.Title;
                        // Check if this is not a Yearly Programme
                        if (!isYearly) {
                            seasonTitle = $"{programme.Title}: Season {seasonNo}";
                        }

                        // Find Season from State
                        Classes.State.Season stateSeason = state.Seasons.Season.FirstOrDefault(xx => xx.Series == seriesInfo.Uid && xx.Title == seasonTitle && xx.Year == year);
                        // Check if the State Season Exists
                        if (stateSeason != null) {
                            // Create Season
                            season = new Season {
                                Series = seriesInfo.Id,
                                SeriesUid = seriesInfo.Uid,
                                Title = seasonTitle,
                                Year = year,
                                Uid = stateSeason.Uid
                            };
                            // Add Season to List
                            baseMxf.With.Seasons.AddExisting(season);
                        } else {
                            // Create Season
                            season = new Season {
                                Series = seriesInfo.Id,
                                SeriesUid = seriesInfo.Uid,
                                Title = seasonTitle,
                                Year = year
                            };
                            // Add Season to List
                            baseMxf.With.Seasons.AddNew(season, state.Seasons.GetNextAvailableUid());
                            // Add the Season to the State
                            state.Seasons.Add(season);
                        }
                    }
                }

                #endregion ####################################################


                #region Create WMC Program ####################################

                // Find Program from WMC BaseMXF
                Program program = baseMxf.With.Programs.Program.FirstOrDefault(xx => xx.Title == programme.Title && xx.EpisodeTitle == programme.Subtitle && xx.Description == programme.Desc && xx.SeasonNumber == seasonNo && xx.EpisodeNumber == episodeNo);
                // Create new Program if None Exists
                if (program == null) {
                    
                    // Initialise Programme variables
                    Classes.State.Program stateProgram;
                    string programmeSeriesId = null;
                    string programmeSeriesUid = null;
                    string programmeSeasonId = null;
                    string programmeSeasonUid = null;
                    string programmeOriginalAirdate = null;
                    string programmeGuideImage = null;

                    // If the Programme is a TV Show
                    if (isSeries) {
                        // Find SeriesInfo from WMC BaseMXF
                        SeriesInfo seriesInfo = baseMxf.With.SeriesInfos.SeriesInfo.FirstOrDefault(xx => xx.Title == programme.Title);
                        // Find Season from WMC BaseMXF
                        Season season = baseMxf.With.Seasons.Season.FirstOrDefault(xx => xx.Series == seriesInfo.Id);
                        // Set Programme Series
                        programmeSeriesId = seriesInfo.Id;
                        programmeSeriesUid = seriesInfo.Uid;
                        // Set Programme Season
                        programmeSeasonId = season.Id;
                        programmeSeasonUid = season.Uid;
                        // Find Program from State
                        stateProgram = state.Programs.Program.FirstOrDefault(xx => xx.Series == seriesInfo.Uid && xx.Season == season.Uid && xx.Title == programme.Title && xx.Year == year && xx.SeasonNo == seasonNo && xx.EpisodeNo == episodeNo);
                    } 
                    // If the Programme is a Movie
                    else {
                        stateProgram = state.Programs.Program.FirstOrDefault(xx => xx.Title == programme.Title && xx.Year == year && xx.SeasonNo == seasonNo && xx.EpisodeNo == episodeNo);
                    }

                    // If the Programme is a Repeat
                    if (isRepeat) {
                        programmeOriginalAirdate = originalAirDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    // If the Program has a Guide Image
                    if (hasImage) {
                        programmeGuideImage = guideImage.Id;
                    }

                   
                    // Check if the State Program Exists
                    if (stateProgram != null) {
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
                            EpisodeNumber = episodeNo,
                            Series = programmeSeriesId,
                            SeriesUid = programmeSeriesUid,
                            Season = programmeSeasonId,
                            SeasonUid = programmeSeasonUid,
                            OriginalAirdate = programmeOriginalAirdate,
                            GuideImage = programmeGuideImage,
                            Uid = stateProgram.Uid
                        };
                        // Add Program to List
                        baseMxf.With.Programs.AddExisting(program);
                    } else {
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
                            EpisodeNumber = episodeNo,
                            Series = programmeSeriesId,
                            SeriesUid = programmeSeriesUid,
                            Season = programmeSeasonId,
                            SeasonUid = programmeSeasonUid,
                            OriginalAirdate = programmeOriginalAirdate,
                            GuideImage = programmeGuideImage,
                        };
                        // Add Program to List
                        baseMxf.With.Programs.AddNew(program, state.Programs.GetNextAvailableUid());
                        // Add the Program to the State
                        state.Programs.Add(program);
                    }
                }

                #endregion ####################################################


                #region Create WMC ScheduleEntry ##############################

                // Find Channel from WMC BaseMXF
                Channel channel = baseMxf.With.Lineups.Lineup.First().Channels.Channel.FirstOrDefault(xx => xx.Id == programme.Channel);
                // Find ScheduleEntries from WMC BaseMXF
                ScheduleEntries scheduleEntries = baseMxf.With.ScheduleEntries.FirstOrDefault(xx => xx.Service == channel.Service);
                // Find Existing ScheduleEntry
                ScheduleEntry scheduleEntry = scheduleEntries.ScheduleEntry.FirstOrDefault(xx => xx.StartDateTime == startTime);
                // Check if ScheduleEntry already exists
                if (scheduleEntry == null) {

                    // Create ScheduleEntry
                    scheduleEntry = new ScheduleEntry {
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
                        Duration = (int)(endTime - startTime).TotalSeconds,
                        StartDateTime = startTime,
                        EndDateTime = endTime
                    };

                    // If this is the First ScheduleEntry
                    if (scheduleEntries.ScheduleEntry.Count == 0) {
                        // Add the StartTime
                        scheduleEntry.StartTime = startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    // If this is a Subsequent ScheduleEntry
                    else {
                        // Get the Last ScheduleEntry
                        ScheduleEntry lastEntry = scheduleEntries.ScheduleEntry.Last();
                        // If the Last ScheduleEntry EndTime does not match the New ScheduleEntry StartTime
                        if (lastEntry.EndDateTime != startTime) {
                            // If the New ScheduleEntry is set to Start before the Last ScheduleEntry Ends
                            if (lastEntry.EndDateTime > startTime) {
                                // Set the Last ScheduleEntry End Time to the Start Time of the New ScheduleEntry
                                lastEntry.EndDateTime = startTime;
                                // Set the Last ScheduleEntry Duration
                                lastEntry.Duration = (int)(lastEntry.EndDateTime - lastEntry.StartDateTime).TotalSeconds;
                            } else {
                                // Get Service
                                Service service = baseMxf.With.Services.Service.FirstOrDefault(xx => xx.Id == scheduleEntries.Service);
                                // Find Filler Program from WMC BaseMXF
                                Program fillerProgram = baseMxf.With.Programs.Program.FirstOrDefault(xx => xx.Title == service.Name);
                                // Check if Filler Program Exists
                                if (fillerProgram == null) {
                                    // Create new Filler Program
                                    fillerProgram = new Program {
                                        Title = service.Name,
                                        Description = "No EPG Data Available.",
                                        ShortDescription = "No EPG Data Available.",
                                        GuideImage = service.LogoImage,
                                        Year = DateTime.Now.Year.ToString()
                                    };
                                    // Add new Filler Program to List
                                    baseMxf.With.Programs.AddNew(fillerProgram, state.Programs.GetNextAvailableUid());
                                }
                                // Create new Filler ScheduleEntry
                                ScheduleEntry fillerEntry = new ScheduleEntry() {
                                    Program = fillerProgram.Id,
                                    Duration = (int)(startTime - lastEntry.EndDateTime).TotalSeconds,
                                    EndDateTime = startTime
                                };
                                // Add new Filler ScheduleEntry to List
                                scheduleEntries.Add(fillerEntry);
                            }
                        }
                    }
                    // Add ScheduleEntry to List
                    scheduleEntries.Add(scheduleEntry);
                }

                #endregion ####################################################
            }

            #endregion ########################################################


            #region Iterate MXF ScheduleEntries ###############################

            //// Iterate through all created ScheduleEntries
            //foreach (ScheduleEntries scheduleEntries in baseMxf.With.ScheduleEntries) {
            //    // If this ScheduleEntries Object has no ScheduleEntry Objects
            //    if (scheduleEntries.ScheduleEntry.Count() == 0) {
            //        // Get Service
            //        Service service = baseMxf.With.Services.Service.FirstOrDefault(xx => xx.Id == scheduleEntries.Service);
            //        // Create new Filler Program
            //        Program program = new Program {
            //            Title = service.Name,
            //            Description = "No Data Available.",
            //            ShortDescription = "No EPG Data Available.",
            //            GuideImage = service.LogoImage,
            //            Year = DateTime.Now.Year.ToString()
            //        };
            //        // Add new Filler Program to List
            //        baseMxf.With.Programs.Add(program);
            //        // Create new Filler ScheduleEntry
            //        ScheduleEntry scheduleEntry = new ScheduleEntry() {
            //            Program = program.Id,
            //            StartTime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"),
            //            Duration = 86400
            //        };
            //        // Add new Filler ScheduleEntry to List
            //        scheduleEntries.Add(scheduleEntry);
            //    }
            //}

            #endregion ########################################################


            #region Sort Data #################################################

            // Sort Keywords
            baseMxf.With.Keywords.Keyword = baseMxf.With.Keywords.Keyword.OrderBy(xx => int.Parse(xx.Id.Replace("k",""))).ToList();

            #endregion ########################################################


            #region Create WMC MXF XML  #######################################

            // Create XML Serializer
            serializer = new XmlSerializer(typeof(MXF));
            // Add NameSpaces to XML
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("sql", "urn:schemas-microsoft-com:XML-sql");
            xmlSerializerNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            // Write the data to XML
            using (FileStream file = File.Create(mxfPath)) {
                serializer.Serialize(file, baseMxf, xmlSerializerNamespaces);
                file.Close();
            }

            #endregion ########################################################


            #region Create State XML  #########################################

            // Create XML Serializer
            serializer = new XmlSerializer(typeof(Classes.State.State));
            // Write the data to XML
            using (FileStream file = File.Create(statePath)) {
                serializer.Serialize(file, state);
                file.Close();
            }

            #endregion ########################################################


            #region Import WMC MXF  ###########################################

            // Import the MXF Guide into WMC
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "ehome", "loadmxf.exe"), $"-i \"{mxfPath}\"");

            //// Retrieve the XML data
            //FileStream mxf = File.OpenRead(path);
            //// Import the MXF Guide into WMC
            //Microsoft.MediaCenter.Store.MXF.MxfImporter.Import(mxf, wmcStore);

            #endregion ########################################################


            #region Map EPG Data ##############################################

            // Get list of MergedLineups
            //var mergedLineups = new Microsoft.MediaCenter.Guide.MergedLineups(wmcStore);

            //// Iterate through
            //foreach (Microsoft.MediaCenter.Guide.Lineup lineup in new Microsoft.MediaCenter.Guide.Lineups(wmcStore)) {
            //    System.Diagnostics.Debug.WriteLine(lineup.Name);

            //}


            // Get list of MergedLineups
            //var merge = new Microsoft.MediaCenter.Guide.MergedLineup();
            //var mergeRule = new Microsoft.MediaCenter.Guide.LineupMergeRule();
            //mergeRule.MergeType = Microsoft.MediaCenter.Guide.MergeType.WmisScanned;
            //merge.LineupMergeRule = mergeRule;
            //merge.Name = "Adelaide-XMLTV+(Scanned (Digital Antenna (DVB-T)))";
            //merge.

            //// Iterate through
            //foreach (Microsoft.MediaCenter.Guide.LineupMergeRule rule in mergeRules) {
            //    System.Diagnostics.Debug.WriteLine(rule.);

            //}


            //var allChannels = new Microsoft.MediaCenter.Guide.Channels(wmcStore).Where(xx => !xx.CallSign.Contains("Deleted"));
            //int count = 1;
            //// Iterate through
            //foreach (Microsoft.MediaCenter.Guide.Channel c in allChannels) {
            //    System.Diagnostics.Debug.WriteLine("-----");
            //    System.Diagnostics.Debug.WriteLine($"Channel {count} of {allChannels.Count()}");
            //    System.Diagnostics.Debug.WriteLine($"CallSign: {c.CallSign}");
            //    System.Diagnostics.Debug.WriteLine($"CallSignHash: {c.CallSignHash}");
            //    System.Diagnostics.Debug.WriteLine($"Number: {c.ChannelNumber}");
            //    System.Diagnostics.Debug.WriteLine($"NumberPriority: {c.ChannelNumberPriority}");
            //    System.Diagnostics.Debug.WriteLine($"ChannelType: {c.ChannelType.ToString()}");
            //    System.Diagnostics.Debug.WriteLine($"TuningInfos: {c.TuningInfos.ToString()}");
            //    System.Diagnostics.Debug.WriteLine($"NumberPriority: {c.ChannelNumberPriority}");
            //    System.Diagnostics.Debug.WriteLine($"UID: {c.UniqueId}");
            //    System.Diagnostics.Debug.WriteLine($"Visibility: {c.Visibility}");

            //    count++;
            //}

            //// Iterate through each Merged ineup
            //foreach (Microsoft.MediaCenter.Guide.MergedLineup mergedLineup in mergedLineups) {


            //    //System.Diagnostics.Debug.WriteLine("Here");
            //    //mergedLineup.ClearChannelCache();
            //    // Merge the Lineup
            //    //mergedLineup.FullMerge();
            //}

            #endregion ########################################################

        }
    }
}
