using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper) 
        {
            _logger = logger;
            _db = db;
            _mapper=mapper;
        }
        [HttpGet]
        //implemetacion de la interfaz IActionResult, con ActionResult retorna un estado por ejemplo ok 
        public async Task<ActionResult<IEnumerable<VillaDTO> >>GetVillas()
        {
            _logger.LogInformation("todas las villas se estan obteniendo");
            IEnumerable<Villa> villa = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillaDTO>>(villa));
            
        }
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (id == 0)
            {
                _logger.LogError("Error al traer el id: " + id);
                return BadRequest();
            }
            else if(villa == null)
            {
                return NotFound();
            }
            return  Ok(_mapper.Map<VillaDTO>(villa));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<VillaDTO>> crearVilla([FromBody] VillaDTO villaDTO) 
        {
            if(villaDTO == null|| !ModelState.IsValid)
            {
                return BadRequest();
            }
            else if(villaDTO.Id>0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else if (_db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null) 
            {
                ModelState.AddModelError("Nombre ya existe", "La villa con este nombre ya existe");
                return BadRequest(ModelState);
            }
           
             Villa modelo= new Villa();
            modelo.Amenidad = villaDTO.Amenidad;
            modelo.Tarifa = villaDTO.Tarifa;
            modelo.Ocupantes = villaDTO.Ocupantes;
            modelo.MetrosCuadrados=villaDTO.MetrosCuadrados;
            modelo.FechaCreacion=villaDTO.FechaCreacion;
            modelo.FechaActualizacion = villaDTO.FechaActualizacion;
            modelo.Nombre=villaDTO.Nombre;
            modelo.Detalle=villaDTO.Detalle;
            modelo.ImagenUrl=villaDTO.ImagenUrl;
            await _db.Villas.AddAsync(modelo);
            _db.SaveChanges();
            return Ok(villaDTO);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id) 
        {
            var villa = await _db.Villas.FirstOrDefaultAsync(v=>v.Id==id);     
            if (id == 0) 
            {
                return BadRequest();
            }
            
            else if (villa == null)
            {
                return NotFound();
            }
            else 
            { 
                _db.Villas.Remove(villa);
               await  _db.SaveChangesAsync();
                return NoContent();
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDTO villaDTO) 
        { 
            if(villaDTO == null || id!=villaDTO.Id) 
            {
                return BadRequest();
            }
            Villa modelo= new Villa();
            modelo.Id = villaDTO.Id;
            modelo.Amenidad = villaDTO.Amenidad;
            modelo.Tarifa = villaDTO.Tarifa;
            modelo.Ocupantes = villaDTO.Ocupantes;
            modelo.MetrosCuadrados = villaDTO.MetrosCuadrados;
            modelo.FechaCreacion = villaDTO.FechaCreacion;
            modelo.FechaActualizacion = villaDTO.FechaActualizacion;
            modelo.Nombre = villaDTO.Nombre;
            modelo.Detalle = villaDTO.Detalle;
            modelo.ImagenUrl = villaDTO.ImagenUrl;
            _db.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> pathDTO)
        {
            if (pathDTO == null || id == 0)
            {
                return BadRequest();
            }
           // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
           //pathDTO.ApplyTo(villa,ModelState);
           var villa=await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v=>v.Id == id);
           VillaDTO villaAct= _mapper.Map<VillaDTO>(villa);
            if (villa == null)
            {
                return BadRequest(ModelState);
            }
            pathDTO.ApplyTo(villaAct,ModelState);
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            Villa modelo=_mapper.Map<Villa>(villaAct);
          
            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
   
}
