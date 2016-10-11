using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimaSysMessaging.BusinessLogic
{
   public static class TypeEnum
    {
        public enum Type
        {
            None,
            New,
            Outbox,
            Deleted
        };
    }
}
