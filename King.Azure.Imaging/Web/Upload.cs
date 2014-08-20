namespace King.Azure.Imaging.Web
{
    using King.Azure.Imaging.Models;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class Upload
    {
        public async Task<RawData> Something(HttpRequest request)
        {
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
                raw.FileName = request.Headers["X-File-Name"];
                raw.ContentType = request.Headers["X-File-Type"];
            }

            return raw;
        }
    }
}