using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public ProdutosController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPreços()
        {
            return _uow.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet("primeiro")]
        [HttpGet("/primeiro")]
        public ActionResult<Produto> GetPrimeiro()
        {
            var produtos = _uow.ProdutoRepository.Get().FirstOrDefault();
            if (produtos is null)
            {
                return NotFound();
            }
            return produtos;
        }

        [HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> Get2(string valor)
        {
            var teste = valor;
            return _uow.ProdutoRepository.Get().FirstOrDefault();
        }


        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }

        [HttpGet()]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get2()
        {
            return _uow.ProdutoRepository.Get()
                .AsNoTracking().ToList();
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _uow.ProdutoRepository.Add(produto);
            _uow.Commit();

            return new CreatedAtRouteResult("ObterProduto", 
                new { id = produto.ProdutoId }, produto);
       }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _uow.ProdutoRepository.Update(produto);
            _uow.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uow.ProdutoRepository.GetById
                (p => p.ProdutoId == id);
            
            if(produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _uow.ProdutoRepository.Delete(produto);
            _uow.Commit();

            return Ok(produto);
        }
    }
}
