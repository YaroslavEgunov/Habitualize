using Habitualize.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Habitualize.ViewModel
{
    // Contains information about current page
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _activeTab;
        private Rect _mascotPosition = new Rect(0.5, 0.5, -1, -1);
        private string _highlightKey;
        private bool _isOverlayVisible = true;
        private Rect _bubblePosition = new Rect(0.5, 0.65, -1, -1);
        public ContentView DynamicContent { get; set; }
        public string ActiveTab
        {
            get => _activeTab;
            set
            {
                if (_activeTab != value)
                {
                    _activeTab = value;
                    OnPropertyChanged(nameof(ActiveTab));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> TutorialContentRequested;
        public Rect MascotPosition
        {
            get => _mascotPosition;
            set
            {
                _mascotPosition = value;
                OnPropertyChanged(nameof(MascotPosition));
            }
        }
        public bool IsOverlayVisible
        {
            get => _isOverlayVisible;
            set
            {
                if (_isOverlayVisible != value)
                {
                    _isOverlayVisible = value;
                    OnPropertyChanged(nameof(IsOverlayVisible));
                }
            }
        }
        public string HighlightKey
        {
            get => _highlightKey;
            set { _highlightKey = value; OnPropertyChanged(nameof(HighlightKey)); }
        }
        public Rect BubblePosition
        {
            get => _bubblePosition;
            set
            {
                _bubblePosition = value;
                OnPropertyChanged(nameof(BubblePosition));
            }
        }
        public event Action<string> TutorialStepChanged;
        public string TutorialText { get; set; } = "Welcome";
        public ICommand NextTutorialStepCommand { get; }
        public bool IsTutorialActive { get; set; } = true;
        private int _step = 0;
        private readonly (string text, Rect mascot, Rect bubble, string highlightKey)[] _steps = new[]
        {
            ("Welcome, I'm your little helper", new Rect(0.5, 0.5, -1, -1), new Rect(0.5, 0.65, -1, -1), null),
            ("This is the main page of the application or the Map", new Rect(0.1, 0.65, -1, -1), new Rect(0.2, 0.8, -1, -1), "MapButton"),
            ("Here you can track your progress and level", new Rect(0.1, 0.2, -1, -1), new Rect(0.4, 0.3, -1, -1), null),
            ("Create habits and keep track of statistics", new Rect(0.5, 0.6, -1, -1), new Rect(0.55, 0.6, -1, -1), null),
            ("The next page is your profile", new Rect(0.1, 0.85, -1, -1), new Rect(0.5, 0.85, -1, -1), "ProfileButton"),
            ("Here you can write about yourself, chat with friends", new Rect(0.1, 0.35, -1, -1), new Rect(0.2, 0.45, -1, -1), "ProfileButton"),
            ("Then there is a schedule", new Rect(0.1, 0.85, -1, -1), new Rect(0.5, 0.85, -1, -1), "ScheduleButton"),
            ("Where all your habits will be displayed", new Rect(0.5, 0.6, -1, -1), new Rect(0.55, 0.6, -1, -1), "ScheduleButton"),
            ("After that, we move on to ready-made plans for 30 days", new Rect(0.3, 0.75, -1, -1), new Rect(0.3, 0.85, -1, -1), "ThirdButton"),
            ("It's easy start", new Rect(0.3, 0.75, -1, -1), new Rect(0.4, 0.65, -1, -1), "ThirdButton"),
            ("And finally the settings", new Rect(0.8, 0.85, -1, -1), new Rect(0.3, 0.85, -1, -1), "SettingsButton"),
            ("...and these are just settings", new Rect(0.4, 0.75, -1, -1), new Rect(0.6, 0.65, -1, -1), "SettingsButton"),
            ("The training is completed,\n" +
             "you are ready to conquer a previously unknown life \n" +
             "with useful habits, good luck traveler!", new Rect(0.5, 0.5, -1, -1), new Rect(0.5, 0.65, -1, -1), null),
        };

        public MainPageViewModel()
        {
            NextTutorialStepCommand = new Command(NextStep);
            SetStep(0);
        }

        private void NextStep()
        {
            _step++;
            if (_step < _steps.Length)
            {
                SetStep(_step);
            }
            else
            {
                IsTutorialActive = false;
                OnPropertyChanged(nameof(IsTutorialActive));
            }
        }

        private void SetStep(int step)
        {
            var s = _steps[step];
            TutorialText = s.text;
            MascotPosition = s.mascot;
            BubblePosition = s.bubble;
            HighlightKey = s.highlightKey;
            OnPropertyChanged(nameof(TutorialText));
            OnPropertyChanged(nameof(MascotPosition));
            OnPropertyChanged(nameof(BubblePosition));
            OnPropertyChanged(nameof(HighlightKey));
            TutorialStepChanged?.Invoke(s.highlightKey);
            IsOverlayVisible = !(step == 2 || step == 3 || step == 5 || step == 7 || step == 9 || step == 11);
            if (s.text.Contains("Welcome", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Map");
            }
            else if (s.text.Contains("Map", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Map");
            }
            else if (s.text.Contains("карта", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Map");
            }
            else if (s.text.Contains("карта", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Map");
            }
            else if (s.text.Contains("profile", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Profile");
            }
            else if (s.text.Contains("schedule", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Schedule");
            }  
            else if (s.text.Contains("30 days", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Third");
            }            
            else if (s.text.Contains("settings", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Settings");
            }
            else if (s.text.Contains("traveler", StringComparison.OrdinalIgnoreCase))
            {
                TutorialContentRequested?.Invoke("Map");
            }
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}