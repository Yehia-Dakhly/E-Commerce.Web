using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModuleDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class BasketsController(IServiceManager serviceManager) : ApiBaseController
    {
        // GetBasket
        [HttpGet] // GET/BaseUrl/api/Basket
        public async Task<ActionResult<BasketDTo>> GetBasket(string Key)
        {
            var Basket = await serviceManager.BasketService.GetBasketAsync(Key);
            return Ok(Basket);
        }
        // Create Or Update Basket
        [HttpPost]
        public async Task<ActionResult<BasketDTo>> CreateOrUpdateBasket(BasketDTo basket)
        {
            var Basket = await serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);

            return Ok(Basket);
        }
        // Delete Basket
        [HttpDelete("{Key}")] // DELETE BaseUrl/api/Basket/GUID
        public async Task<ActionResult<bool>> DeleteBasket(string Key)
        {
            var Result = await serviceManager.BasketService.DeleteBasketAsync(Key);
            return Ok(Result);
        }
    }
}
