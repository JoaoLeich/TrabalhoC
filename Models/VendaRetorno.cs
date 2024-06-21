public class vendaRetorno
{
    public vendaRetorno()
    {
    }

    public vendaRetorno(int quant, double preco, string nome)
    {

        this.totalPreco = preco;
        this.quantVendida = quant;
        this.NomeProduto = nome;

    }

    public string NomeProduto { get; set; }

    public int quantVendida { get; set; }

    public double totalPreco { get; set; }



}