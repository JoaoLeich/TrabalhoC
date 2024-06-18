using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


public class FornecedorService
{
    private readonly LojaDBContext _dbContext;
    public FornecedorService(LojaDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Fornecedor>> GetFornecedorsAsync()
    {
        return await _dbContext.Fornecedor.ToListAsync();
    }

    public async Task<Fornecedor> GetFornecedorAsyncById(int id)
    {
        return await _dbContext.Fornecedor.FindAsync(id);
    }
    // Método para gravar um novo produto
    public async Task AddFornecedorAsync(Fornecedor fornecedor)
    {
        _dbContext.Fornecedor.Add(fornecedor);
        await _dbContext.SaveChangesAsync();
    }
    // Método para atualizar os dados de um produto
    public async Task UpdateFornecedorAsync(Fornecedor fornecedor)
    {
        _dbContext.Entry(fornecedor).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
    // Método para excluir um produto
    public async Task DeleteFornecedorAsync(int id)
    {
        var produto = await _dbContext.Fornecedor.FindAsync(id);
        if (produto != null)
        {
            _dbContext.Fornecedor.Remove(produto);
            await _dbContext.SaveChangesAsync();
        }
    }
}
