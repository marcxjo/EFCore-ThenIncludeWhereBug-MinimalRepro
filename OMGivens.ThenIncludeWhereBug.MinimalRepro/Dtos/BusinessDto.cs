using System.Collections.Generic;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro.Dtos;

public record BusinessDto(
    int Id,
    string CompanyName,
    RolodexDto Rolodex
);

public record RolodexDto(
    int Id,
    IEnumerable<ClientDto> Clients
);

public record ClientDto(
    int Id,
    string FirstName,
    string LastName
);
