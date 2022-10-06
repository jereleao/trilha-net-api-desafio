using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the item with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A item</returns>
        /// <response code="200">Returns a list</response>
        /// <response code="404">If the item is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null) return NotFound();

            return Ok(tarefa);
        }

        /// <summary>
        /// Get all itens
        /// </summary>
        /// <returns>A list with the itens</returns>
        /// <response code="200">Returns a list</response>
        [HttpGet("ObterTodos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var tarefas = await _context.Tarefas.ToListAsync();

            return Ok(tarefas);
        }

        /// <summary>
        /// Get all itens with the given title
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns>A list with the filtered itens</returns>
        /// <response code="200">Returns a list</response>
        [HttpGet("ObterPorTitulo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var tarefa = await _context.Tarefas.Where(t => t.Titulo == titulo).ToListAsync();
            return Ok(tarefa);
        }

        /// <summary>
        /// Get all itens with the given date
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A list with the filtered itens</returns>
        /// <response code="200">Returns a list</response>
        [HttpGet("ObterPorData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefa = await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();
            return Ok(tarefa);
        }

        /// <summary>
        /// Get all itens with the given status
        /// </summary>
        /// <param name="status"></param>
        /// <returns>A list with the filtered itens</returns>
        /// <response code="200">Returns a list</response>
        [HttpGet("ObterPorStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = await _context.Tarefas.Where(t => t.Status == status).ToListAsync();
            return Ok(tarefa);
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns>A newly created item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is empty</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            await _context.AddAsync(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        /// <summary>
        /// Update a item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        /// <response code="200">If the item is updated</response>
        /// <response code="400">If the item is empty</response>
        /// <response code="404">If the item is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco = tarefa;
            await _context.AddAsync(tarefaBanco);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Delete a item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If the item is deleted</response>
        /// <response code="404">If the item is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Remove(id);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
