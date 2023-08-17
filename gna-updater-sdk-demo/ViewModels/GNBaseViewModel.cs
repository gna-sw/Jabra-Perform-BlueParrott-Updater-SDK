using GalaSoft.MvvmLight;
using GNAUpdaterSDK_Demo.Interfaces;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class GNBaseViewModel : ViewModelBase, ISupportParentViewModel
    {
        public GNBaseViewModel(GNBaseViewModel parent)
        {
            ParentViewModel = parent;
        }

        #region ISupportParentViewModel Members

        private object _parentViewModel;
        public object ParentViewModel
        {
            get
            {
                return _parentViewModel;
            }
            set
            {
                _parentViewModel = value;
            }
        }

        #endregion
    }
}
