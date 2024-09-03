using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.DTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    [Route("api/transfer")]
    [ApiController]
    public class APITransferController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IWalletService _walletService;
        private readonly IUsersService _usersService;

        public APITransferController(ICardService cardService, IWalletService walletService, IUsersService usersService)
        {
            _cardService = cardService;
            _walletService = walletService;
            _usersService = usersService;
        }

        [HttpPost("transferFunds")]
        public IActionResult TransferToCard([FromBody] TransferRequestDTO transferRequest, [FromQuery] string transferType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var senderUsername = User.Identity.Name;
            var sender = this._usersService.GetByUsername(senderUsername);
            var sendersCard = _cardService.GetByUserId(sender.Id);
            //var sendersCard = this._cardService.GetByCardHoler(senderUsername);

            var wallet = sender.UserWallets.FirstOrDefault(x => x.Currency == transferRequest.Currency);

            if (transferType == "toWallet")
            {
                this._walletService.AddFunds(transferRequest.Amount, transferRequest.Currency, wallet, sendersCard, sender);
            }
            else if (transferType == "toCard")
            {
                this._walletService.WithdrawFunds(transferRequest.Amount, wallet, sendersCard);
            }
            else
            {
                return BadRequest("Invalid transfer type.");
            }


            return RedirectToAction("TransactionSuccess", "Wallet"); //подлежи на промяна след като изясним логиката по виртуалния портфейл
        }

        //[HttpPost("transferToWallet")]
        //public IActionResult TransferToWallet([FromBody] TransferRequest transferRequest)
        //{
        //    var senderUsername = User.Identity.Name;
        //    var sender = this._usersService.GetByUsername(senderUsername);
        //    var sendersCard = this._cardService.GetByCardHoler(senderUsername);

           

        //    return StatusCode(StatusCodes.Status200OK, transferRequest);     //подлежи на промяна след като изясним логиката по виртуалния портфейл
        //}
    }
}
