namespace King.Azure.Imaging.Models
{
    using System;

    /// <summary>
    /// Raw Uploaded Data
    /// </summary>
    public class RawData
    {
        #region Properties
        public byte[] Contents { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public Guid Identifier { get; set; }
        #endregion
    }
}