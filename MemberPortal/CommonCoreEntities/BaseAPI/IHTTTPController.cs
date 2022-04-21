using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public interface IHTTPController<T>
    {
        Task<ActionResult<T>> Post(T item);
        Task<IList<T>> GetAll();
        Task<ActionResult<T>> Get(long id);
        //TODO: Solve dependency issue
        Task<IActionResult> Put(long id, T item);
        //Task<IActionResult> Delete(long id);
    }
}
