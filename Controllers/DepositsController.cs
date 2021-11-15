using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeApi.Models;

namespace HomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositsController : ControllerBase
    {
        private readonly TodoContext _context;

        public DepositsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Deposits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deposit>>> GetDeposits()
        {
            return await _context.Deposits.ToListAsync();
        }

        // GET: api/deposits/Total
        // outputs the total amount accumulated on the account.
        [HttpGet("/api/[controller]/total")]
        public async Task<IActionResult> GetTotal()
        {
            double total = 0;
            var deposits = await _context.Deposits.ToListAsync();
            if (deposits.Count() != 0)
            {
                foreach (var deposit in deposits)
                {
                    total += deposit.Amount;
                }
            }
            return Ok(total);
        }

        // GET: api/Deposits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Deposit>> GetDeposit(long id)
        {
            var deposit = await _context.Deposits.FindAsync(id);

            if (deposit == null)
            {
                return NotFound();
            }

            return deposit;
        }

        // PUT: api/Deposits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeposit(long id, Deposit deposit)
        {
            if (id != deposit.Id)
            {
                return BadRequest();
            }

            _context.Entry(deposit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Deposits
        [HttpPost]
        public async Task<ActionResult<Deposit>> PostDeposit(Deposit deposit)
        {
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetDeposit", new { id = deposit.Id }, deposit);
            return CreatedAtAction(nameof(GetDeposit), new { id = deposit.Id }, deposit);
        }

        // DELETE: api/Deposits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposit(long id)
        {
            var deposit = await _context.Deposits.FindAsync(id);
            if (deposit == null)
            {
                return NotFound();
            }

            _context.Deposits.Remove(deposit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepositExists(long id)
        {
            return _context.Deposits.Any(e => e.Id == id);
        }
    }
}
