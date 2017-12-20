using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Trial_of_Kanoor
{
    abstract class Enemy
    {
        public Vector2 location;
        public Vector2 dimension;
        public Vector2 velocity;
        public int health;
        public int maxHealth;
        public int damage;
        public int resistance;
        public int lifeSpan;
        public int aggTime;
        public Color col;

        protected Enemy(Vector2 dimension, int health, int maxHealth, int damage, int resistance, Color col)
        {
            this.dimension  = dimension;
            this.health     = health;
            this.maxHealth  = maxHealth;
            this.damage     = damage;
            this.resistance = resistance;
            this.col        = col;
        }
        public void Draw(SpriteBatch sb, Texture2D tex)
        {
            sb.Draw(tex, new Rectangle((int)location.X, (int)location.Y, (int)dimension.X, (int)dimension.Y), null, col, 0f, new Vector2(tex.Width / 2, tex.Height), SpriteEffects.None, 0f);
            sb.Draw(tex, new Rectangle((int)location.X - 5, (int)(location.Y - dimension.Y) - 10, 20, 4), null, Color.DarkRed, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            sb.Draw(tex, new Rectangle((int)location.X - 5, (int)(location.Y - dimension.Y) - 10, (int)((float)health / (float)maxHealth * 20), 4), null, Color.Green, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
