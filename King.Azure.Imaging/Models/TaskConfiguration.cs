namespace King.Azure.Imaging.Models
{
    public class TaskConfiguration : ITaskConfiguration
    {
        public IStorageElements StorageElements
        {
            get;
            set;
        }

        public IVersions Versions
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }
    }
}