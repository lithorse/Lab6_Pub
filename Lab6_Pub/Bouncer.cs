using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Lab6_Pub
{
    class Bouncer
    {
        public Patron LetInPatron()
        {
            return new Patron();
        }
    }
}
