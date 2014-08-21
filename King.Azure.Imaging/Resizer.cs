namespace King.Azure.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Resizer
    {
        public byte[] Large(byte[] data, ImageFormat format)
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
    }
}
