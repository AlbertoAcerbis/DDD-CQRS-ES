using System;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Abstracts
{
    public interface IEntityBase
    {
        Guid Id { get; }
    }
}