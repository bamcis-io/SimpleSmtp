using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BAMCIS.SimpleSmtp
{
    public class Configuration
    {
        private static readonly string _filePath = AppDomain.CurrentDomain.BaseDirectory + "Config\\config.json";
        private static volatile Configuration _instance;
        private static object _syncRoot = new object();
        private static ulong _sessionId = 0;

        public List<string> UnallowedSenders { get; set; }
        public bool HeadersMustMatch { get; set; }
        public bool UseSMTPS { get; set; }
        public int ReceiveTimeout { get; set; }
        public List<string> IPBlackList { get; set; }
        public List<string> IPWhiteList { get; set; }
        public List<string> DnsBlackList { get; set; }
        public List<string> DnsWhiteList { get; set; }
        public bool CheckReverseDnsLookup { get; set; }
        public bool StoreMessages { get; set; }
        public int MaxMessageSizeInKiB { get; set; }
        public int MaxSessions { get; set; }
        public bool AllowPrivateIPs { get; set; }
        public bool UseGreyListing { get; set; }
        public string Domain { get; set; }
        public List<string> AllowedRelayDomains { get; set; }
        public List<string> BlockedRelayDomains { get; set; }
        public int MaxMessages { get; set; }
        public int MaxErrors { get; set; }
        public int MaxVerifyRequests { get; set; }
        public int MaxRecipients { get; set; }
        public bool VirusScanAttachments { get; set; }
        public VirusScanService.VirusScannerEnum VirusScanner { get; set; }
        public int MaxAttachmentSizeToScanInBytes { get; set; }
        public bool UseAuthentication { get; set; }
        public bool AllowNonDomainSenders { get; set; }
        public string OutgoingFolder { get; set; }
        public string QuarantineFolder { get; set; }

        public static string GetSessionId()
        {
            string Id = String.Empty;
            lock (_syncRoot)
            {
                if (_sessionId == ulong.MaxValue)
                {
                    _sessionId = 0;
                }

                Id = String.Format("{0:X}{1:X}", DateTime.Now.Ticks, ++_sessionId);
            }

            return Id;
        }

        private Configuration()
        {
            this.UnallowedSenders = new List<string>();
            this.IPBlackList = new List<string>();
            this.IPWhiteList = new List<string>();
            this.DnsBlackList = new List<string>();
            this.DnsWhiteList = new List<string>();
            this.CheckReverseDnsLookup = true;
            this.AllowedRelayDomains = new List<string>();
            this.BlockedRelayDomains = new List<string>();
            this.HeadersMustMatch = false;
            this.UseSMTPS = false;
            this.ReceiveTimeout = -1;
            this.MaxMessageSizeInKiB = 10 * 1024; //10 MiB
            this.MaxSessions = -1;
            this.AllowPrivateIPs = true;
            this.Domain = Environment.MachineName + ".local";
            this.MaxMessages = -1;
            this.MaxErrors = -1;
            this.MaxVerifyRequests = -1;
            this.MaxRecipients = -1;
            this.VirusScanAttachments = false;
            this.VirusScanner = VirusScanService.VirusScannerEnum.CLAM_AV;
            this.MaxAttachmentSizeToScanInBytes = 26214400; //25 Mb
            this.UseAuthentication = false;
            this.UseGreyListing = false;
            this.AllowPrivateIPs = true;
            this.AllowNonDomainSenders = true;
            this.OutgoingFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outgoing");
            this.QuarantineFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Quarantine");
        }

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = ReadConfig(_filePath);

                            if (!Directory.Exists(_instance.OutgoingFolder))
                            {
                                Directory.CreateDirectory(_instance.OutgoingFolder);
                            }

                            if (!Directory.Exists(_instance.QuarantineFolder))
                            {
                                Directory.CreateDirectory(_instance.QuarantineFolder);
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        private static Configuration ReadConfig(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader SReader = new StreamReader(filePath))
                {
                    string Content = SReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Configuration>(Content);
                }
            }
            else
            {
                return new Configuration();
            }
        }
    }
}
