using System.IO;

namespace CinemaSuite.Utility
{
    public class Files
    {
        public const string ERROR_FORMAT_DUPLICATE_FILE_NAME = "The file {0}.{2} already exists in target folder. Saved as {1}.{2}.";

        /// <summary>
        /// Returns a unique file name in the specified folder, adding a number postfix for duplicates.
        /// </summary>
        /// <param name="folder">The full path to the folder</param>
        /// <param name="fileName">The original filename</param>
        /// <param name="extension">The file extension</param>
        /// <returns>The new filename.</returns>
        public static string GetUniqueFilename(string folder, string filename, string extension)
        {
            int i = 1;
            while (File.Exists(string.Format("{0}/{1}{2}.{3}", folder, filename, i, extension)))
            {
                i++;
            }
            return string.Format("{0}{1}", filename, i);
        }
    }
}