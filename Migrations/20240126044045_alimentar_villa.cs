﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class alimentar_villa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle", new DateTime(2024, 1, 25, 22, 40, 43, 925, DateTimeKind.Local).AddTicks(7100), new DateTime(2024, 1, 25, 22, 40, 43, 925, DateTimeKind.Local).AddTicks(7090), "img.jpg", 50.0, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle", new DateTime(2024, 1, 25, 22, 40, 43, 925, DateTimeKind.Local).AddTicks(7103), new DateTime(2024, 1, 25, 22, 40, 43, 925, DateTimeKind.Local).AddTicks(7102), "img.jpg", 40.0, "Premiun vista piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
