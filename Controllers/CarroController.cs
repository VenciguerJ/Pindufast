using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinduFast.Data;
using PinduFast.Models;
using System.Diagnostics;
using System.IO;
using static System.Net.Mime.MediaTypeNames;


namespace PinduFast.Controllers;

public class CarroController : Controller
{
    private readonly ILogger<CarroController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CarroController(ILogger<CarroController> logger, AppDbContext dbc, IWebHostEnvironment IHE)
    {
        _context = dbc;
        _logger = logger;
        _webHostEnvironment = IHE;
    }

    public async Task<IActionResult> Index(string searchString, int? IsActive)
    {
        int firstReload = 0;

        // Se o valor não foi definido, defina-o como true ou false (valor padrão)
        int isActiveValue = IsActive ?? 1; // Definir como `true` por padrão, você pode mudar para false se preferir.

        try
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                // Query com busca por string e status
                var resultado = await _context.Carro
                    .FromSqlInterpolated($"SELECT * FROM Carro WHERE Nome LIKE '%' + {searchString} + '%' AND Ativo = {isActiveValue}")
                    .ToListAsync();

                return View(resultado);
            }
            else
            {
                // Query sem busca, apenas por status
                var resultado = await _context.Carro
                    .FromSqlInterpolated($"SELECT * FROM Carro WHERE Ativo = {isActiveValue}")
                    .ToListAsync();

                return View(resultado);
            }
        }
        catch (Exception ex)
        {
            // Tratamento de erro
            TempData["ErrorMessage"] = "Erro ao realizar operação, consulte o console de desenvolvedor.";
            Console.WriteLine(ex.ToString());

            // Caso de erro, retorna todos os carros
            var carros = await _context.Carro.ToListAsync();
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
            _context.Carro.Add(c); // Adiciona o novo produto
            _context.SaveChanges(); // Salva as alterações no banco
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
        Carro carro = await _context.Carro.FindAsync(id);

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
        _context.Carro.Update(c);
        _context.SaveChanges();
        TempData["FineMessage"] = "Cadastro atualizado!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        Carro c = await _context.Carro.FindAsync(id);
        _context.Carro.Remove(c);
        _context.SaveChanges();
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
