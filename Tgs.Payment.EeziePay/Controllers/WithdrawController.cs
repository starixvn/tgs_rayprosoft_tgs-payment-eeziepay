using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tgs.Payment.EeziePay.Services;
using Tgs.Payment.EeziePay.ViewModels;
using TGS.ServiceUtility;

namespace Tgs.Payment.EeziePay.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        private readonly ILogger<WithdrawController> _logger;

        public WithdrawController(
            ILogger<WithdrawController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("callback")]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/xml")]
        [FormatFilter]
        public async Task<CallbackConfirmViewModel> WithdrawCallbackAsync(
            //[FromServices] IOptions<JsonOptions> jsonOptions,
            [FromServices] IGetService<GetWithdrawFormQuery, WithdrawFormInfo> getWithdrawFormService,
            [FromServices] ICommandService<SaveWithdrawReturnCommand> saveReturnService,
            [FromForm] IFormCollection form)
        {
            //TODO: Check HttpMethod and Request
            //Learn LeePay change WithdrawResponseViewModel request

            //var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

            //var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

            //_logger.LogInformation(json);

            //var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
            foreach (var p in form)
            {
                Console.WriteLine($"{p.Key}: {p.Value}");
            }

            //var jsonSerializerOptions = jsonOptions.Value.JsonSerialOptions;

            var withdrawForm = await getWithdrawFormService
                .GetAsync(new GetWithdrawFormQuery("request.partner_orderid"))
                .ConfigureAwait(false);

            //TODO: EeziePayWithdrawReturn Property
            var callbackMessage = new EeziePayWithdrawReturn()
            {
            };

            await saveReturnService
                .ExecuteAsync(new SaveWithdrawReturnCommand(withdrawForm.Id, callbackMessage))
                .ConfigureAwait(false);

            //_logger.LogInformation(JsonSerializer.Serialize(callbackMessage, jsonSerializerOptions));
            _logger.LogInformation(JsonSerializer.Serialize(callbackMessage));

            //TODO: Prepare Return Xml & Fill BillNo
            //<xml>
            //    <billno>20140707123456789</billno>
            //    <status>OK</status>
            //</xml>

            return new CallbackConfirmViewModel()
            {
                BillNo = "20140707123456789",
                Status = "OK"
            };
        }
    }
}