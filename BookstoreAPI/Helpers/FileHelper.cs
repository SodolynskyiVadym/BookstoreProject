namespace BookstoreAPI.Helpers
{
    public class FileHelper
    {

        string pathBooks = @"../book_store_front/src/assets/bookPhoto/";
        string pathAuthors = @"../book_store_front/src/assets/authorPhoto/";


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

    }
}
