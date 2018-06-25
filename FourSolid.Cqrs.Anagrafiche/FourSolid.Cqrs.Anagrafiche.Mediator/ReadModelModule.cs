using Autofac;
using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Repository;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
{
    public class ReadModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentUnitOfWork>().As<IDocumentUnitOfWork>().SingleInstance();
        }
    }
}