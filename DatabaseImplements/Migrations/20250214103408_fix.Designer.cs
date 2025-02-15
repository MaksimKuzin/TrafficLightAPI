﻿// <auto-generated />
using DatabaseImplements.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatabaseImplements.Migrations
{
    [DbContext(typeof(TLDbContext))]
    [Migration("20250214103408_fix")]
    partial class fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DatabaseImplements.Models.Observation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Masks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PossibleNumbers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Segments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SequenceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SequenceId");

                    b.ToTable("Observations");
                });

            modelBuilder.Entity("DatabaseImplements.Models.Sequence", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Missing")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Start")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sequences");
                });

            modelBuilder.Entity("DatabaseImplements.Models.Observation", b =>
                {
                    b.HasOne("DatabaseImplements.Models.Sequence", "Sequence")
                        .WithMany("Observations")
                        .HasForeignKey("SequenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sequence");
                });

            modelBuilder.Entity("DatabaseImplements.Models.Sequence", b =>
                {
                    b.Navigation("Observations");
                });
#pragma warning restore 612, 618
        }
    }
}
