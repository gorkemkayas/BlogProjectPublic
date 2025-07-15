using BlogProject.Domain.Entities;
using BlogProject.Web.ViewModels;


namespace BlogProject.Web.Utilities
{
    public static class PhotoSaver
    {
        public static async Task<ExtendedProfileViewModel> ConfigurePictureAsync(ExtendedProfileViewModel newUserInfo, AppUser oldUserInfo, IFormFile? formFile ,Domain.Entities.PhotoType type)
        {

            if (formFile is null)
            {
                newUserInfo.SetProperty(type, oldUserInfo.GetPropertyValue(type));
                return newUserInfo;
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "userPhotos", $"{oldUserInfo.UserName}");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = formFile.FileName.Replace(" ", "_");
            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            newUserInfo.SetProperty(type, fileName);

            if (oldUserInfo.GetPropertyValue(type) != null)
            {
                var oldPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, oldUserInfo.GetPropertyValue(type));

                if (File.Exists(oldPhotoPath))
                {
                    File.Delete(oldPhotoPath);
                }

            }
            return newUserInfo;
        }
    }
}
