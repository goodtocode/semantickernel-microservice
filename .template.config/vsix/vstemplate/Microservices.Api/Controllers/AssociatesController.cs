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
    public class AssociatesController : ControllerBase
    {
        private readonly AssociateDbContext _dbContext;

        public AssociatesController(AssociateDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/Associates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Associate>>> GetAssociate()
        {
            return await _dbContext.Associate.ToListAsync();
        }

        // GET: api/Associates/5
        [HttpGet("{key}")]
        public async Task<ActionResult<Associate>> GetAssociate(Guid key)
        {
            var entity = await _dbContext.Associate.FindAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        // PUT: api/Associates/5
        [HttpPut("{key}")]
        public async Task<IActionResult> Putassociate(Guid key, Associate entity)
        {
            if (key != entity.AssociateKey)
            {
                return BadRequest();
            }

            _dbContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
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

        // POST: api/Associates
        [HttpPost]
        public async Task<ActionResult<Associate>> PostAssociate(Associate entity)
        {
            _dbContext.Associate.Add(entity);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetEntity", new { key = entity.AssociateKey }, entity);
        }

        // DELETE: api/Associates/5
        [HttpDelete("{key}")]
        public async Task<ActionResult<Associate>> DeleteEntity(Guid key)
        {
            var entity = await _dbContext.Associate.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            _dbContext.Associate.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        private bool EntityExists(Guid key)
        {
            return _dbContext.Associate.Any(e => e.AssociateKey == key);
        }
    }
}
