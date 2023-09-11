﻿using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.UserManagement;
using UserManagement.Domain.UserManagement.Advocacy;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [ConnectionStringName(UserDbConsts.EfConnectionStrinName)]
    public class UsermanagementDbContext : AbpDbContext<UsermanagementDbContext>, IUsermanagementDbContext
    {
        public UsermanagementDbContext(DbContextOptions<UsermanagementDbContext> options) : base(options)
        {
        }

        public DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureAuditLogging();
            builder.ConfigureUserManagement();
        }
    }
}
