using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Trial_of_Kanoor
{
    class Character
    {
        int invwidth = 7;
        int invheight = 5;
        Item[,] inventory;
        bool invMenu;
        int invShowing;

        Item head;
        Item torso;
        Item legs;
        Item boots;

        bool clicked;
        int clickShowing;
        Point itemClicked;

        Vector2 dimension = new Vector2(4, 4);
        public Vector2 location = new Vector2(607, 3757);
        Rectangle rect;
        Vector2 velocity;

        public int health = 10000;
        public int maxHealth = 10000;
        public int damage = 25;
        public int armor = 1;

        public Character()
        {
            inventory = new Item[invwidth, invheight];
            inventory[0, 0] = new Item(new Point(0, 0));
            inventory[1, 0] = new Item(new Point(1, 0));
            inventory[2, 0] = new Item(new Point(2, 0));
            inventory[3, 0] = new Item(new Point(3, 0));
            inventory[4, 0] = new Item(new Point(4, 0));
            inventory[5, 0] = new Item(new Point(5, 0));
            inventory[6, 0] = new Item(new Point(6, 0));
            inventory[0, 1] = new Item(new Point(0, 1));
        }

        public void Update(GraphicsDeviceManager graphics, Camera cam, Arrow[] arrowlist, KeyboardState newK, KeyboardState oldK, MouseState newM, MouseState oldM)
        {
            location += velocity;
            if (newK.IsKeyDown(Keys.W) && oldK.IsKeyUp(Keys.W)) { velocity.Y = -2f; }
            if (newK.IsKeyDown(Keys.A)) velocity.X = -1f;
            if (newK.IsKeyDown(Keys.D)) velocity.X = +1f;
            if (newK.IsKeyDown(Keys.Tab) && oldK.IsKeyUp(Keys.Tab)) invMenu = !invMenu;
            if (clicked)
                clickShowing -= clickShowing / 5;
            else
                clickShowing -= (350+clickShowing)/5;

            if (invMenu)
                invShowing -= invShowing/5;
            else if (!invMenu)
                invShowing -= (350 + invShowing)/5;
            if (newM.LeftButton == ButtonState.Pressed && oldM.LeftButton == ButtonState.Released)
            {
                if (new Rectangle(clickShowing + 20, graphics.PreferredBackBufferHeight - 25 - 46 * 6 - 128, 310, 128).Contains(newM.Position))
                    clicked = true;
                else clicked = false;
                int nullcount = 0;
                for (int i = 0; i < arrowlist.Length; i++)
                    if (arrowlist[i] == null)
                    {
                        arrowlist[i] = new Arrow(location - new Vector2(0, dimension.Y / 2),
                                (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - location.X * cam.Zoom,
                                             newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - location.Y * cam.Zoom + dimension.Y / 2) /
                                (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - location.X * cam.Zoom,
                                             newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - location.Y * cam.Zoom + dimension.Y / 2).Length())), damage);
                        nullcount++;
                        break;
                    }
                if (nullcount == 0)
                {
                    int x = 0;
                    for (int i = 0; i < arrowlist.Length; i++)
                        if (arrowlist[x].lifeSpan < arrowlist[i].lifeSpan)
                            x = i;
                    arrowlist[x] = new Arrow(location - new Vector2(0, dimension.Y/2),
                                (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - location.X * cam.Zoom,
                                             newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - location.Y * cam.Zoom + dimension.Y / 2) /
                                (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - location.X * cam.Zoom,
                                             newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - location.Y * cam.Zoom + dimension.Y / 2).Length())), damage);
                }
                //for (int i = 0; i < arrowlist.Length; i++)
                //    if (arrowlist[i] == null)
                //    {
                //        arrowlist[i] = (new Arrow(Location,
                //        (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - Location.X * cam.Zoom,
                //                     newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - Location.Y * cam.Zoom + Dimension.Y / 2) /
                //        (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - Location.X * cam.Zoom,
                //                     newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - Location.Y * cam.Zoom + Dimension.Y / 2).Length())), Damage));
                //        break;
                //    }
            }
            if (velocity.Y == 0) velocity.Y = 0.5f;
            velocity.Y += 0.1f;
            velocity.X /= 1.1f;
            rect = new Rectangle(location.ToPoint(), dimension.ToPoint());

        }
        public void CamDraw(GraphicsDeviceManager graphics, SpriteBatch sb, Camera cam, Texture2D texPlayer, Texture2D texReticule, MouseState newM)
        {
            sb.Draw(texPlayer, rect, null, Color.FromNonPremultiplied(255,255,255,128), 0f, new Vector2(1,2), SpriteEffects.None, 0f);
            Vector2 reticuleLoc = location - dimension.Y / 2 * Vector2.UnitY + 25 *
                (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - location.X * cam.Zoom,
                             newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - location.Y * cam.Zoom + dimension.Y / 2) /
                (new Vector2(newM.X + cam.X * cam.Zoom - graphics.PreferredBackBufferWidth / 2 - location.X * cam.Zoom,
                             newM.Y + cam.Y * cam.Zoom - graphics.PreferredBackBufferHeight / 2 - location.Y * cam.Zoom + dimension.Y / 2).Length()));
            sb.Draw(texReticule, reticuleLoc
                , null, Color.White, (float)Math.Atan2(reticuleLoc.Y, reticuleLoc.X), new Vector2(texReticule.Width, texReticule.Height) / 2, 0.1f, SpriteEffects.None, 0f);
        }
        public void HUDDraw(GraphicsDeviceManager graphics, SpriteBatch sb, Camera cam, SpriteFont fontItemName, SpriteFont fontItemDesc, Texture2D texHealth, Texture2D pixel, Texture2D texArmorsheet, MouseState newM)
        {
            sb.Draw(texHealth, new Rectangle((int)(0.01 * graphics.PreferredBackBufferWidth), 0, (int)(0.98 * graphics.PreferredBackBufferWidth), 15), Color.DarkGray);
            sb.Draw(texHealth, new Rectangle((int)(0.01 * graphics.PreferredBackBufferWidth), 0, (int)((float)health / (float)maxHealth * 0.98 * graphics.PreferredBackBufferWidth), 15), Color.White);

            sb.Draw(pixel, new Rectangle(invShowing + 23 + 46 * itemClicked.X, graphics.PreferredBackBufferHeight - 59 - 46 * (invheight - itemClicked.Y - 1), 36, 36), null, Color.SlateBlue);
            sb.Draw(pixel, new Rectangle(clickShowing + 20, graphics.PreferredBackBufferHeight - 25 - 46 * 6, 310, 128), null, Color.FromNonPremultiplied(50, 50, 50, 255), 0f, Vector2.UnitY, SpriteEffects.None, 0f);
            sb.Draw(pixel, new Rectangle(clickShowing + 30, graphics.PreferredBackBufferHeight - 25 - 46 * 6 - 118, 64, 64), null, Color.Gray, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            if (inventory[itemClicked.X, itemClicked.Y] != null)
            {
                inventory[itemClicked.X, itemClicked.Y].DrawMenu(graphics, sb, fontItemName, fontItemDesc, texArmorsheet, new Vector2(clickShowing + 25 + 64 + 20, graphics.PreferredBackBufferHeight - 301 - 128 + 10));
                sb.Draw(texArmorsheet, new Rectangle(clickShowing + 30, graphics.PreferredBackBufferHeight - 25 - 46 * 6 - 118, 64, 64), new Rectangle(32 * inventory[itemClicked.X, itemClicked.Y].ID.X, 32 * inventory[itemClicked.X, itemClicked.Y].ID.Y, 32, 32), Color.White);
            }

            for (int x = 0; x < invwidth; x++) for (int y = 0; y < invheight; y++)
                {
                    bool selected = new Rectangle(invShowing + 25 + 46 * x, graphics.PreferredBackBufferHeight - 57 - 46 * (invheight - y - 1), 32, 32).Contains(newM.Position);
                    Color[] invCols;
                    if (selected) invCols = new Color[] { Color.DimGray, Color.Gray };
                    else invCols = new Color[] { Color.Gray, Color.White };
                    sb.Draw(pixel, new Rectangle(invShowing + 25 + 46 * x, graphics.PreferredBackBufferHeight - 25 - 46 * (invheight - y - 1), 32, 32), null, invCols[0], 0f, Vector2.UnitY, SpriteEffects.None, 0f);
                    if (inventory[x, y] != null) inventory[x, y].Draw(graphics, sb, texArmorsheet, x, invheight - y - 1, invShowing, invCols[1]);
                    if (selected && newM.LeftButton == ButtonState.Pressed)
                    {
                        clicked = true;
                        itemClicked = new Point(x, y);
                    }
                }
        }
        public void ResolveCollision(Color[,] platforms, KeyboardState newK, KeyboardState oldK)
        {
            //Resolve Downward Collisions including platform dropping.
            for (int i = 0; i < velocity.Y; i++)
            {
                try
                {
                    //this one is the platform drop color.
                    if (platforms[(int)location.X, (int)location.Y + i] == Color.Yellow)
                    {
                        if (!(newK.IsKeyDown(Keys.S) && oldK.IsKeyUp(Keys.S)))
                        {
                            location = new Vector2(location.X, location.Y + i);
                            velocity.Y = 0;
                        }
                        else
                            location.Y += 1;
                    }
                    if (platforms[(int)location.X, (int)location.Y + i] == Color.Black)
                    {
                        location = new Vector2(location.X, location.Y + i);
                        velocity.Y = 0;

                    }
                }
                catch { location = new Vector2(607, 3757); }
            }
            try
            {
                //Step up adjustments
                if (platforms[(int)location.X + 1, (int)location.Y - 1] == Color.Black)
                {
                    location.Y--;
                    velocity.Y = 0;
                }
                if (platforms[(int)location.X - 1, (int)location.Y - 1] == Color.Black)
                {
                    location.Y--;
                    velocity.Y = 0;
                }
            }
            catch { location = new Vector2(607, 3757); }
            //Resolve Rightward Collisions.
            for (int i = 0; i < velocity.X; i++)
            {
                try
                {
                    if (platforms[(int)(location.X + dimension.X / 2 + i), (int)(location.Y - dimension.Y / 2)] == Color.Red) 
                    {
                        location = new Vector2(location.X + i, location.Y);
                        velocity.X = 0;
                    }
                }
                catch { location = new Vector2(607, 3757); }
            }
            //Resolve Leftward Collisions.
            for (int i = 0; i < -velocity.X; i++)
            {
                try
                {
                    if (platforms[(int)(location.X - dimension.X / 2 - 1 - i), (int)(location.Y - dimension.Y / 2)] == Color.Red) 
                    {
                        location = new Vector2(location.X - i, location.Y);
                        velocity.X = 0;
                    }
                }
                catch { location = new Vector2(607, 3757); }
            }
            //Resolve Upward Collisions.
            for (int i = 0; i < -velocity.Y; i++)
            {
                try
                {
                    if (platforms[(int)(location.X), (int)(location.Y - dimension.Y - i)] == Color.Red)
                    {
                        location = new Vector2(location.X, location.Y - i);
                        velocity.Y = 0;
                    }
                }
                catch { location = new Vector2(607, 3757); }
            }
        }
    }
}
