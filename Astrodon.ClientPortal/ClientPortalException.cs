using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.ClientPortal
{
    public class ClientPortalException:Exception
    {
        public ClientPortalException(string message)
            :base(message)
        {

        }
    }
}
