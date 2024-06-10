using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public interface ICommandSteps<T>
    {
        T Sut { get; }
        Guid SutKey { get; }
        IList<T> RecycleBin { get; }
        Task Cleanup();
    }
}
