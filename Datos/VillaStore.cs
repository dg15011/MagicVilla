﻿using MagicVilla_API.Models.DTO;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO {Id=1,Nombre="Vista a la piscina",Ocupantes=3,MetrosCuadrados=50},
            new VillaDTO {Id=2,Nombre="Vista a la playa",Ocupantes=4,MetrosCuadrados=50}
        };
    }
}
