using System;
using System.Linq;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Core;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Database;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Dtos;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Entities;
using Xunit;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro;

public class When_using_in_memory_provider_to_query_Business_Data : IDisposable
{
    private ReproDbContext _db;

    private Business _fakeBusiness;
    private Rolodex _fakeRolodex;
    private Client _fakeClient0;
    private Client _fakeClient1;
    private Client _fakeClient2;

    public When_using_in_memory_provider_to_query_Business_Data()
    {
        _db = new
            ReproDbContext(
                new DbContextOptionsBuilder<ReproDbContext>()
                    .UseInMemoryDatabase("TestDb")
                    .Options
            );

        _fakeBusiness = new Business
        {
            Id = 1,
            CompanyName = "Springfield Nuclear Power Plant"
        };

        _fakeRolodex = new Rolodex()
        {
            Id = 1,
            BusinessId = _fakeBusiness.Id,
            CollectionType = "Business-y Things"
        };

        _fakeClient0 = new Client()
        {
            Id = 1,
            RolodexId = _fakeRolodex.Id,
            FirstName = "Homer",
            LastName = "Simpson"
        };

        _fakeClient1 = new Client()
        {
            Id = 2,
            RolodexId = _fakeRolodex.Id,
            FirstName = "Marge",
            LastName = "Simpson"
        };

        _fakeClient2 = new Client()
        {
            Id = 3,
            RolodexId = _fakeRolodex.Id,
            FirstName = "Hank",
            LastName = "Scorpio"
        };

        _db.AddRange(
            _fakeBusiness,
            _fakeRolodex,
            _fakeClient0,
            _fakeClient1,
            _fakeClient2
        );

        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.RemoveRange(
            _fakeBusiness,
            _fakeRolodex,
            _fakeClient0,
            _fakeClient1,
            _fakeClient2
        );

        _db.SaveChanges();
    }

    [Fact]
    public async Task The_expected_dto_is_returned_when_not_filtering_clients()
    {
        var dto = await QueryMethods.GetBusinesses(
            _db,
            Array.Empty<string>()
        );

        dto.Single().ShouldCompare(
            new BusinessDto(
                _fakeBusiness.Id,
                _fakeBusiness.CompanyName,
                new RolodexDto(
                    _fakeRolodex.Id,
                    new[]
                    {
                        new ClientDto(
                            _fakeClient0.Id,
                            _fakeClient0.FirstName,
                            _fakeClient0.LastName
                        ),
                        new ClientDto(
                            _fakeClient1.Id,
                            _fakeClient1.FirstName,
                            _fakeClient1.LastName
                        ),
                        new ClientDto(
                            _fakeClient2.Id,
                            _fakeClient2.FirstName,
                            _fakeClient2.LastName
                        ),
                    }
                )
            ),
            "The returned DTO does not match the expected model",
            new ComparisonConfig
            {
                MaxDifferences = 50,
                IgnoreCollectionOrder = true,
                IgnoreConcreteTypes = true
            }
        );
    }

    [Fact]
    public async Task Filtering_on_clients_named_Simpson_is_respected()
    {
        var dto = await QueryMethods.GetBusinesses(
            _db,
            new[] { "Simpson" }
        );


        dto.Single().ShouldCompare(
            new BusinessDto(
                _fakeBusiness.Id,
                _fakeBusiness.CompanyName,
                new RolodexDto(
                    _fakeRolodex.Id,
                    new[]
                    {
                        new ClientDto(
                            _fakeClient0.Id,
                            _fakeClient0.FirstName,
                            _fakeClient0.LastName
                        ),
                        new ClientDto(
                            _fakeClient1.Id,
                            _fakeClient1.FirstName,
                            _fakeClient1.LastName
                        ),
                    }
                )
            ),
            "The returned DTO does not match the expected model",
            new ComparisonConfig
            {
                MaxDifferences = 50,
                IgnoreCollectionOrder = true,
                IgnoreConcreteTypes = true
            }
        );
    }

    [Fact]
    public async Task Filtering_on_clients_named_Scorpio_is_respected()
    {
        var dto = await QueryMethods.GetBusinesses(
            _db,
            new[] { "Scorpio" }
        );

        dto.Single().ShouldCompare(
            new BusinessDto(
                _fakeBusiness.Id,
                _fakeBusiness.CompanyName,
                new RolodexDto(
                    _fakeRolodex.Id,
                    new[]
                    {
                        new ClientDto(
                            _fakeClient2.Id,
                            _fakeClient2.FirstName,
                            _fakeClient2.LastName
                        )
                    }
                )
            ),
            "The returned DTO does not match the expected model",
            new ComparisonConfig
            {
                MaxDifferences = 50,
                IgnoreCollectionOrder = true,
                IgnoreConcreteTypes = true
            }
        );
    }
}
