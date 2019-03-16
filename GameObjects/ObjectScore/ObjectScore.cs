using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    class ObjectScore : GameObject, IPickable
    {
        static int[] points = new int[] {800, 1000, 1200, 1300, 1500};
        static string[] randomTexture = new string[] { "apple", "iceCream" };

        private bool isPicked;
        private int score;
        private AudioSource audioSource;
        private AudioClip clipOnPicked;

        public ObjectScore(Vector2 spritePosition) : base(spritePosition, randomTexture[RandomGenerator.GetRandom(0, randomTexture.Length)], DrawManager.Layer.Middleground)
        {
            Rect rect = new Rect(new Vector2(sprite.Width / 2, sprite.Height / 2), null, Width, Height);
            RigidBody = new RigidBody(spritePosition, this, null, rect);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Pickable;

            score = points[RandomGenerator.GetRandom(0, points.Length)];

            audioSource = new AudioSource();
            clipOnPicked = AudioManager.GetAudioClip("scoreable");
        }

        void IPickable.OnPlayerPick(Player p)
        {
            if (!isPicked)
            {
                IsActive = false;
                isPicked = true;

                GuiNumber guiNumber = new GuiNumber(Position, score.ToString());
                guiNumber.Dissolve = true;
                guiNumber.Scale *= 0.4f;

                audioSource.Play(clipOnPicked);
                p.AddScore(score);
            }
        }
    }
}
