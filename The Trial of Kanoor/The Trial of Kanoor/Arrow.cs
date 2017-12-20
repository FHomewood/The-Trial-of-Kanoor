using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Trial_of_Kanoor
{
    class Arrow
    {
        public Vector2 location;
        public Vector2 velocity;
        public int damage;
        public int lifeSpan;

        public Arrow(Vector2 location, Vector2 velocity, int damage)
        {
            this.location = location;
            this.velocity = velocity;
            this.damage = damage;
        }
        
        public void Update()
        {
            lifeSpan++;
            location += velocity;
        }

        public void Draw(SpriteBatch sb, Texture2D tex)
        {
            sb.Draw(tex, location, null, Color.Green, 0f, new Vector2(tex.Width, tex.Height) / 2, 1f, SpriteEffects.None, 0f);
        }
    }
}