﻿// <auto-generated />
using System;
using AppManagementSystem.DbObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppointmentManagementSystem.DbObjects.Migrations
{
    [DbContext(typeof(AppointmentManagementContext))]
    [Migration("20240722134326_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AppointmentManagementSystem.DomainObjects.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceType")
                        .HasColumnType("int");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Appointment");

                    b.HasDiscriminator().HasValue("Appointment");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AppointmentManagementSystem.DomainObjects.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("AppointmentManagementSystem.DomainObjects.MassageAppointment", b =>
                {
                    b.HasBaseType("AppointmentManagementSystem.DomainObjects.Appointment");

                    b.Property<int>("MassageServices")
                        .HasColumnType("int");

                    b.Property<int>("Preference")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("MassageAppointment");
                });

            modelBuilder.Entity("AppointmentManagementSystem.DomainObjects.PersonalTrainingAppointment", b =>
                {
                    b.HasBaseType("AppointmentManagementSystem.DomainObjects.Appointment");

                    b.Property<string>("CustomerComments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InjuriesOrPains")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrainingDuration")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("PersonalTrainingAppointment");
                });

            modelBuilder.Entity("AppointmentManagementSystem.DomainObjects.Appointment", b =>
                {
                    b.HasOne("AppointmentManagementSystem.DomainObjects.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}
