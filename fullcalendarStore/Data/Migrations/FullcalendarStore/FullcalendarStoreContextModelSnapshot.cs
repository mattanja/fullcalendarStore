﻿// <auto-generated />
using fullcalendarStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace fullcalendarStore.Data.Migrations.FullcalendarStore
{
    [DbContext(typeof(FullcalendarStoreContext))]
    partial class FullcalendarStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("fullcalendarStore.Models.CalendarItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("CalendarItems");
                });
#pragma warning restore 612, 618
        }
    }
}