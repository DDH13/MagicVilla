using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/RichGuyAPI")]
[ApiController]
public class RichGuyAPIController : ControllerBase
{
    private readonly IRichGuyService _rgService;

    public RichGuyAPIController(IRichGuyService rgService)
    {
        _rgService = rgService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RichGuy>))]
    public ActionResult<IEnumerable<RichGuy>> GetRichGuys()
    {
        return Ok(_rgService.Get());
    }
    
    [HttpGet("{id}", Name = "GetRichGuy")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RichGuy))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RichGuy> GetRichGuy(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var richGuy = _rgService.Get(id);
            return Ok(richGuy);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RichGuy))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<RichGuy> CreateRichGuy([FromBody] RichGuy richGuy)
    {
        if (_rgService.Get().Any(v => v.Name.ToLower() == richGuy.Name.ToLower()))
        {
            ModelState.AddModelError("", $"RichGuy {richGuy.Name} already exists");
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _rgService.Create(richGuy);
        return CreatedAtRoute("GetRichGuy", new {id = richGuy.Id}, richGuy);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateRichGuy(string id, [FromBody] RichGuy richGuy)
    {
        if (richGuy == null)
        {
            return BadRequest();
        }

        if (richGuy.Id != id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _rgService.Update(id, richGuy);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteRichGuy(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        try
        {
            var richGuy = _rgService.Get(id);
            if (richGuy == null)
            {
                return NotFound();
            }

            _rgService.Remove(richGuy);
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public IActionResult PartiallyUpdateRichGuy(string id, [FromBody] JsonPatchDocument<RichGuy> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        try
        {
            var richGuy = _rgService.Get(id);
            if (richGuy == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(richGuy, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _rgService.Update(id, richGuy);
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    

    
}