using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;

namespace DPA_Musicsheets.ViewModels.State
{
    public class StateUnsavedChanges : ApplicationState
    {
        public override void Handle(MainViewModel context)
        {
            var lilypondVm = SimpleIoc.Default.GetInstance<LilypondViewModel>();
            lilypondVm.LilypondSaved += (src, e) =>
            {
                if (e.Result)
                {
                    context.State = new StateSaved();
                }
            };
        }
    }
}
