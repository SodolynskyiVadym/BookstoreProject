namespace BookstoreAPI.Helpers
{
    public class FileHelper
    {

        string pathBooks = @"../book_store_front/src/assets/bookPhoto/";
        string pathAuthors = @"../book_store_front/src/assets/authorPhoto/";
        string pathPublishers = @"../book_store_front/src/assets/publisherPhoto/";


        public void RenameBookPhoto(string? oldName, string newName, int id)
        {
            if (oldName != null)
            {
                string oldFileName = oldName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";
                string newFileName = newName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";


                if (File.Exists(pathBooks + oldFileName))
                {
                    File.Move(pathBooks + oldFileName, pathBooks + newFileName);
                }
            } 
        }


        public void RenameAuthorPhoto(string? oldName, string newName, int id)
        {
            if (oldName != null)
            {
                string oldFileName = oldName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";
                string newFileName = newName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";


                if (File.Exists(pathAuthors + oldFileName))
                {
                    File.Move(pathAuthors + oldFileName, pathAuthors + newFileName);
                }
            }
        }


        public void RenamePublisherPhoto(string? oldName, string newName, int id)
        {
            if (oldName != null)
            {
                string oldFileName = oldName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";
                string newFileName = newName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";


                if (File.Exists(pathPublishers + oldFileName))
                {
                    File.Move(pathPublishers + oldFileName, pathPublishers + newFileName);
                }
            }
        }

    }
}
