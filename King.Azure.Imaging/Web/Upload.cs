namespace King.Azure.Imaging.Web
{
    using King.Azure.Imaging.Models;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Upload
    /// </summary>
    public class Upload
    {
        #region Members
        /// <summary>
        /// File Name Header
        /// </summary>
        public const string FileNameHeader = "X-File-Name";

        /// <summary>
        /// Content Type Header
        /// </summary>
        public const string ContentTypeHeader = "X-File-Type";
        #endregion

        #region Methods
        public async Task<RawData> Load(HttpRequestMessage request)
        {
            if (null == request)
            {
                throw new ArgumentNullException("request");
            }

            var raw = new RawData()
            {
                Identifier = Guid.NewGuid(),
            };

            if (request.Files.Count > 0)
            {
                var file = request.Files[0];
                raw.Contents = new byte[file.ContentLength];
                await file.InputStream.ReadAsync(raw.Contents, 0, file.ContentLength);
                raw.ContentType = file.ContentType;
                raw.FileName = file.FileName;
            }
            else if (request.ContentLength > 0)
            {
                raw.Contents = new byte[request.ContentLength];
                await request.InputStream.ReadAsync(raw.Contents, 0, request.ContentLength);
                raw.FileName = request.Headers[FileNameHeader];
                raw.ContentType = request.Headers[ContentTypeHeader];
            }

            return raw;
        }
        #endregion
    }
}