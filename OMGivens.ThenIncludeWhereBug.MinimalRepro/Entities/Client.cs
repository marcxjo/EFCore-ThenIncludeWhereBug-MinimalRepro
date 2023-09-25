using System.ComponentModel.DataAnnotations;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro.Entities;

public class Client
{
    [Key] public int Id { get; init; }

    public int RolodexId { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;
}