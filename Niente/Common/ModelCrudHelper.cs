using Microsoft.EntityFrameworkCore;
using Niente.Data;
using Niente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Common
{
    public static class ModelCrudHelper
    {
        public static void DetachLocalInstance<T>(this ApplicationDbContext context, int entryId)
            where T : class, INienteContent
        {
            var local = context.Set<T>()
                               .Local
                               .FirstOrDefault(m => m.Id == entryId);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
        }
    }
}
