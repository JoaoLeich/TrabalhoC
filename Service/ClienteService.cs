using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


public class ClienteService
{
    private readonly LojaDBContext _dbContext;
    public ClienteService(LojaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Cliente>> GetAllClientesAsync()
    {
        return await _dbContext.Cliente.ToListAsync();
    }

    public async Task<Cliente> GetClienteById(int id)
    {
        return await _dbContext.Cliente.FindAsync(id);
    }

    public async Task<Cliente> GetClienteByName(String name)
    {

        return await _dbContext.Cliente.FindAsync(name);

    }

    // Método para gravar um novo produto
    public async Task AddClienteAsync(Cliente cliente)
    {
        _dbContext.Cliente.Add(cliente);
        await _dbContext.SaveChangesAsync();
    }
    // Método para atualizar os dados de um produto
    public async Task UpdateClienteAsync(Cliente cliente)
    {
        _dbContext.Cliente.Entry(cliente).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
    // Método para excluir um produto
    public async Task DeleteClienteAsync(int id)
    {
        var cliente = await _dbContext.Cliente.FindAsync(id);
        if (cliente != null)
        {
            _dbContext.Cliente.Remove(cliente);
            await _dbContext.SaveChangesAsync();
        }
    }
}
