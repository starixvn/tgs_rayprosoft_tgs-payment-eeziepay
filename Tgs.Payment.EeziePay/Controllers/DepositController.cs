using System;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    public class DepositController : ControllerBase
    {
        private readonly ILogger<DepositController> _logger;

        public DepositController(
            ILogger<DepositController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("callback")]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/xml")]
        [FormatFilter]
        public async Task<CallbackConfirmViewModel> DepositCallbackAsync(
            //[FromServices] IOptions<JsonOptions> jsonOptions,
            [FromServices] IGetService<GetDepositFormQuery, DepositFormInfo> getDepositFormService,
            [FromServices] ICommandService<SaveDepositReturnCommand> saveReturnService,
            [FromServices] ICommandService<ApproveDepositFormCommand> approveService,
            [FromForm] IFormCollection form)
        {
            //TODO: Check HttpMethod and Request
            //Learn LeePay change DepositResponseViewModel request

            //var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

            //var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

            //_logger.LogInformation(json);

            //var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;

            foreach (var p in form)
            {
                Console.WriteLine($"{p.Key}: {p.Value}");
            }

            var depositForm = await getDepositFormService
                .GetAsync(new GetDepositFormQuery("request.partner_orderid"))
                .ConfigureAwait(false);

            if (depositForm.Status < 1000)
            {
                //TODO: EeziePayDepositReturn Property
                var callbackMessage = new EeziePayDepositReturn()
                {
                };

                await saveReturnService
                    .ExecuteAsync(new SaveDepositReturnCommand(depositForm.Id, callbackMessage))
                    .ConfigureAwait(false);

                //TODO: 可能要替換NamingPolicy
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                _logger.LogInformation(JsonSerializer.Serialize(callbackMessage, jsonSerializerOptions));

                await approveService
                    .ExecuteAsync(new ApproveDepositFormCommand(depositForm.Id, depositForm.BrandId))
                    .ConfigureAwait(false);
            };

            //TODO: Prepare Return Xml & Fill BillNo
            //<xml>
            //    <billno>20140707123456789</billno>
            //    <status>OK</status>
            //</xml>
            //var result = new { billno = "", status = "OK" };

            return new CallbackConfirmViewModel()
            {
                BillNo = "20140707123456789",
                Status = "OK"
            };
        }
    }
}