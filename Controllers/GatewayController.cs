using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class GatewayController: Controller
{
  private readonly ApplicationDbContext _context;
  private readonly HttpClient _client;
  [TempData] public string FlashMessage { get; set; }
  [TempData] public string FlashClass { get; set; }

  public GatewayController(ApplicationDbContext context)
  {
    _context = context;
    _client = new HttpClient();
  }

  [Route("/gateways")]
  public IActionResult Index()
  {
    return View();
  }

  [Route("/gateways/webpay")]
  public async Task<IActionResult> GatewayWebpay()
  {
    var data = new {
      buy_order = "buyOrder12345678",
      session_id = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(),
      amount = 1000,
      return_url = "http://127.0.0.1:5281/gateways/webpay/response"
    };
    string contentType = "application/json";
    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
    _client.DefaultRequestHeaders.Add("Tbk-Api-Key-Id", _context.VariablesGlobales.Find(2).Valor);
    _client.DefaultRequestHeaders.Add("Tbk-Api-Key-Secret", _context.VariablesGlobales.Find(3).Valor);

    var result = await _client.PostAsJsonAsync(_context.VariablesGlobales.Find(1).Valor, data);
    string responseBody = await result.Content.ReadAsStringAsync();
    WebpayToken webpayToken = JsonConvert.DeserializeObject<WebpayToken>(responseBody);


    Console.WriteLine($"token = {webpayToken.Token} | URL = {webpayToken.Url}");

    return View(webpayToken);
  }

  [Route("/gateways/webpay/response")]
  public async Task<IActionResult> GatewayWebpayResponse([FromQuery(Name = "token_ws")] string token_ws)
  {
    _client.DefaultRequestHeaders.Add("Tbk-Api-Key-Id", _context.VariablesGlobales.Find(2).Valor);
    _client.DefaultRequestHeaders.Add("Tbk-Api-Key-Secret", _context.VariablesGlobales.Find(3).Valor);
    var result = await _client.PutAsJsonAsync($"{_context.VariablesGlobales.Find(1).Valor}/{token_ws}", new {});
    string responseBody = await result.Content.ReadAsStringAsync();
    WebpayRespuesta webpayRespuesta = JsonConvert.DeserializeObject<WebpayRespuesta>(responseBody);

    return View(webpayRespuesta);
  }

  // ########## PAYPAL ########## //
  private async Task<PaypalToken> ReturnToken()
  {
    var data = new Dictionary<string, string>
    {
      {"grant_type", "client_credentials"}
    };
    HttpClient client = new();
    var authToken = Encoding.ASCII.GetBytes($"{_context.VariablesGlobales.Find(5).Valor}:{_context.VariablesGlobales.Find(6).Valor}");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
    var result = await client.PostAsync($"{_context.VariablesGlobales.Find(4).Valor}/v1/oauth2/token", new FormUrlEncodedContent(data));
    string responseBody = await result.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<PaypalToken>(responseBody);
  }

  [Route("/gateways/paypal")]
  public async Task<IActionResult> GatewayPaypal()
  {
    PaypalToken paypalToken = await ReturnToken();
    string order = $"order_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
    int amount = 10;
    // Pay Order
    var data = new
    {
      intent = "CAPTURE",
      purchase_units = new[] {
        new {
          reference_id = order,
          amount = new {
            currency_code = "USD",
            value = $"{amount}.0"
          },
        },
      },
      payment_source = new {
        paypal = new {
          experience_context = new {
            payment_method_preference = "IMMEDIATE_PAYMENT_REQUIRED",
            payment_method_selected = "PAYPAL",
            brand_name = "Klee",
            locale = "en-EN",
            landing_page = "LOGIN",
            shipping_preference = "NO_SHIPPING",
            user_action = "PAY_NOW",
            return_url = "http://127.0.0.1:5281/gateways/paypal/response",
            cancel_url = "http://127.0.0.1:5281/gateways/paypal/cancel"
          }
        }
      }
    };

    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {paypalToken.AccessToken}");
    _client.DefaultRequestHeaders.Add("PayPal-Request-Id", order);

    var result = await _client.PostAsJsonAsync($"{_context.VariablesGlobales.Find(4).Valor}/v2/checkout/orders", data);
    string responseBody = await result.Content.ReadAsStringAsync();

    PaypalOrder paypalOrder = JsonConvert.DeserializeObject<PaypalOrder>(responseBody);
    PaypalViewModel paypal = new() {
      PaypalToken = paypalToken,
      PaypalOrder = paypalOrder
    };

    // return Content($"Token={paypalToken.AccessToken}");
    return View(paypal);
  }

  [Route("/gateways/paypal/response")]
  public async Task<IActionResult> GateWayPaypalResponse([FromQuery(Name="token")] string token)
  {
    PaypalToken paypalToken = await ReturnToken();
    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {paypalToken.AccessToken}");
    var result = await _client.PostAsJsonAsync($"{_context.VariablesGlobales.Find(4).Valor}/v2/checkout/orders/{token}/capture", new {});
    string responseBody = await result.Content.ReadAsStringAsync();
    PaypalOrder paypalOrder = JsonConvert.DeserializeObject<PaypalOrder>(responseBody);

    return View(paypalOrder);
  }

  [Route("/gateways/paypal/cancel")]
  public async Task<IActionResult> GateWayPaypalCancel([FromQuery(Name="token")] string token)
  {
    ViewBag.Token = token;

    return View();
  }


  // ########## PAYPAL ########## //
}