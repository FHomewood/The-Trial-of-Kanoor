using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Trial_of_Kanoor
{
    class Ghost:Enemy
    {
        public Ghost(Vector2 loc): base(new Vector2(10,10),100, 100, 10, 3,Color.Purple)
        {
            location = loc;
        }
        public void Update(Arrow[] arrowlist, Character focus)
        {
            lifeSpan++;
            if ((focus.location - location).Length() < 100)
            {
                aggTime = 1;
            }
            if (aggTime > 0)
            {
                aggTime++;
                location += (float)(0.25 * Math.Cos((float)lifeSpan / 10) + 0.3) * (focus.location - location) / (focus.location - location).Length();
                if (aggTime > 400)
                    aggTime = 0;
            }
            for (int X = 0; X < arrowlist.Count(); X++)
            {
                if (arrowlist[X] != null)
                    if (new Rectangle((location - dimension + dimension.X / 2 * Vector2.UnitX).ToPoint(), (dimension).ToPoint()).Contains(arrowlist[X].location)) 
                    {
                        health -= arrowlist[X].damage;
                        arrowlist[X] = null;
                    }
            }
            if (new Rectangle(location.ToPoint(), dimension.ToPoint()).Contains(focus.location.ToPoint()))
            {
                focus.health -= damage / focus.armor;
            }
        }
    }
}