﻿// <auto-generated />
using System;
using Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Migrations
{
    [DbContext(typeof(ChatCompletionContext))]
    [Migration("20240616223741_v1.1")]
    partial class v11
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatMessageEntity", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatSessionKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartitionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Key");

                    b.HasIndex("ChatSessionKey");

                    b.ToTable("ChatMessages", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PartitionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("ChatSessions", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities.Forecast", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ForecastDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PartitionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("TemperatureF")
                        .HasColumnType("int");

                    b.Property<string>("ZipCodesSearch")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Key");

                    b.ToTable("Forecasts", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities.ForecastsView", b =>
                {
                    b.Property<Guid>("Key")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ForecastDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("TemperatureF")
                        .HasColumnType("int");

                    b.Property<string>("ZipCodesSearch")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Key");

                    b.ToTable((string)null);

                    b.ToView("ForecastsView", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities.WeatherForecastPostalCodeEntity", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PartitionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WeatherForecastKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("Key");

                    b.HasIndex("WeatherForecastKey");

                    b.ToTable("ForecastZipCodes", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Subject.AuthorEntity", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PartitionKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("Authors", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatMessageEntity", b =>
                {
                    b.HasOne("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", "ChatSession")
                        .WithMany("Messages")
                        .HasForeignKey("ChatSessionKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatSession");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities.WeatherForecastPostalCodeEntity", b =>
                {
                    b.HasOne("Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities.Forecast", "WeatherForecast")
                        .WithMany("ZipCodes")
                        .HasForeignKey("WeatherForecastKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeatherForecast");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities.Forecast", b =>
                {
                    b.Navigation("ZipCodes");
                });
#pragma warning restore 612, 618
        }
    }
}
