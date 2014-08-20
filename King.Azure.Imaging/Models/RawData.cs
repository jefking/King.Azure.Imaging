namespace King.Azure.Imaging.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RawData
    {
        public byte[] Contents { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public Guid Identifier { get; set; }
    }
}