namespace WebAPI.src.Resource
{
    public class Resource
    {
        public static string BucketName = "prn231assignment.appspot.com";
        public static string GetStoragePath(int id) => "uploads/" + id;
        public static string GetFileUrl(int id) => $"https://storage.googleapis.com/{BucketName}/{GetStoragePath(id)}";
    }
}
