using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PinduFast.Models;

public class Carro
{
    public int Id { get; set; }

    [MaxLength(7)]
    public string Placa {  get; set; }

    [MaxLength(50)]
	public string Nome {  get; set; }

    [Required(ErrorMessage ="Campo obrigatório")]
    public int Portas {  get; set; }

    [Required(ErrorMessage = "Campo obrigatório")]
    public bool Ativo {  get; set; }
    
    [Required(ErrorMessage = "Campo obrigatório")]
    public decimal Preco { get; set; }

    public byte[]? Imagem { get; set; }

    [Required(ErrorMessage = "Campo obrigatório")]
    public DateTime DataPublicacao {  get; set; }
}
