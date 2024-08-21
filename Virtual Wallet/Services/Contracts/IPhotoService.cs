using CloudinaryDotNet.Actions;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<DeletionResult> UploadImageAsync(string publicId);
    }
}
