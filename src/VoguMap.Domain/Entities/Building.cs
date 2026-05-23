using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VoguMap.Domain.Entities;

/// <summary>
/// Учебные корпуса
/// </summary>
[Table("buildings")]
public partial class Building
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Полный адрес
    /// </summary>
    [Column("address")]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Широта
    /// </summary>
    [Column("latitude")]
    [Precision(9, 6)]
    public decimal? Latitude { get; set; }

    /// <summary>
    /// Долгота
    /// </summary>
    [Column("longitude")]
    [Precision(9, 6)]
    public decimal? Longitude { get; set; }

    [InverseProperty("Building")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
