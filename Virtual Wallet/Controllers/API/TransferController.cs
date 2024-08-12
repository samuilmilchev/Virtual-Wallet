using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    [Route("api/TransferToWallet")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ICardService _cardService;

        public TransferController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        public IActionResult Transfer([FromBody] TransferRequest transferRequest)
        {
            var senderUsername = User.Identity.Name;
            var sendersCard = this._cardService.GetByCardHoler(senderUsername);

            if (sendersCard.Balance < transferRequest.Amount)
            {
                throw new InsufficientFundsException("Transaction failed due to insufficient funds!");
            }

            return Ok(sendersCard); //подлежи на промяна след като изясним логиката по виртуалния портфейл
        }
    }
}
