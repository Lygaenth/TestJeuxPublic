using Prism.Mvvm;
using TestJeux.Business.Managers.API;

namespace TestJeux.Display.ViewModels.Display.Menu
{
    public class OptionMenuViewModel : BindableBase
    {
        private ISoundService _soundManager;

        private double _currentMusicVolume;
        public double CurrentMusicVolume
        {
            get { return _currentMusicVolume; }
            set
            {
                SetProperty(ref _currentMusicVolume, value);
            }
        }

        public OptionMenuViewModel(ISoundService soundManager)
        {
            _soundManager = soundManager;
        }
    }
}
