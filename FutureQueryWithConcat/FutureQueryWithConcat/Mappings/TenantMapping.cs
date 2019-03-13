using System;
using System.Collections.Generic;
using System.Text;
using FutureQueryWithConcat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FutureQueryWithConcat.Mappings
{
    public class TenantMapping : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(128);
        }
    }
}
