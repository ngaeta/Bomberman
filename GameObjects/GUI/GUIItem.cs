using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class GUIItem : GameObject
    {
        public GUIItem(Vector2 spritePosition, string spriteSheetName) : base(spritePosition, spriteSheetName, DrawManager.Layer.GUI)
        {
        }
    }
}
