﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TranVuDienQuan_Buoi4.Data;

public class TranVuDienQuan_Buoi4Context : IdentityDbContext<IdentityUser>
{
    public TranVuDienQuan_Buoi4Context(DbContextOptions<TranVuDienQuan_Buoi4Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
