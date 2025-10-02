using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class BGMPlayer : SoundPlayer
    {
        private SoundPlayer _player;
        public List<BGM> songs = new List<BGM>();
        private string thisPlaying = string.Empty;

        public BGMPlayer(List<BGM> songs)
        {
            if (songs == null || songs.Count <1)
            {
                //songs.Add("assets/audio/bgm/rabbit game ost V4_64-2025-codetest - Track 01 (TITLE SCREEN - Adventure Scroll).wav");
            }
            this.songs = songs;
            thisPlaying = "assets/audio/bgm/rabbit game ost V4_64-2025-codetest - Track 01 (TITLE SCREEN - Adventure Scroll).wav";
            _player = new SoundPlayer(thisPlaying);
            _player.LoadAsync();
        }
        public void SelectPath(string fileName)
        {
            int index = 0;
            int matchentry = 0;
            foreach (var song in songs)
            {
                if (song.Filepath.Contains(fileName)) 
                {
                    matchentry = index;
                }
                index++;
            }
            _player.SoundLocation = songs[matchentry].Filepath;
            _player.LoadAsync();
        }
        // Play the currently selected file asynchronously
        public void Play()
        {
            ThreadPool.QueueUserWorkItem(_ => _player.Play());
        }  

        // Stop playback
        public void Stop()
        {
            _player.Stop();
        }
        public void PlayNow(string filename)
        {
            SelectPath(filename);
            Play();
        }
    }
}
