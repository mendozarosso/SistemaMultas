﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaMultas.Data;

#nullable disable

namespace SistemaMultas.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241208012246_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaMultas.Models.Concepto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<decimal>("MontoBase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Conceptos");
                });

            modelBuilder.Entity("SistemaMultas.Models.Multa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AgenteId")
                        .HasColumnType("int");

                    b.Property<string>("CedulaAgente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CedulaCiudadano")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConceptoId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("FotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitud")
                        .HasColumnType("float");

                    b.Property<double>("Longitud")
                        .HasColumnType("float");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NombreCiudadano")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AgenteId");

                    b.HasIndex("ConceptoId");

                    b.ToTable("Multas");
                });

            modelBuilder.Entity("SistemaMultas.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Cedula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Activo = true,
                            Cedula = "adamix",
                            Clave = "$2a$11$BmEL8/qVEeSR2i.H94BAs.TVpQkGA5Z/75sQ3CjiBZthcB0CmXD2G",
                            Nombre = "Administrador",
                            Tipo = "Admin"
                        });
                });

            modelBuilder.Entity("SistemaMultas.Models.Multa", b =>
                {
                    b.HasOne("SistemaMultas.Models.Usuario", "Agente")
                        .WithMany("Multas")
                        .HasForeignKey("AgenteId");

                    b.HasOne("SistemaMultas.Models.Concepto", "Concepto")
                        .WithMany("Multas")
                        .HasForeignKey("ConceptoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agente");

                    b.Navigation("Concepto");
                });

            modelBuilder.Entity("SistemaMultas.Models.Concepto", b =>
                {
                    b.Navigation("Multas");
                });

            modelBuilder.Entity("SistemaMultas.Models.Usuario", b =>
                {
                    b.Navigation("Multas");
                });
#pragma warning restore 612, 618
        }
    }
}
