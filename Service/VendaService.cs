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

        var isCLienteExists = clienteService.GetClienteById(venda.cliente.id);

        var isProdutoExists = await prodService.GetProductByIdAsync(venda.produto.id);

        if (isCLienteExists == null || isProdutoExists == null)
        {

            //nao existe         
            return false;

        }

        await _dbContext.Venda.AddAsync(venda);
        return true;
    }

    public async Task<List<Venda>> ConsultarVendasProduto(int produtoid)
    {

        var Vendas = _dbContext.Venda.Where(v => v.produto.id == produtoid).ToList();

        return Vendas;

    }

    public async Task<vendaRetorno> ConsultarVendasProdutoSumarizada(int produtoid)
    {
        
        var Vendas = _dbContext.Venda.Where(v => v.produto.id == produtoid).ToList();

        string prodName = "";
        double totalVendas = 0;

        foreach (var item in Vendas)
        {
            
            totalVendas += item.precoUnit * item.quantVendida;
            prodName = item.produto.Nome;

        }

        var VendaRet = new vendaRetorno(Vendas.Count,totalVendas,prodName);

        return VendaRet;

    }



}