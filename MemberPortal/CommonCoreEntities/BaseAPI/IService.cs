using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public interface IService<T> : IService<T, long>
    { }

    public interface IService<T, T_ID> : IGetCount
    {
    }

    public interface IGetCount
    {
        Task<int> GetCount();

        Task<int> GetFirstOrDefault();
    }
}
