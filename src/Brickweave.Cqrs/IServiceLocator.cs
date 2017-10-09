using System;

namespace Brickweave.Cqrs
{
    public interface IServiceLocator
    {
        object GetInstance(Type type);
    }
}
