using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly IVillaService _villaService;

        public VillaApiController(IVillaService villaService)
        {
            _villaService = villaService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Villa>))]
        public ActionResult<IEnumerable<Villa>> GetVillas()
        {
            return Ok(_villaService.Get());
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Villa))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Villa> GetVilla(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                var villa = _villaService.Get(id);
                return Ok(villa);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Villa))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Villa> CreateVilla([FromBody] Villa villa)
        {
            if (_villaService.Get().Any(v => v.Name.ToLower() == villa.Name.ToLower()))
            {
                ModelState.AddModelError("", $"Villa {villa.Name} already exists");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(villa);
            }

            _villaService.Create(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                var villa = _villaService.Get(id);
                _villaService.Remove(villa);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(string id, [FromBody] Villa villa)
        {
            if (villa.Id != id)
            {
                return BadRequest();
            }

            try
            {
                var v = _villaService.Get(id);
                v.Name = villa.Name;
                v.Occupancy = villa.Occupancy;
                v.Sqft = villa.Sqft;
                _villaService.Update(v.Id, villa);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(string id, [FromBody] JsonPatchDocument<Villa> patchDoc)
        {
            try
            {
                var villa = _villaService.Get(id);
                patchDoc.ApplyTo(villa, ModelState);
                _villaService.Update(villa.Id, villa);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
            
        }
    }
}