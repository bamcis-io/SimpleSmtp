namespace BAMCIS.VirusScanService
{
    /// <summary>
    /// The Results of a Scan
    /// </summary>
    public class ScanResult
    {
        public ScanResultEnum  Result {get; set;}
        public string Message { get; set; }
        public bool IsVirusFree
        {
            get
            {
                return (Result != ScanResultEnum.INFECTED);
            }
        }
    }
}
