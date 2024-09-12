using System.IO;

namespace FileExplorer.Helpers.General
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Writes all bytes into a memory stream
        /// </summary>
        public static MemoryStream WriteMemoryStream(this byte[] bytes)
        {
            var ms = new MemoryStream(bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            return ms;
        }

        /// <summary>
        /// Reads memory stream into array of bytes 
        /// </summary>
        public static byte[] ReadAll(this MemoryStream ms)
        {
            var bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
