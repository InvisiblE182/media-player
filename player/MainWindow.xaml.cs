using System;
using System.IO;
using System.Runtime.InteropServices;
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
using Microsoft.Win32;
using System.Windows.Threading;



namespace player
{
    public partial class MainWindow : Window
    {
        public MediaPlayer player { get; set; }
        private List<double> volume { get; set; }
        private List<string> songList { get; set; }
        private int songIdx { get; set; }
        private int previousSong { get; set; }
        public string item { get; set; }
        public DispatcherTimer timer { get; set; }
        public Object lastVolContent { get; set; }

        public MainWindow()
        {
            Initialise();
        }

        private void Initialise()
        {
            player = new MediaPlayer();
            songIdx = 0;
            songList = new List<string>();
            volume = new List<double>();
            timer = new DispatcherTimer();
        }
        
        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            if (shuffleButton.Background == Brushes.White)
            {//se shuffle nefunguje zpět - hraje to tu samou co teď
                shuffleButton.Background = Brushes.Aqua;
            } else
            {
                shuffleButton.Background = Brushes.White;
            }
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            if (repeatButton.Background == Brushes.White)
            {
                repeatButton.Background = Brushes.Aqua;
            }
            else
            {
                repeatButton.Background = Brushes.White;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            playButton.Content = FindResource("Play");
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playButton.Content != FindResource("Pause") && songList.Count != 0)
            {
                playButton.Content = FindResource("Pause");
                Timer_Start();
                player.Play();
                tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                prgBar.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            }
            else
            {
                playButton.Content = FindResource("Play");
                player.Pause();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (songList.Count != 0)
            {
                prgBar.Value = 0;
                if (shuffleButton.Background == Brushes.White)
                {
                    previousSong = songIdx;
                    songIdx += 1;
                    if (songIdx == songList.Count)//current song was last in playlist
                    {
                        songIdx = 0;
                    }

                    player.Open(new Uri(songList[songIdx]));
                    tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                    if (playButton.Content == FindResource("Pause"))
                    {
                        Timer_Start();
                        player.Play();
                    }
                }
                else
                {
                    previousSong = songIdx;
                    while (songIdx == previousSong)//this prevents playing the same song again
                    {
                        Random rnd = new Random();
                        songIdx = rnd.Next(0, songList.Count);
                    }
                    
                    if (playButton.Content == FindResource("Pause"))
                    {
                        player.Open(new Uri(songList[songIdx]));
                        tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                        player.Play();
                        Timer_Start();
                        
                    }
                }
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (songList.Count != 0)
            {
                prgBar.Value = 0;
                if (shuffleButton.Background == Brushes.White)
                {
                    previousSong = songIdx;
                    songIdx -= 1;
                    if (songIdx < 0)
                    {
                        songIdx = songList.Count - 1;
                    }
                    
                    if (playButton.Content == FindResource("Pause"))
                    {
                        player.Open(new Uri(songList[songIdx]));
                        tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                        player.Play();
                        Timer_Start();
                    }
                    else
                    {
                        player.Open(new Uri(songList[songIdx]));
                        tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                    }
                }
                else
                {
                    previousSong = songIdx;
                    if (playButton.Content == FindResource("Pause"))
                    {
                        player.Open(new Uri(songList[songIdx]));
                        tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                        player.Play();
                        Timer_Start();
                    }
                    else
                    {
                        player.Open(new Uri(songList[songIdx]));
                        tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                    }
                    songIdx -= 1;
                    if (songIdx < 0)
                    {
                        songIdx = songList.Count - 1;
                    }
                }
            }
        }

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            if (player.IsMuted == false)
            {
                player.IsMuted = true;
                lastVolContent = volumeButton.Content;
                volumeButton.Content = FindResource("Muted");
                volumeButton.Background = Brushes.Aqua;
            }
            else
            {
                player.IsMuted = false;
                volumeButton.Background = Brushes.White;
                volumeButton.Content = lastVolContent;
            }
        }
        
        private void VolumeSlider_ValueChanged(object sender, EventArgs e)
        {
            player.Volume = volumeSlider.Value/100; //player.Volume is in range 0.0 - 1.0
            if (volumeSlider.Value > 66)
            {
                volumeButton.Content = FindResource("High Volume");

            }
            else if (volumeSlider.Value > 32)
            {
                volumeButton.Content = FindResource("Medium Volume");
            }
            else if (volumeSlider.Value > 0)
            {
                volumeButton.Content = FindResource("Low Volume");
            }
            else
            {
                volumeButton.Content = FindResource("Muted");
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            songIdx = 0;
            if (playButton.Content == FindResource("Pause"))
            {
                playButton.Content = FindResource("Play");
                player.Pause();
            }
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "All Files|*.mp3*";
            if (open.ShowDialog() == true)
            {
                songList.Clear();
                playlist.Items.Clear();
                foreach (string file in open.FileNames)
                {
                    songList.Add(file);
                }
                player.Open(new Uri(songList[songIdx]));
                foreach(string s in songList)
                {
                    playlist.Items.Add(System.IO.Path.GetFileName(s).Replace(".mp3", String.Empty));
                }
                tbPlaying.Text = "First up: " + playlist.Items[songIdx];  
                playlist.BorderBrush = Brushes.Gray;
            }
        }

        private void Timer_Start()
        {

            if (playButton.Content == FindResource("Pause"))
            {
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            prgTime.Text = String.Format("{0} / {1}", player.Position.ToString(@"mm\:ss"), player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            prgBar.Value = player.Position.TotalSeconds;
        }

        private void Playlist_DoubleClick(object sender, RoutedEventArgs e)
        {
            songIdx = playlist.SelectedIndex;
            player.Open(new Uri(songList[songIdx]));
            tbPlaying.Text = "Now playing: " + playlist.Items[songIdx]; ;
            Timer_Start();
            player.Play();
            
        }

        private void User_Change_Start(object sender, RoutedEventArgs e)
        {
            if (playButton.Content == FindResource("Pause"))
            {
                timer.Stop();
            }
        }

        private void Value_Changed(object sender, RoutedEventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds(prgBar.Value);
            if (songList.Count != 0)
            {
                prgTime.Text = String.Format("{0} / {1}", time.ToString(@"mm\:ss"), player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            if (Math.Round(prgBar.Value) / Math.Round(player.Position.TotalSeconds) != 1 && songList.Count != 0)
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)prgBar.Value);
                player.Position = ts;
            }
        }

        private void User_Change_Complete(object sender, RoutedEventArgs e)
        {
            if (playButton.Content == FindResource("Pause"))
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)prgBar.Value);
                player.Position = ts;
                Timer_Start();
            }
        }

    }
}
