using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compras.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carrito",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false, defaultValue: 65)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrito", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "descuento_temporada",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    porcentaje = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    fecha_desde = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_hasta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false, defaultValue: 65)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_descuento_temporada", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "carrito_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    carrito_id = table.Column<Guid>(type: "uuid", nullable: false),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo_producto = table.Column<string>(type: "text", nullable: false),
                    nombre_producto = table.Column<string>(type: "text", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false, defaultValue: 65)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrito_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_carrito_item_carrito_carrito_id",
                        column: x => x.carrito_id,
                        principalTable: "carrito",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orden",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estado_orden = table.Column<int>(type: "integer", nullable: false),
                    total = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    descuento_temporada = table.Column<Guid>(type: "uuid", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false, defaultValue: 65)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orden", x => x.id);
                    table.ForeignKey(
                        name: "FK_orden_descuento_temporada_descuento_temporada",
                        column: x => x.descuento_temporada,
                        principalTable: "descuento_temporada",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orden_detalle",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    orden_id = table.Column<Guid>(type: "uuid", nullable: false),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo_producto = table.Column<string>(type: "text", nullable: false),
                    nombre_producto = table.Column<string>(type: "text", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "numeric(13,4)", precision: 13, scale: 4, nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_eliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false, defaultValue: 65)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orden_detalle", x => x.id);
                    table.ForeignKey(
                        name: "FK_orden_detalle_orden_orden_id",
                        column: x => x.orden_id,
                        principalTable: "orden",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_carrito_usuario_id",
                table: "carrito",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_carrito_item_carrito_id",
                table: "carrito_item",
                column: "carrito_id");

            migrationBuilder.CreateIndex(
                name: "IX_orden_descuento_temporada",
                table: "orden",
                column: "descuento_temporada");

            migrationBuilder.CreateIndex(
                name: "IX_orden_usuario_id",
                table: "orden",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_orden_detalle_orden_id",
                table: "orden_detalle",
                column: "orden_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carrito_item");

            migrationBuilder.DropTable(
                name: "orden_detalle");

            migrationBuilder.DropTable(
                name: "carrito");

            migrationBuilder.DropTable(
                name: "orden");

            migrationBuilder.DropTable(
                name: "descuento_temporada");
        }
    }
}
