namespace BAMCIS.VirusScanService
{
    public class VirusScannerFactory
    {
        public static IVirusScan GetVirusScanner()
        {
            return GetVirusScanner(VirusScannerEnum.CLAM_AV);
        }

        public static IVirusScan GetVirusScanner(VirusScannerEnum scanner)
        {
            switch (scanner)
            {
                default:
                case VirusScannerEnum.CLAM_AV:
                    {
                        return new ClamAVScanner();
                    }
                
            }
        }
    }
}
