using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Observables;
using System.Globalization;

namespace MicMuteIndicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver<AudioSwitcher.AudioApi.DeviceMuteChangedArgs>
    {
        private CoreAudioController coreAudioController;

        public static readonly DependencyProperty MicIsLiveProperty = DependencyProperty.Register("MicIsLive", typeof(bool), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            coreAudioController = new CoreAudioController();
            MicIsLive = !coreAudioController.DefaultCaptureDevice.IsMuted;
            coreAudioController.DefaultCaptureDevice.MuteChanged.Subscribe(this);
        }
        public bool MicIsLive
        {
           get { return (bool)GetValue(MicIsLiveProperty); }
           set { SetValue(MicIsLiveProperty, (object)value); }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            Dispatcher.Invoke(new Action(()=> { throw error; }));
        }

        public void OnNext(DeviceMuteChangedArgs value)
        {
            bool isMuted = value.IsMuted;
            Dispatcher.Invoke(new Action(() => MicIsLive = !isMuted));
        }
    }

    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BoolToBrushConverter : IValueConverter
    {
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }

}
