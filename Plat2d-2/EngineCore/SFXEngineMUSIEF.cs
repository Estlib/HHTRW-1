using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

//MU-lti SI-ngle EF-fect

namespace Plat2d_2.EngineCore
{
    public class SFXEngineMUSIEF
    {
        private static readonly SFXEngineMUSIEF _instance = new SFXEngineMUSIEF();
        public static SFXEngineMUSIEF Instance => _instance;

        private readonly Dictionary<string, List<SoundPlayer>> _playersPool = new Dictionary<string, List<SoundPlayer>>();
        private readonly Dictionary<string, int> _nextPlayerIndex = new Dictionary<string, int>();

        private SFXEngineMUSIEF() { }

        /// <summary>
        /// Register a sound by name and file path, with optional initial pool size.
        /// </summary>
        public void RegisterSound(string name, string filePath, int poolSize = 9)
        {
            if (_playersPool.ContainsKey(name))
            {
                foreach (var oldPlayer in _playersPool[name])
                    oldPlayer.Stop();
            }

            var list = new List<SoundPlayer>();
            for (int i = 0; i < poolSize; i++)
            {
                var player = new SoundPlayer(filePath);
                player.LoadAsync();
                list.Add(player);
            }

            _playersPool[name] = list;
            _nextPlayerIndex[name] = 0;

            LogUtility.MonitorAudiosystem($"Registered audio {filePath} as '{name}' with pool size {poolSize}", 6);
            Log.Info($"Registered audio {filePath} as '{name}' with pool size {poolSize}", 6);
        }

        /// <summary>
        /// Play a sound by name. Dynamically expands the pool if needed.
        /// </summary>
        public void Play(string name, bool cutItself = true)
        {
            if (!_playersPool.ContainsKey(name))
            {
                Log.Error($"Sound '{name}' not registered.");
                throw new ArgumentException($"Sound '{name}' not registered.");
            }

            var pool = _playersPool[name];
            int index = _nextPlayerIndex[name];

            // Dynamically expand the pool if needed
            if (index >= pool.Count)
            {
                var newPlayer = new SoundPlayer(pool[0].SoundLocation);
                newPlayer.LoadAsync();
                pool.Add(newPlayer);
            }

            var player = pool[index];

            if (cutItself)
            {
                // Stop only this instance
                player.Stop();
            }

            player.Play(); // async play

            // Move to next player in pool
            _nextPlayerIndex[name] = (index + 1) % pool.Count;

            Log.Info($"Playing SFX '{name}' (cutItself={cutItself}) using pool index {index}");
        }

        /// <summary>
        /// Stop all sounds immediately.
        /// </summary>
        public void StopAll()
        {
            foreach (var pool in _playersPool.Values)
            {
                foreach (var player in pool)
                {
                    player.Stop();
                }
            }

            Log.Select("All SFX stopped // StopAll()");
        }
    }
}
