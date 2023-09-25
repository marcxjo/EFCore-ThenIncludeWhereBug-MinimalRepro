# EFCore-ThenIncludeWhereBug-MinimalRepro
Small project with unit tests to demonstrate that the EFCore in-memory database provider does not respect .Where() when applied to .ThenInclude()d data sets

This project was created as a minimal example of an issue reported on https://github.com/dotnet/efcore/issues/31849. It was subsequently discovered that change tracking must be disabled to ensure that the impacted filter is respected. This need is documented on https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#filtered-include, but was only discovered after the issue was filed.

This repo remains in place in order to maintain the integrity of the reference reported on the above noted issue and to provide at-a-glance insight into the necessary change to resolve issues like that which surfaced this issue. See the first PR filed against this repo for the solution.
