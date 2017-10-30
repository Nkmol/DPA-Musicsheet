using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.ViewModels.CoR
{
    public abstract class KeyHandler
    {
        protected KeyHandler Successor;

        public KeyHandler SetSuccessor(KeyHandler successor)
        {
            this.Successor = successor;

            return this;
        }

        public abstract bool HandleRequest();
    }
}
