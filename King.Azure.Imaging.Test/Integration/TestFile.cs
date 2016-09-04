namespace King.Azure.Imaging.Test.Integration
{
    using System.IO;
    using System.Reflection;

    public class TestFile
    {
        public static string IconFile
        {
            get
            {
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return dir.Substring(0, dir.Length - 9) + @"\icon.png";
            }
        }
        public static byte[] Icon()
        {
            return File.ReadAllBytes(IconFile);
        }
    }
}