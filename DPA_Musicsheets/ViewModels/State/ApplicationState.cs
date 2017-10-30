using System.ComponentModel;

namespace DPA_Musicsheets.ViewModels.State
{
    public abstract class ApplicationState
    {
        public abstract void Handle(MainViewModel context);
    }
}
