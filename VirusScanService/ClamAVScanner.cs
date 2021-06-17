using nClam;
using System;
using System.IO;
using System.Linq;

namespace BAMCIS.VirusScanService
{
    public class ClamAVScanner : IVirusScan
    {
        private string _server;
        private int _port;
        ClamClient _clam; 

        public ClamAVScanner()
        {
            this._server = "localhost";
            this._port = 3310;
            this._clam = new ClamClient(this._server, this._port);
        }

        public ClamAVScanner(string server)
        {
            this._server = server;
            this._port = 3310;
            this._clam = new ClamClient(this._server, this._port);
        }

        public ClamAVScanner(string server, int port)
        {
            this._server = server;
            this._port = port;
            this._clam = new ClamClient(this._server, this._port);
        }

        public ScanResult Scan(string path)
        {
            return CreateScanResult(this._clam.SendAndScanFile(path));
        }

        public ScanResult Scan(byte[] bytes)
        {
            return CreateScanResult(this._clam.SendAndScanFile(bytes));
        }

        public ScanResult Scan(Stream stream)
        {
            return CreateScanResult(this._clam.SendAndScanFile(stream));
        }

        private static ScanResult CreateScanResult(ClamScanResult result)
        {
            ScanResult Result = new ScanResult();
            switch (result.Result)
            {
                default:
                case ClamScanResults.Unknown:
                    {
                        Result.Result = ScanResultEnum.UNKNOWN;
                        Result.Message = "Could not scan file";
                        break;
                    }
                case ClamScanResults.Clean:
                    {
                        Result.Result = ScanResultEnum.CLEAN;
                        Result.Message = "No virus found";
                        break;
                    }
                case ClamScanResults.VirusDetected:
                    {
                        Result.Result = ScanResultEnum.INFECTED;
                        Result.Message = String.Format("Virus(es) detected: {0}", String.Join(",", result.InfectedFiles.Select(x => x.VirusName).ToArray()));
                        break;
                    }
                case ClamScanResults.Error:
                    {
                        Result.Result = ScanResultEnum.ERROR;
                        Result.Message = String.Format("VIRUS SCAN ERROR! {0}", result.RawResult);
                        break;
                    }
            }

            return Result;
        }
    }
}
