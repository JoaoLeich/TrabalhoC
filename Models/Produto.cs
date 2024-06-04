using System.ComponentModel.DataAnnotations;


public class Produto
{

    [Key]
    public int id { get; set; }

    public String Nome { get; set; }

    public Double Preco { get; set; }

    public String Fornecedor { get; set; }
}