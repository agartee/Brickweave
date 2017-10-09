using System;

namespace Brickweave.Core
{
    public interface IServiceLocator
    {
        object GetInstance(Type type);
    }
}
