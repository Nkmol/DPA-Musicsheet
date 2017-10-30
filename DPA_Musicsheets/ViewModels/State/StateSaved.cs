using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;

namespace DPA_Musicsheets.ViewModels.State
{
    public class StateSaved : ApplicationState
    {
        public override void Handle(MainViewModel context)
        {
            var lilypondVm = SimpleIoc.Default.GetInstance<LilypondViewModel>();
            lilypondVm.LilypondTextChanged += (src, e) =>
            {
                if (lilypondVm.TextChanged(e.LilypondText))
                {
                    context.State = new StateUnsavedChanges();
                }
            };
        }
    }
}
