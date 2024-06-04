using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var con = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LojaDBContext>(op => op.UseMySql(con, new MySqlServerVersion(new Version(8, 3, 0))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Produto
app.MapPost("/AddProduto", async (LojaDBContext DbContext, Produto produto) =>
{

    DbContext.Produtos.Add(produto);
    await DbContext.SaveChangesAsync();
    return Results.Created($"createdproduto/{produto.id}", produto);

});

app.MapGet("/Produtos", async (LojaDBContext DbContext) =>
{

    var Produtos = await DbContext.Produtos.ToListAsync();

    return Results.Ok(Produtos);

});

app.MapGet("/produtos/{id}", async (int id, LojaDBContext DbContext) =>
{

    var Produto = await DbContext.Produtos.FindAsync(id);

    if (Produto == null)
    {

        return Results.NotFound($"Produto with id: {id} not found");

    }

    return Results.Ok(Produto);

});

app.MapPut("/produtos/{id}", async (LojaDBContext DbContext, Produto updateProduto) =>
{

    var Produto = await DbContext.Produtos.FindAsync(updateProduto.id);

    if (Produto == null)
    {

        return Results.NotFound("Produto Not Found");
    }

    Produto.Fornecedor = updateProduto.Fornecedor;
    Produto.Preco = updateProduto.Preco;
    Produto.Fornecedor = updateProduto.Fornecedor;

    await DbContext.SaveChangesAsync();

    return Results.Ok(Produto);

});

#endregion

#region Cliente

app.MapPost("/CreateCliente", async (LojaDBContext DbContext, Cliente cliente) =>
{

    DbContext.Cliente.Add(cliente);
    await DbContext.SaveChangesAsync();
    return Results.Created($"createCliente/{cliente.id}", cliente);

});

app.MapGet("/Clientes", async (LojaDBContext DbContext) =>
{

    var Clientes = await DbContext.Cliente.ToListAsync();

    return Results.Ok(Clientes);

});

app.MapGet("/cliente/{id}", async (int id, LojaDBContext DbContext) =>
{

    var cliente = await DbContext.Cliente.FindAsync(id);

    if (cliente == null)
    {

        return Results.NotFound($"Cliente with id: {id} not found");

    }

    return Results.Ok(cliente);

});

app.MapPut("/cliente/{id}", async (LojaDBContext DbContext, Cliente updateCliente) =>
{

    var cliente = await DbContext.Cliente.FindAsync(updateCliente.id);

    if (cliente == null)
    {

        return Results.NotFound("Cliente Not Found");
    }

    cliente.Nome = updateCliente.Nome;
    cliente.Email = updateCliente.Email;
    cliente.CPF = updateCliente.CPF;

    await DbContext.SaveChangesAsync();

    return Results.Ok(updateCliente);

});


#endregion

#region Fornecedor


app.MapPost("/CreateFornecedor", async (LojaDBContext DbContext, Fornecedor fornecedor) =>
{

    DbContext.Fornecedor.Add(fornecedor);
    await DbContext.SaveChangesAsync();
    return Results.Created($"createFornecedor/{fornecedor.id}", fornecedor);

});

app.MapGet("/Fornecedores", async (LojaDBContext DbContext) =>
{

    var fornecedores = await DbContext.Fornecedor.ToListAsync();

    return Results.Ok(fornecedores);

});

app.MapGet("/fornecedor/{id}", async (int id, LojaDBContext DbContext) =>
{

    var fornecedor = await DbContext.Fornecedor.FindAsync(id);

    if (fornecedor == null)
    {

        return Results.NotFound($"Fornecedor with id: {id} not found");

    }

    return Results.Ok(fornecedor);

});

app.MapPut("/fornecedor/{id}", async (LojaDBContext DbContext, Fornecedor updateFornecedor) =>
{

    var fornecedor = await DbContext.Fornecedor.FindAsync(updateFornecedor.id);

    if (fornecedor == null)
    {

        return Results.NotFound("Fornecedor Not Found");
    }

    fornecedor.Telefone = updateFornecedor.Telefone;
    fornecedor.Nome = updateFornecedor.Nome;
    fornecedor.CNPJ = updateFornecedor.CNPJ;
    fornecedor.Endereco = updateFornecedor.Endereco;

    await DbContext.SaveChangesAsync();

    return Results.Ok(updateFornecedor);

});


#endregion

app.Run();