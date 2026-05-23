using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VoguMap.Domain.Entities;

/// <summary>
/// Помещения
/// </summary>
[Table("rooms")]
public partial class Room
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Корпус
    /// </summary>
    [Column("building_id")]
    public int BuildingId { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Этаж
    /// </summary>
    [Column("floor")]
    public int Floor { get; set; }

    [ForeignKey("BuildingId")]
    [InverseProperty("Rooms")]
    public virtual Building Building { get; set; } = null!;
}
