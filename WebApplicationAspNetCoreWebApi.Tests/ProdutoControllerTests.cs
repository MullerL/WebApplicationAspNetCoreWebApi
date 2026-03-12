using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAspNetCoreWebApi.Controllers;
using WebApplicationAspNetCoreWebApi.Models;

namespace WebApplicationAspNetCoreWebApi.Tests;

public class ProdutoControllerTests
{
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureDeleted(); // Para resetar estados se necessário
        databaseContext.Database.EnsureCreated();

        if (databaseContext.Produtos.Count() <= 0)
        {
            databaseContext.Produtos.Add(new Produto() { Id = 1, Nome = "Produto Teste 1", Preco = 10.50m });
            databaseContext.Produtos.Add(new Produto() { Id = 2, Nome = "Produto Teste 2", Preco = 20.00m });
            databaseContext.SaveChanges();
        }

        databaseContext.ChangeTracker.Clear(); // Simula um contexto HTTP novo (Desanexa entidades)

        return databaseContext;
    }

    [Fact]
    public async Task GetProdutos_DeveRetornarTodosOsProdutos()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);

        // Act
        var result = await controller.GetProdutos();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Produto>>>(result);
        var produtos = Assert.IsAssignableFrom<IEnumerable<Produto>>(actionResult.Value);
        Assert.Equal(2, produtos.Count());
    }

    [Fact]
    public async Task GetProduto_ComIdValido_DeveRetornarOProduto()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);

        // Act
        var result = await controller.GetProduto(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Produto>>(result);
        var produto = Assert.IsType<Produto>(actionResult.Value);
        Assert.Equal(1, produto.Id);
        Assert.Equal("Produto Teste 1", produto.Nome);
    }

    [Fact]
    public async Task GetProduto_ComIdInvalido_DeveRetornarNotFound()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);

        // Act
        var result = await controller.GetProduto(99);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostProduto_DeveCriarProdutoERetornarCreatedAtAction()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);
        var novoProduto = new Produto { Id = 3, Nome = "Novo Produto", Preco = 30.00m };

        // Act
        var result = await controller.PostProduto(novoProduto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Produto>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var produtoCriado = Assert.IsType<Produto>(createdAtActionResult.Value);
        
        Assert.Equal("GetProduto", createdAtActionResult.ActionName);
        Assert.Equal(3, produtoCriado.Id);
        Assert.Equal("Novo Produto", produtoCriado.Nome);
        Assert.Equal(3, dbContext.Produtos.Count());
    }

    [Fact]
    public async Task PutProduto_ComDadosValidos_DeveAtualizarOProduto()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);
        var produtoModificado = new Produto { Id = 1, Nome = "Produto Teste 1 Modificado", Preco = 15.00m };

        // Act
        var result = await controller.PutProduto(1, produtoModificado);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var produtoBanco = await dbContext.Produtos.FindAsync(1);
        Assert.NotNull(produtoBanco);
        Assert.Equal("Produto Teste 1 Modificado", produtoBanco.Nome);
        Assert.Equal(15.00m, produtoBanco.Preco);
        Assert.True(produtoBanco.Alterado > DateTime.MinValue); // Verifica se a data de alteração foi preenchida
    }

    [Fact]
    public async Task PutProduto_ComIdDiferente_DeveRetornarBadRequest()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);
        var produtoModificado = new Produto { Id = 2, Nome = "Produto Teste 2", Preco = 20.00m };

        // Act
        var result = await controller.PutProduto(1, produtoModificado); // ID url != ID objeto

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task DeleteProduto_ComIdValido_DeveRemoverOProduto()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);

        // Act
        var result = await controller.DeleteProduto(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, dbContext.Produtos.Count());
        Assert.Null(await dbContext.Produtos.FindAsync(1));
    }

    [Fact]
    public async Task DeleteProduto_ComIdInvalido_DeveRetornarNotFound()
    {
        // Arrange
        var dbContext = GetDatabaseContext();
        var controller = new ProdutoController(dbContext);

        // Act
        var result = await controller.DeleteProduto(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(2, dbContext.Produtos.Count()); // Não removeu nada
    }
}
