using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Museum.Data.Entities;

namespace Museum.Data.Context
{
    public class MuseumContext : DbContext
    {

        public DbSet<ExhabitEntity> Exhabit { get; set; }
        public DbSet<ExhibitionEntity> Exhibition { get; set; }
        public DbSet<MuseumEntity> Museum { get; set; }
        public DbSet<AuditoriumEntity> Auditoriums { get; set; }


        public MuseumContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// <summary>
            /// Museum -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<MuseumEntity>()
                .HasMany(x => x.Auditoriums)
                .WithOne(x => x.Museum)
                .IsRequired();

            /// <summary>
            /// Auditorium -> Museum relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<AuditoriumEntity>()
                .HasOne(x => x.Museum)
                .WithMany(x => x.Auditoriums)
                .HasForeignKey(x => x.MuseumId)
                .IsRequired();


            /// <summary>
            /// Auditorium -> Exhibition relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<AuditoriumEntity>()
               .HasMany(x => x.Exhibition)
               .WithOne(x => x.Auditorium)
               .IsRequired();

            /// <summary>
            /// Exhibition -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<ExhibitionEntity>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Exhibition)
                .HasForeignKey(x => x.AuditoriumId)
                .IsRequired();

            /// <summary>
            /// Exhibition -> Exhabit relation
            /// </summary>
            /// <returns></returns>
            //modelBuilder.Entity<ExhibitionEntity>()
            //    .HasOne(x => x.Exhabit)
            //    .WithMany(x => x.Exhibition)
            //    .HasForeignKey(x => x.ExhabitId)
            //    .IsRequired();

            /// <summary>
            /// Exhabit -> Exhibition relation
            /// </summary>
            /// <returns></returns>
        }
    }


    }
