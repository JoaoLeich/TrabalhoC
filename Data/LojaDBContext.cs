using Microsoft.EntityFrameworkCore;

public class LojaDBContext : DbContext
{
    public LojaDBContext(DbContextOptions<LojaDBContext> options) : base(options)
    {
    }


    public DbSet<Produto> Produtos { get; set; }

    public DbSet<Cliente> Cliente { get; set; }

    public DbSet<Fornecedor> Fornecedor { get; set; }

    public DbSet<Venda> Venda { get; set; }



}