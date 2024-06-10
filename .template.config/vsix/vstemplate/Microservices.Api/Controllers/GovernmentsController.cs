using Microservice.Domain;
using Microservice.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace $safeprojectname$
{
    //[Authorize]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Route("api/[controller]")]
    [ApiController]
    public class GovernmentsController : ControllerBase
    {
        private readonly AssociateDbContext _dbContext;

        public GovernmentsController(AssociateDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/Governments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Government>>> GetGovernment()
        {
            return await _dbContext.Government.ToListAsync();
        }

        // GET: api/Governments/5
        [HttpGet("{key}")]
        public async Task<ActionResult<Government>> GetGovernment(Guid key)
        {
            var government = await _dbContext.Government.FindAsync(key);

            if (government == null)
            {
                return NotFound();
            }

            return government;
        }

        // PUT: api/Governments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{key}")]
        public async Task<IActionResult> PutGovernment(Guid key, Government government)
        {
            if (key != government.GovernmentKey)
            {
                return BadRequest();
            }

            _dbContext.Entry(government).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GovernmentExists(key))
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

        // POST: api/Governments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Government>> PostGovernment(Government government)
        {
            _dbContext.Government.Add(government);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetGovernment", new { key = government.GovernmentKey }, government);
        }

        // DELETE: api/Governments/5
        [HttpDelete("{key}")]
        public async Task<ActionResult<Government>> DeleteGovernment(Guid key)
        {
            var government = await _dbContext.Government.FindAsync(key);
            if (government == null)
            {
                return NotFound();
            }

            _dbContext.Government.Remove(government);
            await _dbContext.SaveChangesAsync();

            return government;
        }

        private bool GovernmentExists(Guid key)
        {
            return _dbContext.Government.Any(e => e.GovernmentKey == key);
        }
    }
}
