using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels.CoR
{
    public class KeyHandlerModifierKeys : KeyHandler
    {
        public List<Key> CurrentKeysPushed { get; }
        private readonly Key _key;

        public KeyHandlerModifierKeys(Key key, List<Key> currentKeysPushed) : base()
        {
            CurrentKeysPushed = currentKeysPushed;
            _key = key;
        }

        public override bool HandleRequest()
        {
            // If successor is null, we are not dependent on next check -> true
            return CurrentKeysPushed.Contains(_key) && (Successor == null || (Successor?.HandleRequest() ?? false));
        }

        public static KeyHandlerModifierKeys Creator(List<Key> currentKeysPushed, params Key[] keys)
        {
            KeyHandlerModifierKeys handler = null;
            foreach (var key in keys)
            {
                if (handler == null)
                    handler = new KeyHandlerModifierKeys(key, currentKeysPushed);
                else
                    handler.SetSuccessor(new KeyHandlerModifierKeys(key, currentKeysPushed));
            }

            return handler;
        }
    }
}
