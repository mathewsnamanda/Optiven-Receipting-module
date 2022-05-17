﻿// <auto-generated />
using System;
using ClientStatements.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(OptivenContext))]
    partial class OptivenContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication1.Models.SaveReceipt", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Paymode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlotNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receiptno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("accno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bcopy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("chequenumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("client")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("copy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("paymentfor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("project")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("receivedby")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Receipts");
                });
#pragma warning restore 612, 618
        }
    }
}
