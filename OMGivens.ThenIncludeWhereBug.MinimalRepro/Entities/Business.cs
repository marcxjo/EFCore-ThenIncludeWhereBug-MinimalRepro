using System.ComponentModel.DataAnnotations;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro.Entities;

public class Business
{
    [Key] public int Id { get; init; }

    public string CompanyName { get; init; }

    public Rolodex Rolodex { get; init; }
}