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
    [DbContext(typeof(SemanticKernelContext))]
    [Migration("20240704180236_InitialCreate")]
    partial class InitialCreate
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

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Author.AuthorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.HasIndex("Timestamp")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Timestamp"));

                    b.ToTable("Authors", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatMessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatSessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("ChatSessionId");

                    b.HasIndex("Id")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.HasIndex("Timestamp")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Timestamp"));

                    b.ToTable("ChatMessages", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("AuthorId");

                    b.HasIndex("Id")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.HasIndex("Timestamp")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Timestamp"));

                    b.ToTable("ChatSessions", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.TextGeneration.TextPromptEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Prompt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("AuthorId");

                    b.HasIndex("Id")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.HasIndex("Timestamp")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Timestamp"));

                    b.ToTable("TextPrompts", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.TextGeneration.TextResponseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TextPromptId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.HasIndex("TextPromptId");

                    b.HasIndex("Timestamp")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Timestamp"));

                    b.ToTable("TextResponses", (string)null);
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatMessageEntity", b =>
                {
                    b.HasOne("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", "ChatSession")
                        .WithMany("Messages")
                        .HasForeignKey("ChatSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatSession");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", b =>
                {
                    b.HasOne("Goodtocode.SemanticKernel.Core.Domain.Author.AuthorEntity", "Author")
                        .WithMany("ChatSessions")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.TextGeneration.TextPromptEntity", b =>
                {
                    b.HasOne("Goodtocode.SemanticKernel.Core.Domain.Author.AuthorEntity", "Author")
                        .WithMany("TextPrompts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.TextGeneration.TextResponseEntity", b =>
                {
                    b.HasOne("Goodtocode.SemanticKernel.Core.Domain.TextGeneration.TextPromptEntity", "TextPrompt")
                        .WithMany("TextResponses")
                        .HasForeignKey("TextPromptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TextPrompt");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.Author.AuthorEntity", b =>
                {
                    b.Navigation("ChatSessions");

                    b.Navigation("TextPrompts");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.ChatSessionEntity", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Goodtocode.SemanticKernel.Core.Domain.TextGeneration.TextPromptEntity", b =>
                {
                    b.Navigation("TextResponses");
                });
#pragma warning restore 612, 618
        }
    }
}