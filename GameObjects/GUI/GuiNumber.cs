using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class GuiNumber : GUIItem
    {
        private List<GUIItem> number;
        private Vector2 spritePosition;
        private Vector2 scale;
        private float timeToDissolve;
        private bool dissolve;
        private string num;

        public bool Dissolve
        {
            get
            {
                return dissolve;
            }
            set
            {
                dissolve = value;

                if(dissolve)
                {
                    timeToDissolve = 2f;
                }
            }
        }

        public override bool IsActive
        {
            get
            {
                return base.IsActive;
            }
            set
            {
                base.IsActive = value;

                if(number != null)
                {
                    for (int i = 0; i < number.Count; i++)
                    {
                        number[i].IsActive = value;
                    }
                }
            }
        }

        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;

                for (int i = 0; i < number.Count; i++)
                {
                    number[i].GetSprite().scale = scale;
                }

                SetNumber(num);
            }
        }

        public GuiNumber(Vector2 spritePosition, string number) : base(spritePosition, "number_0")
        {
            this.number = new List<GUIItem>();
            this.spritePosition = spritePosition;
            scale = sprite.scale;
            num = number;

            SetNumber(number);
        }

        public override void Update()
        {
            base.Update();

            if (IsActive && Dissolve)
            {
                if (timeToDissolve <= 0)
                {
                    IsActive = false;
                    Destroy();
                }
                else
                    timeToDissolve -= Game.DeltaTime;
            }
        }

        public override void Draw()
        {
            if (IsActive)
            {
                for (int i = 0; i < number.Count; i++)
                {
                    number[i].Draw();
                }
            }
        }

        public void SetNumber(string number)
        {
            for (int i = 0; i < this.number.Count; i++)
            {
                this.number[i].Destroy();
            }

            this.number.Clear();

            Vector2 startPos = spritePosition;

            for (int i = 0; i < number.Length; i++)
            {
                GUIItem currNum = new GUIItem(startPos, "number_" + number[i]);
                currNum.GetSprite().scale = scale;

                this.number.Add(currNum);
                startPos += new Vector2(1 * scale.X, 0);
            }
        }
    }
}
