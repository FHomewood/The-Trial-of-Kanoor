using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Trial_of_Kanoor
{
    class Item
    {
        public Point ID;
        string name = "";
        string description = "";
        int damage;
        int armor;

        public Item(Point ID)
        {
            this.ID = ID;
            if (ID == new Point(0,0)) { name = "T0 Helm"; description = "This helmet might stop a fast moving bug, not an axe..."; armor = 5; }
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch sb, Texture2D spritesheet, int x, int y, int invShowing, Color showcolor)
        {
            sb.Draw(spritesheet, new Rectangle(invShowing + 25 + 46 * x, graphics.PreferredBackBufferHeight - 25 - 46 * y, 32, 32), new Rectangle(32 * ID.X, 32 * ID.Y, 32, 32), showcolor, 0f, 32 *Vector2.UnitY, SpriteEffects.None, 0f);
        }

        public void DrawMenu(GraphicsDeviceManager graphics, SpriteBatch sb, SpriteFont fontItemName, SpriteFont fontItemDesc, Texture2D texSpritesheet, Vector2 textLoc)
        {
            string desc = WrapText(fontItemDesc, description, 200f);
            sb.DrawString(fontItemDesc, desc, textLoc + 20 * Vector2.UnitY, Color.White);
            sb.DrawString(fontItemName, name, textLoc, Color.White);
        }
        public string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder stringBuilder = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    stringBuilder.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (stringBuilder.ToString() == "")
                        {
                            stringBuilder.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                        else
                        {
                            stringBuilder.Append("\n" + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                    }
                    else
                    {
                        stringBuilder.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
