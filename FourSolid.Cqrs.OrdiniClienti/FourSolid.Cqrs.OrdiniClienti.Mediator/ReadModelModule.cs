using Autofac;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Repository;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class ReadModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentUnitOfWork>().As<IDocumentUnitOfWork>().SingleInstance();
        }
    }
}