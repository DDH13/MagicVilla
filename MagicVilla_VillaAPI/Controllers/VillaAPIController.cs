using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaAPIController(ILogging logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VillaDTO>))]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting list of villas","");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("Villa Id cannot be 0","error");
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaStore.villaList.Any(v => v.Name.ToLower() == villa.Name.ToLower()))
            {
                ModelState.AddModelError("", $"Villa {villa.Name} already exists");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (villa == null)
            {
                return BadRequest(villa);
            }

            if (villa.Id < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
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

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest(villaDTO);
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> patchDoc)
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