namespace King.Azure.Imaging
{
    using System.Threading.Tasks;

    public interface IImagePreProcessor
    {
        Task Process(byte[] content, string contentType, string fileName);
    }
}