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
        ReproDbContext db,
        IEnumerable<string> clientLastNames
    )
    {
        Expression<Func<Client, bool>> clientFilter = c => true;

        if (clientLastNames.Any())
        {
            clientFilter = c => clientLastNames.Contains(c.LastName);
        }

        var businesses = await db.Businesses
            .AsQueryable()
            .Include(b => b.Rolodex)
            .ThenInclude(r => r.Clients.AsQueryable().Where(clientFilter))
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