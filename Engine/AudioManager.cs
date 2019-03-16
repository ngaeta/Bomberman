using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace Bomberman
{
    static class AudioManager
    {
        private static Dictionary<string, AudioClip> audioClips;
        private static List<AudioSource> audioSourcesToDispose;
        private static AudioSource audioBackground;
        private static AudioClip clipBackground;

        static AudioManager()
        {
            audioClips = new Dictionary<string, AudioClip>();
            audioSourcesToDispose = new List<AudioSource>();
        }

        public static void Load()
        {
            AddAudioClip("backgroundMusic", "Assets/Sounds/backgroundMusic8bit.ogg");
            AddAudioClip("bombExplosion", "Assets/Sounds/bombExplosion.wav");
            AddAudioClip("countdown", "Assets/Sounds/countdown.wav");
            AddAudioClip("start", "Assets/Sounds/start.wav");
            AddAudioClip("powerUp1", "Assets/Sounds/powerUp1.wav");
            AddAudioClip("powerUp2", "Assets/Sounds/powerUp2.wav");
            AddAudioClip("powerUp3", "Assets/Sounds/powerUp3.wav");
            AddAudioClip("scoreable", "Assets/Sounds/scoreable.wav");
            AddAudioClip("disappear1", "Assets/Sounds/disappear1.wav");
            AddAudioClip("disappear2", "Assets/Sounds/disappear2.wav");
            AddAudioClip("death", "Assets/Sounds/death.wav");
            AddAudioClip("gameover", "Assets/Sounds/gameover.wav");
            AddAudioClip("winner", "Assets/Sounds/winnerMusic.ogg");
            AddAudioClip("put", "Assets/Sounds/put.wav");
        }

        public static void SetBackgroundAudio(string nameClip)
        {
            audioBackground = new AudioSource();
            clipBackground = GetAudioClip(nameClip);
            clipBackground.Rewind();
        }

        public static void Update()
        {
            if (clipBackground != null)
            {
                audioBackground.Stream(clipBackground, Game.DeltaTime, true);
            }

            for (int i = 0; i < audioSourcesToDispose.Count; i++)
            {
                if (audioSourcesToDispose[i] == null)
                {
                    audioSourcesToDispose.Remove(audioSourcesToDispose[i]);
                    i--;
                }

                else if (!audioSourcesToDispose[i].IsPlaying)
                {
                    audioSourcesToDispose[i].Dispose();
                    audioSourcesToDispose.Remove(audioSourcesToDispose[i]);
                    i--;
                }
            }
        }

        public static void StopBackground()
        {
            if (clipBackground != null && audioBackground != null)
            {
                audioBackground.Stop();
            }
        }

        public static AudioClip GetAudioClip(string audioClipName)
        {
            if (audioClips.ContainsKey(audioClipName))
            {
                return audioClips[audioClipName];
            }

            return null;
        }

        public static void DisposeAudioSource(AudioSource audioSource)
        {
            audioSourcesToDispose.Add(audioSource);
        }

        private static bool AddAudioClip(string key, string audioClipName)
        {
            if (!audioClips.ContainsKey(key))
            {
                audioClips.Add(key, new AudioClip(audioClipName));
                return true;
            }
            return false;
        }

        public static void RemoveAll()
        {
            for (int i = 0; i < audioSourcesToDispose.Count; i++)
            {
                if (audioSourcesToDispose[i] == null)
                {
                    audioSourcesToDispose.Remove(audioSourcesToDispose[i]);
                    i--;
                }

                else if (!audioSourcesToDispose[i].IsPlaying)
                {
                    audioSourcesToDispose[i].Dispose();
                    audioSourcesToDispose.Remove(audioSourcesToDispose[i]);
                    i--;
                }
            }

            audioSourcesToDispose.Clear();
            audioBackground.Dispose();
            audioBackground = null;
            clipBackground = null;
        }
    }
}
