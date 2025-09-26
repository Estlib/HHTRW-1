using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using NAudio.Wave;

namespace Plat2d_2.EngineCore
{//https://chatgpt.com/share/68d2fe37-b5fc-8012-956e-04c16d03d902
    public class SFXEngineMUSIEF

    {
        private static readonly SFXEngineMUSIEF _instance = new SFXEngineMUSIEF();
        public static SFXEngineMUSIEF Instance => _instance;

        private readonly Dictionary<string, string> _filePaths = new Dictionary<string, string>();
        private readonly Dictionary<string, WaveOutEvent> _players = new Dictionary<string, WaveOutEvent>();
        private readonly Dictionary<string, AudioFileReader> _readers = new Dictionary<string, AudioFileReader>();

        private SFXEngineMUSIEF() { }

        public void RegisterSound(string name, string filePath)
        {
            _filePaths[name] = filePath;
        }

        public void Play(string name)
        {
            if (!_filePaths.ContainsKey(name))
                throw new ArgumentException($"Sound '{name}' not registered.");

            string path = _filePaths[name];

            // Stop any currently playing sound
            if (_players.ContainsKey(name))
            {
                _players[name].Stop();
                _players[name].Dispose();
                _players.Remove(name);
                _readers.Remove(name);
            }

            // Create new AudioFileReader and WaveOutEvent for playback
            var reader = new AudioFileReader(path);
            var player = new WaveOutEvent();
            player.Init(reader);
            player.Play();

            // Store the player and reader for potential future use
            _players[name] = player;
            _readers[name] = reader;
        }

        public void StopAll()
        {
            foreach (var player in _players.Values)
            {
                player.Stop();
                player.Dispose();
            }
            _players.Clear();
            _readers.Clear();
        }
    }
}
