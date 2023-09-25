using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Database;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Dtos;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Entities;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro.Core;

public class QueryMethods
{
    public static async Task<IEnumerable<BusinessDto>> GetBusinesses(
        ReproDbContext db
    )
    {
        var businesses = await db.Businesses
            .Include(b => b.Rolodex)
            .ThenInclude(r => r.Clients)
            .ToArrayAsync();

        return businesses.Select(
            b => new BusinessDto(
                b.Id,
                b.CompanyName,
                new RolodexDto(
                    b.Rolodex.Id,
                    b.Rolodex.Clients.Select(
                        c => new ClientDto(
                            c.Id,
                            c.FirstName,
                            c.LastName
                        )
                    )
                )
            )
        );
    }

    public static async Task<IEnumerable<BusinessDto>> GetBusinesses(
        ReproDbContext db,
        IEnumerable<string> clientLastNames
    )
    {
        var businesses = await db.Businesses
            .AsNoTracking() // https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#filtered-include
            .Include(b => b.Rolodex)
            .ThenInclude(r => r.Clients.Where(c => clientLastNames.Contains(c.LastName)))
            .ToArrayAsync();

        return businesses.Select(
            b => new BusinessDto(
                b.Id,
                b.CompanyName,
                new RolodexDto(
                    b.Rolodex.Id,
                    b.Rolodex.Clients.Select(
                        c => new ClientDto(
                            c.Id,
                            c.FirstName,
                            c.LastName
                        )
                    )
                )
            )
        );
    }
}
