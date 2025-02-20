using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Controllers;

public class ApiClientController : Controller
{

    public string _token;
    public  ApiClientController()
    {
      // This token was getted whit postman with the necessary credentials
      _token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6MzYsImlhdCI6MTczOTk5ODAzNCwiZXhwIjoxNzQyNTkwMDM0fQ.Fm-oFGvjbfZfVJnpd_JUoxEOxTtX997aCbwob7Tie7Y";
    }

    [Route("/api-client")]
    public async Task<IActionResult> Index()
    {
      HttpClient client = new();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
      var result = await client.GetAsync("https://www.api.tamila.cl/api/categorias");
      var responseBody = await result.Content.ReadAsStringAsync();
      List<Categoria> categories = JsonConvert.DeserializeObject<List<Categoria>>(responseBody);


      Console.WriteLine($"HTTP Status is: {result.StatusCode}");
      // Console.WriteLine(responseBody);
      return View(categories);
    }

    [Route("/api-client/add")]
    public IActionResult Add()
    {
      return View();
    }

    [HttpPost]
    [Route("/api-client/add")]
    public async Task<IActionResult> Add(Categoria model)
    {
      if (ModelState.IsValid)
      {
        HttpClient client = new();
        var data = new { nombre = model.Nombre };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        var result = await client.PostAsJsonAsync("https://www.api.tamila.cl/api/categorias", data);

        return RedirectToAction(nameof(Add));
      }

      return View();
    }

    [Route("/api-client/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
      HttpClient client = new();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
      var result = await client.GetAsync($"https://www.api.tamila.cl/api/categorias/{id}");
      string responseBody = await result.Content.ReadAsStringAsync();
      Categoria category = JsonConvert.DeserializeObject<Categoria>(responseBody);

      if (category == null) return NotFound();

      return View(category);
    }

    [HttpPost]
    [Route("/api-client/edit/{id}")]
    public async Task<IActionResult> Edit(Categoria model)
    {
      HttpClient client = new();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

      if (ModelState.IsValid)
      {
        var data = new { nombre = model.Nombre };
        var result = await client.PutAsJsonAsync("https://www.api.tamila.cl/api/categorias/" + model.Id, data);

        return RedirectToAction(nameof(Index));
      }

      var result2 = await client.GetAsync("https://www.api.tamila.cl/api/categorias/" + model.Id );
      string responseBody = await result2.Content.ReadAsStringAsync();
      Categoria category = JsonConvert.DeserializeObject<Categoria>(responseBody);

      return View();
    }

        [Route("/api-client/delete/{id}")]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      HttpClient client = new();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
      var delete = await client.DeleteAsync("https://www.api.tamila.cl/api/categorias/" + id);

      return RedirectToAction(nameof(Index));
    }
}