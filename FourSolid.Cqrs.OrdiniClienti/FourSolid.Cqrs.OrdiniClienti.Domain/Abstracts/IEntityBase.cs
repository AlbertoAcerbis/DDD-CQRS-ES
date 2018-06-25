using System;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.Abstracts
{
    public interface IEntityBase
    {
        Guid Id { get; }
    }
}