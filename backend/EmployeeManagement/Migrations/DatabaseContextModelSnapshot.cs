﻿// <auto-generated />
using EmployeeManagement.DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeManagement.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeManagement.Models.EmployeeRegisterModel", b =>
                {
                    b.Property<int>("Employeeid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Employeeid"));

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Occupation")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Employeeid");

                    b.ToTable("employee");
                });
#pragma warning restore 612, 618
        }
    }
}