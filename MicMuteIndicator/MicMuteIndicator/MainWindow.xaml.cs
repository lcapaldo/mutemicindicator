﻿using System;
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

namespace MicMuteIndicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver<AudioSwitcher.AudioApi.DeviceMuteChangedArgs>
    {
        private CoreAudioController coreAudioController;

        public static readonly DependencyProperty MutedProperty = DependencyProperty.Register("Muted", typeof(bool), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            coreAudioController = new CoreAudioController();
            Muted = coreAudioController.DefaultCaptureDevice.IsMuted;
            coreAudioController.DefaultCaptureDevice.MuteChanged.Subscribe(this);
        }
        public bool Muted
        {
           get { return (bool)GetValue(MutedProperty); }
           set { SetValue(MutedProperty, (object)value); }
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
            Dispatcher.Invoke(new Action(() => Muted = isMuted));
        }
    }

    

}
