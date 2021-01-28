using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace VideoPlayer
{
    public partial class MainWindow : Window
    {
        private bool userIsDraggingSlider = false;
        
        public MainWindow() => InitializeComponent();
        private void BtnOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.DefaultExt = "*.*";
            openFileDialog.Filter = "Media Files (*.-*)|*.*";
            openFileDialog.ShowDialog();

            try { vPlayer.Source = new Uri(openFileDialog.FileName); }
            catch { new NullReferenceException("Error"); }


            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            
            if ((vPlayer.Source != null) && (vPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                progress.Minimum = 0;
                progress.Maximum = vPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                progress.Value = vPlayer.Position.TotalSeconds;
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            vPlayer.Position = TimeSpan.FromSeconds(progress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(progress.Value).ToString(@"hh\:mm\:ss");
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vPlayer.Volume = volume.Value;
        }

        private void BtnPlay(object sender, RoutedEventArgs e)
        {
            vPlayer.Play();
        }

        private void BtnPause(object sender, RoutedEventArgs e)
        {
            vPlayer.Pause();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

