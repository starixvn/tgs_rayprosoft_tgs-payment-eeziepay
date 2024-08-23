using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tgs.Payment.EeziePay
{
    public class EeziePayClient
    {
        private readonly HttpClient _httpClient;
        private readonly EeziePayHasher _eeziePayHasher;
        private readonly JsonSerializerOptions? _jsonSerializerOptions;
        private readonly ILogger<EeziePayClient> _logger;

        public EeziePayClient(
            HttpClient httpClient,
            IOptions<EeziePayOptions> optionsAccessor,
            EeziePayHasher eeziePayHasher,
            ILogger<EeziePayClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonSerializerOptions = optionsAccessor.Value.SystemSerializerOptions;
            _eeziePayHasher = eeziePayHasher ?? throw new ArgumentNullException(nameof(eeziePayHasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async ValueTask<EeziePayDepositResponse?> DepositAsync(string merchantEncryptionKey,DepositOrder order)
        {
            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            var sign = _eeziePayHasher.ComputeHash(merchantEncryptionKey, orderDict);
            order.Signature = sign;

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //TODO:補Url
            var response = await _httpClient
                .PostAsync("", content)
                .ConfigureAwait(false);

            _logger.LogInformation($"Deposit StatusCode: {response.StatusCode}");

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogInformation($"Deposit Body: {bodyJson}");

            response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<EeziePayDepositResponse>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;

        }

        public async ValueTask<EeziePayWithdrawResponse?> WithdrawAsync(string merchantEncryptionKey, WithdrawOrder order)
        {


            var orderJson = JsonSerializer.Serialize(order, _jsonSerializerOptions);
            var orderDict = JsonSerializer
                .Deserialize<Dictionary<string, object>>(orderJson, _jsonSerializerOptions)!;

            order.Signature = _eeziePayHasher.ComputeHash(merchantEncryptionKey, orderDict);

            //For only VND and IDR currencies, our side doesn’t accept
            //cents value but accept integer into the amount from the
            //parameter.For example of VND 100,000.88 and IDR
            //100,000.88, these amounts are invalid and unacceptable from
            //our side.

            var paramDict = orderDict
                .AsEnumerable()
                .Select(p => new KeyValuePair<string?, string?>(p.Key, p.Value?.ToString()));

            var content = new FormUrlEncodedContent(paramDict);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //TODO: 補Url
            var response = await _httpClient
                .PostAsync("", content)
                .ConfigureAwait(false);

            _logger.LogInformation($"Deposit StatusCode: {response.StatusCode}");

            var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogInformation($"Deposit Body: {bodyJson}");

            response.EnsureSuccessStatusCode();

            var apiResponseData = JsonSerializer.Deserialize<EeziePayWithdrawResponse>(bodyJson, _jsonSerializerOptions);

            return apiResponseData;

        }
    }
}
