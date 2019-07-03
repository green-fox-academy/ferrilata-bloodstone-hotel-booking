﻿// <auto-generated />
using System;
using HotelBookingApp;
using HotelBookingApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelBookingApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20190702100055_seedData1")]
    partial class seedData1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.Bed", b =>
                {
                    b.Property<int>("BedId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Size");

                    b.Property<string>("Type");

                    b.HasKey("BedId");

                    b.ToTable("Beds");
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.Hotel", b =>
                {
                    b.Property<int>("HotelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("LocationId");

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.Property<int>("PropertyTypeId");

                    b.Property<int>("StarRating");

                    b.HasKey("HotelId");

                    b.HasIndex("LocationId")
                        .IsUnique();

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new { HotelId = 1, Description = "description1", LocationId = 1, Name = "hotel1", Price = 1, PropertyTypeId = 1, StarRating = 1 },
                        new { HotelId = 2, Description = "description2", LocationId = 2, Name = "hotel2", Price = 2, PropertyTypeId = 2, StarRating = 1 },
                        new { HotelId = 3, Description = "description3", LocationId = 3, Name = "hotel3", Price = 3, PropertyTypeId = 1, StarRating = 1 }
                    );
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Region");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");

                    b.HasData(
                        new { LocationId = 1, Address = "a street 1", City = "Moon", Country = "Moon", Region = "Moon District?" },
                        new { LocationId = 2, Address = "a street mars", City = "Mars", Country = "Mars", Region = "Mars District?" },
                        new { LocationId = 3, Address = "a street p", City = "p", Country = "Pluto", Region = "Pluto District?" }
                    );
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.PropertyType", b =>
                {
                    b.Property<int>("PropertyTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type");

                    b.HasKey("PropertyTypeId");

                    b.ToTable("PropertyTypes");

                    b.HasData(
                        new { PropertyTypeId = 1, Type = "Apartment" },
                        new { PropertyTypeId = 2, Type = "Hostel" },
                        new { PropertyTypeId = 3, Type = "Hotel" },
                        new { PropertyTypeId = 4, Type = "Guesthouse" }
                    );
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HotelId");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.HasKey("RoomId");

                    b.HasIndex("HotelId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.RoomBed", b =>
                {
                    b.Property<int>("RoomId");

                    b.Property<int>("BedId");

                    b.HasKey("RoomId", "BedId");

                    b.HasIndex("BedId");

                    b.ToTable("RoomBeds");
                });

            modelBuilder.Entity("HotelBookingApp.Models.User.UserModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.Hotel", b =>
                {
                    b.HasOne("HotelBookingApp.Models.Hotel.Location", "Location")
                        .WithOne("Hotel")
                        .HasForeignKey("HotelBookingApp.Models.Hotel.Hotel", "LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HotelBookingApp.Models.Hotel.PropertyType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.Room", b =>
                {
                    b.HasOne("HotelBookingApp.Models.Hotel.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId");
                });

            modelBuilder.Entity("HotelBookingApp.Models.Hotel.RoomBed", b =>
                {
                    b.HasOne("HotelBookingApp.Models.Hotel.Bed", "Bed")
                        .WithMany("RoomBeds")
                        .HasForeignKey("BedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HotelBookingApp.Models.Hotel.Room", "Room")
                        .WithMany("RoomBeds")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
