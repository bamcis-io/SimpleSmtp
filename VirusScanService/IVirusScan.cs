using System.IO;

namespace BAMCIS.VirusScanService
{
    public interface IVirusScan
    {
        /// <summary>
        ///     Scans a file for a virus
        /// </summary>
        /// <param name="fullPath">The full path to the file</param>
        ScanResult Scan(string path);

        /// <summary>
        ///     Scans some bytes for a virus
        /// </summary>
        /// <param name="bytes">The bytes to scan</param>
        ScanResult Scan(byte[] bytes);

        /// <summary>
        ///     Scans a stream for a virus
        /// </summary>
        /// <param name="stream">The stream to scan</param>
        ScanResult Scan(Stream stream);
    }
}
