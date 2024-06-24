using Microsoft.EntityFrameworkCore;

public class VendaService
{

    private readonly ClienteService clienteService;
    private readonly ProductService prodService;
    private readonly LojaDBContext _dbContext;
    public VendaService(LojaDBContext dbContext, ClienteService ClienteServ, ProductService productService)
    {
        _dbContext = dbContext;
        clienteService = ClienteServ;
        prodService = productService;
    }
    public async Task<Boolean> Vender(Venda venda)
    {

        var isCLienteExists = _dbContext.Cliente.Find(venda.cliente.id);

        var isProdutoExists = _dbContext.Produtos.Find(venda.produto.id);

        if (isCLienteExists == null || isProdutoExists == null)
        {


            return false;

        }

        venda.cliente = isCLienteExists;
        venda.produto = isProdutoExists;

        venda.DataCompra = DateTime.UtcNow;
        venda.precoUnit = venda.produto.Preco;

        _dbContext.Venda.Add(venda);
        _dbContext.SaveChanges();
        return true;
    }

    public async Task<List<Venda>> ConsultarVendasProduto(int produtoid)
    {

        var Vendas = _dbContext.Venda.
            Include(e => e.cliente)
            .Include(e => e.produto)
            .Where(e => e.produto.id == produtoid)
            .ToList();

        return Vendas;

    }

    public async Task<vendaRetorno> ConsultarVendasProdutoSumarizada(int produtoid)
    {

        var Vendas = _dbContext.Venda.
            Include(e => e.cliente)
            .Include(e => e.produto)
            .Where(e => e.produto.id == produtoid)
            .ToList();

        string prodName = "";
        double totalVendas = 0;

        foreach (var item in Vendas)
        {

            totalVendas += item.precoUnit * item.quantVendida;
            prodName = item.produto.Nome;

        }

        var VendaRet = new vendaRetorno(Vendas.Count, totalVendas, prodName);

        return VendaRet;

    }

    public async Task<vendaRetorno> ConsultarVendasClienteSumarizada(int clienteID)
    {

        var Vendas = _dbContext.Venda.
            Include(e => e.cliente)
            .Include(e => e.produto)
            .Where(e => e.cliente.id == clienteID)
            .ToList();

        string prodName = "";
        double totalVendas = 0;

        foreach (var item in Vendas)
        {

            totalVendas += item.precoUnit * item.quantVendida;
            prodName = item.produto.Nome;

        }

        var VendaRet = new vendaRetorno(Vendas.Count, totalVendas, prodName);

        return VendaRet;

    }

    public async Task<List<Venda>> ConsultarVendasCliente(int clienteID)
    {

        var Vendas = _dbContext.Venda.
            Include(e => e.cliente)
            .Include(e => e.produto)
            .Where(e => e.cliente.id == clienteID)
            .ToList();

        return Vendas;

    }




}