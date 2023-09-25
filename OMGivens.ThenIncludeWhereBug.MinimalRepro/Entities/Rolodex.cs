using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro.Entities;

public class Rolodex
{
    [Key] public int Id { get; init; }

    public int BusinessId { get; init; }

    public string? CollectionType { get; init; }

    public Business? Business { get; init; }

    public ICollection<Client> Clients { get; init; } = new List<Client>();
}