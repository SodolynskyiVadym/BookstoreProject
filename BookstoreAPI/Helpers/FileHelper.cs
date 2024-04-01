namespace BookstoreAPI.Helpers
{
    public class FileHelper
    {

        string pathBooks = @"../book_store_front/src/assets/bookPhoto/";
        string pathAuthors = @"../book_store_front/src/assets/authorPhoto/";
        string pathPublishers = @"../book_store_front/src/assets/publisherPhoto/";
        string path = @"../book_store_front/src/assets/";


        public void RenamePhoto(string? oldName, string newName, int id, string folderName)
        {
            if (oldName != null)
            {
                string oldFileName = oldName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";
                string newFileName = newName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";


                if (File.Exists($"{path}{folderName}/{oldFileName}"))
                {
                    File.Move($"{path}{folderName}/{oldFileName}", $"{path}{folderName}/{newFileName}");
                }
            } 
        }


        public void DeletePhoto(string? name, int id, string folderName)
        {
            if (name != null)
            {
                if (File.Exists($"{path}{folderName}/{name}"))
                {
                    File.Delete($"{path}{folderName}/{name}");
                }
            }
        }

    }
}
