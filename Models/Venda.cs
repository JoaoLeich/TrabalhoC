using System.ComponentModel.DataAnnotations;

public class Venda
{

    [Key]
    public int ID { get; set; }

    public DateTime DataCompra { get; set; }

    public String numeroNotaFiscal { get; set; }

    public Produto produto { get; set; }

    public Cliente cliente { get; set; }

    public int quantVendida { get; set; }

    public double precoUnit { get; set; }

}