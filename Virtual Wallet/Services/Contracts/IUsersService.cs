﻿using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IUsersService
    {
        IQueryable<User> GetAll();
        User GetByUsername(string username);
        User GetByEmail(string email);
        User GetById(int id);
        bool UserEmailExists(string email);
        User Create(User user);
        User Update(int id, User userUpdate/*, User user*/);
        bool UploadPhotoVerification(string selfie, string idPhoto, User user);
        List<VerificationApply> GetAllVereficationApplies();
        void UpdateUserVerification(User user, string text);
        void AddUserCard(Card card, User user);
        bool Delete(int id, User user);
        bool BlockUser(int userId, User user);
        bool UnblockUser(int userId, User user);
        List<User> FilterBy(UserQueryParameters filterParameters);
        User FindRecipient(UserQueryParameters filterParameters);

        Task SendConfirmationEmailAsync(User user);
    }
}
