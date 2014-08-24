namespace King.Azure.Imaging
{
    using System;
    using System.Drawing.Imaging;
    using System.IO;

    public class Resizer
    {
        #region Methods
        /// <summary>
        /// Large
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="format">Format</param>
        /// <returns>Bytes</returns>
        public virtual byte[] Large(byte[] data, ImageFormat format)
        {
            if (null == data)
            {
                throw new ArgumentNullException("data");
            }

            var images = new Images();
            using (var stream = new MemoryStream())
            {
                stream.Write(data, 0, data.Length);
                return images.ResizeByWidth(stream, format, 960).Data;
            }
        }
        #endregion
    }
}