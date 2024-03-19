namespace BookstoreAPI.Helpers
{
    public class FileHelper
    {

        string pathBooks = @"../book_store_front/src/assets/bookPhoto/";


        public void RenameBookPhoto(string? oldName, string newName, int id)
        {
            if (oldName != null)
            {
                string oldFileName = oldName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";
                string newFileName = newName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";

                Console.WriteLine(oldFileName);
                Console.WriteLine(newFileName);
                Console.WriteLine(File.Exists(pathBooks + oldFileName));
                if (File.Exists(pathBooks + oldFileName))
                {
                    File.Move(pathBooks + oldFileName, pathBooks + newFileName);
                }

            }
            
        }

    }
}
