﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PrettyNeatGenericAPI.Models.DbModels;

#nullable disable

namespace PrettyNeatGenericAPI.Migrations
{
    [DbContext(typeof(PrettyNeatGenericAPIDbContext))]
    [Migration("20230918101331_CheckedBagsLogsModified")]
    partial class CheckedBagsLogsModified
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.BagInventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Assigned")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("AssignedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("AssignedTo")
                        .HasColumnType("integer");

                    b.Property<string>("BagNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CheckedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ChitNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DateIssued")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DeliveryNoteId")
                        .HasColumnType("integer");

                    b.Property<string>("InspectionCode")
                        .HasColumnType("text");

                    b.Property<string>("InspectionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsChecked")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsDelivered")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsFirstTime")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsReturned")
                        .HasColumnType("boolean");

                    b.Property<int?>("LastAssignedTo")
                        .HasColumnType("integer");

                    b.Property<string>("MONumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("QuantityFailed")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityIssued")
                        .HasColumnType("integer");

                    b.Property<int?>("QuantityPassed")
                        .HasColumnType("integer");

                    b.Property<string>("RequestedDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReturnedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("TssFault")
                        .HasColumnType("boolean");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryNoteId");

                    b.ToTable("BagInventory");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.CheckedBagsLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BagNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float?>("ChargePrice")
                        .HasColumnType("real");

                    b.Property<DateTime>("CheckedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ChitNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DateIssued")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MONumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<string>("PunchCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("QuantityIssued")
                        .HasColumnType("integer");

                    b.Property<string>("RequestedDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("CheckedBagsLogs");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.DeliveryNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BagNumber")
                        .HasColumnType("text");

                    b.Property<string>("ChitNumber")
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateOnly?>("DateDispatched")
                        .HasColumnType("date");

                    b.Property<string>("InspectionType")
                        .HasColumnType("text");

                    b.Property<string>("MONumber")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("PartNumber")
                        .HasColumnType("text");

                    b.Property<bool?>("Printed")
                        .HasColumnType("boolean");

                    b.Property<string>("Quality")
                        .HasColumnType("text");

                    b.Property<int?>("QuantityFailed")
                        .HasColumnType("integer");

                    b.Property<int?>("QuantityIssued")
                        .HasColumnType("integer");

                    b.Property<int?>("QuantityPassed")
                        .HasColumnType("integer");

                    b.Property<string>("Supplier")
                        .HasColumnType("text");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("DeliveryNote");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.DeliveryNotePrinted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<List<string>>("BagNumbers")
                        .HasColumnType("text[]");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<List<int>>("DeliveryNoteIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("DeliveryNotePrinted");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<List<string>>("AssignedBag")
                        .HasColumnType("text[]");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PunchCode")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Citizenship")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IDCard")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UnderlyingConditions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.Qualifiers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float?>("ChargePerPiece")
                        .HasColumnType("real");

                    b.Property<float?>("ChargePerPieceDelay")
                        .HasColumnType("real");

                    b.Property<float?>("ChargePerPieceExtraDelay")
                        .HasColumnType("real");

                    b.Property<float?>("ClientPiecesPerHour")
                        .HasColumnType("real");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DateOfIssue")
                        .HasColumnType("text");

                    b.Property<float?>("EmployeePiecesPerHour")
                        .HasColumnType("real");

                    b.Property<string>("PartNumber")
                        .HasColumnType("text");

                    b.Property<float?>("PayPerPiece")
                        .HasColumnType("real");

                    b.Property<float?>("PayPerPieceDelay")
                        .HasColumnType("real");

                    b.Property<float?>("PayPerPieceExtraDelay")
                        .HasColumnType("real");

                    b.Property<string>("ResponsiblePerson")
                        .HasColumnType("text");

                    b.Property<string>("TssCode")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Qualifiers");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.ReturnLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BagNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmployeeSurname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fault")
                        .HasColumnType("text");

                    b.Property<string>("PunchCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReturnedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("ReturnLogs");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.ReturnedBags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BagNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CountReturned")
                        .HasColumnType("integer");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReturnedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("ReturnBags");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Models.DbModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("PrettyNeatGenericAPI.Data.Models.BagInventory", b =>
                {
                    b.HasOne("PrettyNeatGenericAPI.Data.Models.DeliveryNote", "DeliveryNote")
                        .WithMany()
                        .HasForeignKey("DeliveryNoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryNote");
                });
#pragma warning restore 612, 618
        }
    }
}
