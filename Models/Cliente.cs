using System.ComponentModel.DataAnnotations;

public class Cliente
{

    [Key]
    public int id { get; set; }

    public String Nome { get; set; }

    public String CPF { get; set; }

    public String Email { get; set; }

}