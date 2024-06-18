using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductService, ProductService>();
builder.Services.AddScoped<FornecedorService, FornecedorService>();
builder.Services.AddScoped<ClienteService, ClienteService>();
var con = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LojaDBContext>(op => op.UseMySql(con, new MySqlServerVersion(new Version(8, 3, 0))));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        //Opcoes de validacao do token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("testetestetestetestetestetestetestetestetestetestetestetestetesteteste"))
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

#region Produto
app.MapGet("/produtos", async (ProductService productService) =>
{
    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);
});

app.MapGet("/produtos/{id}", async (int id, ProductService productService) =>
{
    var produto = await productService.GetProductByIdAsync(id);
    if (produto == null)
    {
        return Results.NotFound($"Product with ID {id} not found.");
    }
    return Results.Ok(produto);
});

app.MapPost("/produtos", async (Produto produto, ProductService productService) =>
{
    await productService.AddProductAsync(produto);
    return Results.Created($"/produtos/{produto.id}", produto);
});

app.MapPut("/produtos/{id}", async (int id, Produto produto, ProductService productService) =>
{
    if (id != produto.id)
    {
        return Results.BadRequest("Product ID mismatch.");
    }
    await productService.UpdateProductAsync(produto);
    return Results.Ok();
});

app.MapDelete("/produtos/{id}", async (int id, ProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.Ok();
});

#endregion

#region Cliente

// app.MapPost("/CreateCliente", async (ClienteService service, Cliente cliente) =>
// {
//     service.AddClienteAsync(cliente);
//     return Results.Created($"createCliente/{cliente.id}", cliente);

// });

app.MapPost("/CreateCliente", async (ClienteService service, Cliente cliente) =>
{
    await service.AddClienteAsync(cliente);
    return Results.Created($"createCliente/{cliente.id}", cliente);

});

app.MapGet("/Clientes", async (ClienteService service) =>
{

    var Clientes = await service.GetAllClientesAsync();

    return Results.Ok(Clientes);

});

app.MapGet("/cliente/{id}", async (int id, ClienteService service) =>
{

    var cliente = await service.GetClienteById(id);

    if (cliente == null)
    {

        return Results.NotFound($"Cliente with id: {id} not found");

    }

    return Results.Ok(cliente);

});

app.MapPut("/cliente/{id}", async (ClienteService service, Cliente updateCliente) =>
{

    var cliente = await service.GetClienteById(updateCliente.id);

    if (cliente == null)
    {

        return Results.NotFound("Cliente Not Found");
    }

    cliente.Nome = updateCliente.Nome;
    cliente.Email = updateCliente.Email;
    cliente.CPF = updateCliente.CPF;

    await service.UpdateClienteAsync(cliente);

    return Results.Ok(updateCliente);

});


#endregion

#region Fornecedor


app.MapPost("/CreateFornecedor", async (FornecedorService service, Fornecedor fornecedor) =>
{

    await service.AddFornecedorAsync(fornecedor);
    return Results.Created($"createFornecedor/{fornecedor.id}", fornecedor);

});

app.MapGet("/Fornecedores", async (FornecedorService service) =>
{

    var fornecedores = await service.GetFornecedorsAsync();

    return Results.Ok(fornecedores);

});

app.MapGet("/fornecedor/{id}", async (int id, FornecedorService service) =>
{

    var fornecedor = await service.GetFornecedorAsyncById(id);

    if (fornecedor == null)
    {

        return Results.NotFound($"Fornecedor with id: {id} not found");

    }

    return Results.Ok(fornecedor);

});

app.MapPut("/fornecedor/{id}", async (FornecedorService service, Fornecedor updateFornecedor, int id) =>
{

    var fornecedor = await service.GetFornecedorAsyncById(id);

    if (fornecedor == null)
    {

        return Results.NotFound("Fornecedor Not Found");
    }

    fornecedor.Telefone = updateFornecedor.Telefone;
    fornecedor.Nome = updateFornecedor.Nome;
    fornecedor.CNPJ = updateFornecedor.CNPJ;
    fornecedor.Endereco = updateFornecedor.Endereco;

    service.UpdateFornecedorAsync(updateFornecedor);

    return Results.Ok(updateFornecedor);

});


#endregion

app.Run();

