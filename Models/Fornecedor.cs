using System.ComponentModel.DataAnnotations;

public class Fornecedor
{

    [Key]
    public int id { get; set; }

    public String CNPJ { get; set; }

    public String Nome { get; set; }

    public String Endereco { get; set; }

    public String Telefone { get; set; }

}