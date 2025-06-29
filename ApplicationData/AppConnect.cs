using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pract.ApplicationData
{
    class AppConnect
    {
        public static ex_practEntities modelOdb { get; set; }
        public static Users currentUser;

        public static void Initialize()
        {
            modelOdb = new ex_practEntities();
        }
    }
}
