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
builder.Services.AddScoped<VendaService, VendaService>();

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

app.MapGet("/produtos", async (HttpContext context, ProductService productService) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var produtos = await productService.GetAllProductsAsync();
    return Results.Ok(produtos);

});


app.MapGet("/produtos/{id}", async (HttpContext context, int id, ProductService productService) =>
{
    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var produto = await productService.GetProductByIdAsync(id);
    if (produto == null)
    {
        return Results.NotFound($"Product with ID {id} not found.");
    }
    return Results.Ok(produto);



});

app.MapPost("/produtos", async (HttpContext context, Produto produto, ProductService productService) =>
{
    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    await productService.AddProductAsync(produto);
    return Results.Created($"/produtos/{produto.id}", produto);

});

app.MapPut("/produtos/{id}", async (HttpContext context, int id, Produto produto, ProductService productService) =>
{
    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    if (id != produto.id)
    {
        return Results.BadRequest("Product ID mismatch.");
    }
    await productService.UpdateProductAsync(produto);
    return Results.Ok();
});

app.MapDelete("/produtos/{id}", async (HttpContext context, int id, ProductService productService) =>
{
    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    await productService.DeleteProductAsync(id);
    return Results.Ok();
});

#endregion

#region Cliente

app.MapPost("/LoginCliente", async (ClienteService service, LoginModel loginUser) =>
{

    var isLogin = await service.Login(loginUser);

    if (isLogin)
    {

        var token = Token.GenerateToken();

        return Results.Accepted("token: " + token);
    }

    return Results.Unauthorized();

});


app.MapPost("/CreateCliente", async (HttpContext context, ClienteService service, Cliente cliente) =>
{
    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    await service.AddClienteAsync(cliente);
    return Results.Created($"createCliente/{cliente.id}", cliente);



});

app.MapGet("/Clientes", async (HttpContext context, ClienteService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var Clientes = await service.GetAllClientesAsync();

    return Results.Ok(Clientes);



});

app.MapGet("/cliente/{id}", async (HttpContext context, int id, ClienteService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var cliente = await service.GetClienteById(id);

    if (cliente == null)
    {

        return Results.NotFound($"Cliente with id: {id} not found");

    }

    return Results.Ok(cliente);


});

app.MapPut("/cliente/{id}", async (HttpContext context, ClienteService service, Cliente updateCliente) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

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


app.MapPost("/CreateFornecedor", async (HttpContext context, FornecedorService service, Fornecedor fornecedor) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    await service.AddFornecedorAsync(fornecedor);
    return Results.Created($"createFornecedor/{fornecedor.id}", fornecedor);

});

app.MapGet("/Fornecedores", async (HttpContext context, FornecedorService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var fornecedores = await service.GetFornecedorsAsync();

    return Results.Ok(fornecedores);

});

app.MapGet("/fornecedor/{id}", async (HttpContext context, int id, FornecedorService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var fornecedor = await service.GetFornecedorAsyncById(id);

    if (fornecedor == null)
    {

        return Results.NotFound($"Fornecedor with id: {id} not found");

    }

    return Results.Ok(fornecedor);

});

app.MapPut("/fornecedor/{id}", async (HttpContext context, FornecedorService service, Fornecedor updateFornecedor, int id) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

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


#region Vendas

app.MapPost("/RealizarVenda", async (HttpContext context, Venda venda, VendaService service) =>
{
    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var isVendeu = service.Vender(venda);

    return Results.Ok(isVendeu);

});

app.MapGet("/ConsultarVendasByProduto/{id}", async (HttpContext context, int id, VendaService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var Vendas = await service.ConsultarVendasProduto(id);

    return Results.Ok(Vendas);

});

app.MapGet("/ConsultarPorProdutosSumarizada/{id}", async (HttpContext context, int id, VendaService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var ret = await service.ConsultarVendasProdutoSumarizada(id);

    return Results.Ok(ret);

});

app.MapGet("/ConsultarPorCliente/{id}", async (HttpContext context, int id, VendaService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var ret = await service.ConsultarVendasCliente(id);

    return Results.Ok(ret);

});

app.MapGet("/ConsultarPorClienteSumarizada/{id}", async (HttpContext context, int id, VendaService service) =>
{

    var isToken = Token.IsTokenPresenteEValido(context);

    if (!isToken)
    {

        return Results.Unauthorized();

    }

    var ret = await service.ConsultarVendasClienteSumarizada(id);

    return Results.Ok(ret);

});



#endregion

app.Run();

