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
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db) 
        {
            _logger = logger;
            _db = db;
        }
        [HttpGet]
        //implemetacion de la interfaz IActionResult, con ActionResult retorna un estado por ejemplo ok 
        public ActionResult<IEnumerable<VillaDTO> >GetVillas()
        {
            _logger.LogInformation("todas las villas se estan obteniendo");
            return Ok(_db.Villas.ToList());
            
        }
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (id == 0)
            {
                _logger.LogError("Error al traer el id: " + id);
                return BadRequest();
            }
            else if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> crearVilla([FromBody] VillaDTO villaDTO) 
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
            _db.Villas.Add(modelo);
            _db.SaveChanges();
            return Ok(villaDTO);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id) 
        {
            var villa = _db.Villas.FirstOrDefault(v=>v.Id==id);     
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
                _db.SaveChanges();
                return NoContent();
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO) 
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
            _db.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> pathDTO)
        {
            if (pathDTO == null || id == 0)
            {
                return BadRequest();
            }
           // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
           //pathDTO.ApplyTo(villa,ModelState);
           var villa=_db.Villas.AsNoTracking().FirstOrDefault(v=>v.Id == id);
            VillaDTO villaAct = new()
            {
                Id=villa.Id,
                Nombre=villa.Nombre,
                Detalle=villa.Detalle,
                ImagenUrl=villa.ImagenUrl,
                Ocupantes=villa.Ocupantes,
                Tarifa=villa.Tarifa,
                MetrosCuadrados=villa.MetrosCuadrados,
                Amenidad=villa.Amenidad,
                FechaActualizacion=villa.FechaActualizacion,
                FechaCreacion=villa.FechaCreacion,
            };
            pathDTO.ApplyTo(villaAct,ModelState);
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            Villa modelo = new()
            {
                Id = villaAct.Id,
                Nombre = villaAct.Nombre,
                Detalle = villaAct.Detalle,
                ImagenUrl = villaAct.ImagenUrl,
                Ocupantes = villaAct.Ocupantes,
                Tarifa = villaAct.Tarifa,
                MetrosCuadrados = villaAct.MetrosCuadrados,
                Amenidad = villaAct.Amenidad,
                FechaActualizacion = villaAct.FechaActualizacion,
                FechaCreacion = villaAct.FechaCreacion,
            };
            _db.Villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();
        }
    }
   
}
