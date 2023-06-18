using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {

        public VillaApiController()
        {

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Villa>))]
        public ActionResult<IEnumerable<Villa>> GetVillas()
        {
            return Ok(VillaStore.villaList);
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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Villa))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Villa> CreateVilla([FromBody] Villa villa)
        {
            if (VillaStore.villaList.Any(v => v.Name.ToLower() == villa.Name.ToLower()))
            {
                ModelState.AddModelError("", $"Villa {villa.Name} already exists");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(villa);
            }

            if (string.IsNullOrEmpty(villa.Id))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            VillaStore.villaList.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);
            return NoContent();
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

            var v = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (v == null)
            {
                return NotFound();
            }

            v.Name = villa.Name;
            v.Occupancy = villa.Occupancy;
            v.Sqft = villa.Sqft;
            return NoContent();
        }

        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(string id, [FromBody] JsonPatchDocument<Villa> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}