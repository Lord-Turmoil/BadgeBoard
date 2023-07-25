// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Modules.BadgeAccount.Models;
using BadgeBoard.Api.Modules.BadgeBadge.Models;
using BadgeBoard.Api.Modules.BadgeUser.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeBoard.Api.Modules;

public class BadgeContext : DbContext
{
	public BadgeContext(DbContextOptions<BadgeContext> options) : base(options)
	{
	}
	// Sample Module
	// public DbSet<Weather> Weathers { get; set; }

	// Account Module
	public DbSet<UserAccount> Accounts { get; set; }
	public DbSet<EmailRecord> EmailRecords { get; set; }

	// User Module
	public DbSet<User> Users { get; set; }
	public DbSet<UserPreference> UserPreferences { get; set; }
	public DbSet<UserInfo> UserInfo { get; set; }
	public DbSet<FavoriteUser> FavoriteUsers { get; set; }

	// Badge Module
	public DbSet<Badge> Badges { get; set; }
	public DbSet<QuestionPayload> QuestionPayloads { get; set; }
	public DbSet<MemoryPayload> MemoryPayloads { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<CategoryOption> CategoriesOptions { get; set; }

	public DbSet<UnreadRecord> UnreadRecords { get; set; }
}