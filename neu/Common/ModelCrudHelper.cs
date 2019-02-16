using Microsoft.EntityFrameworkCore;
using neu.Data;
using neu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Common
{
    public static class ModelCrudHelper
    {
        public static void DetachLocalInstance<T>(this ApplicationDbContext context, int entryId)
            where T : class, INeuContent
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
