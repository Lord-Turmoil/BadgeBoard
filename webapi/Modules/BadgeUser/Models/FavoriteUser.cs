// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using System.ComponentModel.DataAnnotations;

namespace BadgeBoard.Api.Modules.BadgeUser.Models;

public class FavoriteUser
{
    [Key] public int Id { get; set; }

    public int SrcId { get; set; }
    public int DstId { get; set; }
}