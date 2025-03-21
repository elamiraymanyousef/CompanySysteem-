namespace Company.PL.HelperImage
{
    public static class DecumentSettings
    {
        // contains tow methods
        // 1- upload image
        public static string UploadImage(IFormFile file, string flodername)
        {
            // 1- get folder path
            //string FullPath = $"E:\\rout file\\MVC Task\\Base mvc project\\New folder\\Company-Session-01\\Company.PL\\wwwroot\\"+{flodername};
            // الملف اللي هيتحفظ فيه الصور
            var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", flodername);


            // 2- file name ande make it unique
            // لجعل اسم الصورة فريد
            var fileName = $"{Guid.NewGuid}{file.FileName}";

            // 3- اجمع المسار الكامل للصورة
            var FullPathImage = Path.Combine(FullPath, fileName);

            using var fileStream = new FileStream(FullPathImage, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }

        // 2- delete image
        public static void DeleteImage(string flodername, string imageName)
        {
            // 1- get folder path
            var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", flodername);
            // 2- اجمع المسار الكامل للصورة
            var FullPathImage = Path.Combine(FullPath, imageName);
            // 3- delete image
            if (File.Exists(FullPathImage))
            {
                File.Delete(FullPathImage);
            }

        }
    }
}
