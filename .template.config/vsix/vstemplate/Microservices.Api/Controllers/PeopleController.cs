using Microservice.Persistence;
using Microservice.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    //[Authorize]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AssociateDbContext _dbContext;

        public PeopleController(AssociateDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPerson()
        {
            return await _dbContext.Person.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{key}")]
        public async Task<ActionResult<Person>> GetPerson(Guid key)
        {
            var person = await _dbContext.Person.FindAsync(key);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{key}")]
        public async Task<IActionResult> PutPerson(Guid key, Person person)
        {
            if (key != person.PersonKey)
            {
                return BadRequest();
            }

            _dbContext.Entry(person).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(key))
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

        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _dbContext.Person.Add(person);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { key = person.PersonKey }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{key}")]
        public async Task<ActionResult<Person>> DeletePerson(Guid key)
        {
            var person = await _dbContext.Person.FindAsync(key);
            if (person == null)
            {
                return NotFound();
            }

            _dbContext.Person.Remove(person);
            await _dbContext.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(Guid key)
        {
            return _dbContext.Person.Any(e => e.PersonKey == key);
        }
    }
}
