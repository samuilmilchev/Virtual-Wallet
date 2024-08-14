using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    [Route("api/transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IWalletService _walletService;
        private readonly IUsersService _usersService;

        public TransferController(ICardService cardService, IWalletService walletService, IUsersService usersService)
        {
            _cardService = cardService;
            _walletService = walletService;
            _usersService = usersService;
        }

        [HttpPost("transferToCard")]
        public IActionResult TransferToCard([FromBody] TransferRequest transferRequest)
        {
            var senderUsername = User.Identity.Name;
            var sender = this._usersService.GetByUsername(senderUsername);
            var sendersCard = this._cardService.GetByCardHoler(senderUsername);

            this._walletService.WithdrawFunds(transferRequest.Amount , sender.UserWallet , sendersCard);

            return StatusCode(StatusCodes.Status200OK , transferRequest);     //подлежи на промяна след като изясним логиката по виртуалния портфейл
        }

        [HttpPost("transferToWallet")]
        public IActionResult TransferToWallet([FromBody] TransferRequest transferRequest)
        {
            var senderUsername = User.Identity.Name;
            var sender = this._usersService.GetByUsername(senderUsername);
            var sendersCard = this._cardService.GetByCardHoler(senderUsername);

            this._walletService.AddFunds(transferRequest.Amount , sender.UserWallet , sendersCard);

            return StatusCode(StatusCodes.Status200OK, transferRequest);     //подлежи на промяна след като изясним логиката по виртуалния портфейл
        }
    }
}
