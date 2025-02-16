using Microsoft.AspNetCore.Mvc;
using WatchLibrary;
using WatchLibrary.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTwatches.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchesController : ControllerBase
    {
        private readonly WatchRepository _watchRepository;
        public WatchesController(WatchRepository watchRepository)
        {
            _watchRepository = watchRepository;
        }
        // GET: api/<WatchesController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<IEnumerable<Watch>> Get()
        {

            var watches = _watchRepository.GetAll();
            if (watches != null && watches.Count() != 0)
            {
                return Ok(watches);
            }
            else
                return NotFound(watches);
        }
        // GET api/<WatchesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Watch> Get(int id)
        {
            Watch? watch = _watchRepository.GetById(id);
            if (watch != null)
            {
                return Ok(watch);
            }
            else return NotFound(watch);
        }
        // POST api/<WatchesController>


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Watch> Post([FromBody] Watch newWatch)
        {
            try
            {
                if (newWatch == null)
                {
                    return BadRequest("Watch cannot be null.");
                }

                newWatch.Validate(); // Hvis der er en validate-metode i Watch-klassen
                _watchRepository.Add(newWatch);

                return CreatedAtAction(nameof(Get), new { id = newWatch.Id }, newWatch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<WatchesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<Watch> Put(int id, [FromBody] Watch updatedWatch)
        {
            if (updatedWatch == null || id != updatedWatch.Id)
            {
                return BadRequest("Invalid data");
            }
            var existingWatch = _watchRepository.GetById(id);
            if (existingWatch == null)
            {
                return NotFound();
            }
            updatedWatch.Validate();
            _watchRepository.Update(id, updatedWatch);
            return Ok(updatedWatch);
        }

        // DELETE api/<WatchesController>/5
        [HttpDelete("{id}")]
        public ActionResult<Watch> Remove(int id)
        {
            var watch = _watchRepository.GetById(id);
            if (watch != null)
            {
                _watchRepository.Remove(id); // Assuming the correct method is Delete
                return Ok(watch);
            }
            else return NotFound();
        }

    }
}
