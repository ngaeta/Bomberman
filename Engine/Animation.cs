using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Animation : IActivable, IUpdatable, ICloneable
    {
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public int OffsetX { get; private set; }
        public int OffsetY { get; private set; }

        float frameDelay;
        float counter;
        int startX;
        int startY;

        int currentFrameIndex;
        int currentFrame
        {
            get { return currentFrameIndex; }
            set
            {
                currentFrameIndex = value;
                OffsetX = startX + (currentFrameIndex % cols) * FrameWidth;
                OffsetY = startY + (currentFrameIndex / cols) * FrameHeight;
            }
        }


        int rows;
        int cols;

        int numFrames;

        public int LoopAtFrame { get; set; }

        Dictionary<int, float> waitFrame;

        bool loop;
        public bool ReverseAfterFinish { set; get; }
        public bool IsPlaying { get; protected set; }
        public int CurrFrame { get { return currentFrameIndex; } }
        public float Speed { get { return frameDelay; } set { frameDelay = value; } }
        public bool IsActive { get; set; }

        private bool isPaused { get; set; }
        private int frameToAdd;


        public Animation(int fwidth, int fheight, int cols = 1, int rows = 1, float fps = 1f, bool loop = true, int startX = 0, int startY = 0)
        {
            FrameWidth = fwidth;
            FrameHeight = fheight;
            this.loop = loop;
            this.rows = rows;
            this.cols = cols;
            this.startX = startX;
            this.startY = startY;
            frameDelay = 1 / fps;
            numFrames = rows * cols;
            IsActive = true;
            currentFrame = 0;  //al
            LoopAtFrame = int.MinValue;
            ReverseAfterFinish = false;
            frameToAdd = 1;
        }

        protected virtual void OnAnimationEnds()
        {
            if(ReverseAfterFinish)
            {
                frameToAdd = 1;
            }

            if (loop)
            {
                currentFrame = 0;
            }
            else
            {
                IsActive = false;
                IsPlaying = false;
            }
        }
        public void Update()
        {
            if (IsActive)
            {
                if (!isPaused && currentFrame != LoopAtFrame)
                {
                    IsPlaying = true;
                    counter += Game.DeltaTime;

                    if (counter >= frameDelay)
                    {
                        counter = 0;
                        currentFrame += frameToAdd;

                        if (currentFrame == numFrames)
                        {
                            if (!ReverseAfterFinish)
                                OnAnimationEnds();
                            else
                                frameToAdd = -1;
                        }
                        else if (ReverseAfterFinish && currentFrame < 0)
                            OnAnimationEnds();
                    }
                }
            }
        }

        public void Play()
        {
            isPaused = false;
            IsPlaying = true;
            IsActive = true;
        }

        public void Reset()
        {
            currentFrame = 0;
            IsActive = true;
        }

        public void Pause()
        {
            isPaused = true;
            IsPlaying = false;
            counter = 0;
        }

        public object Clone()
        {
            return MemberwiseClone(); //crea copia superficiale della classe
        }
    }
}
