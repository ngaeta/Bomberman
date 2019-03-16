using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    interface IHittable
    {
        int OnHit(GameObject hitter);
        bool IsHitted();
    }
}
