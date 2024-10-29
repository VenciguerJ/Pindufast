using Microsoft.AspNetCore.Mvc;
using PinduFast.Models;
using System.Diagnostics;
using PinduFast.Repositories;


namespace PinduFast.Controllers;

public class CarroController : Controller
{
    private readonly IRepository<Carro> _carroRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CarroController(IRepository<Carro> carroRepository, IWebHostEnvironment IHE)
    {
        _carroRepository = carroRepository;
        _webHostEnvironment = IHE;
    }
    public async Task<IActionResult> Index(string? searchString, int? IsActive)
    {
        try
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var resultado = await _carroRepository.BuscaComplete(searchString, IsActive);

                return View(resultado);
            }
            else
            {   searchString = "";
                if(IsActive == null)
                {
                    var resultado = await _carroRepository.GetAll();
                    return View(resultado);
                }
                else
                {
                    var resultado = await _carroRepository.BuscaComplete(searchString, IsActive);
                    return View(resultado);
                }

                
            }
        }
        catch (Exception ex)
        {
            // Tratamento de erro
            TempData["ErrorMessage"] = "Erro ao realizar operação, consulte o console de desenvolvedor.";
            Console.WriteLine(ex.ToString());

            // Caso de erro, retorna todos os carros
            var carros = await _carroRepository.GetAll();
            return View(carros);
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Carro c, IFormFile imagem)
    {
        try
        {
            if (imagem != null && imagem.Length > 0)
            {
                using var memorystream = new MemoryStream();
                await imagem.CopyToAsync(memorystream);
                c.Imagem = memorystream.ToArray();
            }
            else
            {
                string caminhoImagemPadrao = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "Default_img.jpg");
                c.Imagem = System.IO.File.ReadAllBytes(caminhoImagemPadrao);
            }
           await _carroRepository.Add(c);
            TempData["FineAlert"] = "Anuncio criado com sucesso!";
            
        }
        catch(Exception ex)
        {
            TempData["ErrorMessage"] = "Erro ao realizar operação, consulte o console de desenvolvedor";
            Console.WriteLine(ex.ToString());
        }
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Edit(int id)
    {
        Carro? carro = await _carroRepository.GetById(id);

        return View(carro);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Carro c, IFormFile imagem)
    {
        if (imagem != null && imagem.Length > 0)
        {
            using var memorystream = new MemoryStream();
            await imagem.CopyToAsync(memorystream);
            c.Imagem = memorystream.ToArray();
        }
        else
        {
            string caminhoImagemPadrao = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "Default_img.jpg");
            c.Imagem = System.IO.File.ReadAllBytes(caminhoImagemPadrao);
        }
        await _carroRepository.Update(c);
        TempData["FineMessage"] = "Cadastro atualizado!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        Carro? c = await _carroRepository.GetById(id);
        await _carroRepository.Delete(id);
         TempData["FineMessage"] = "Cadastro Excluido!";
        return RedirectToAction("Index");
    }

    public IActionResult Sobre()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
