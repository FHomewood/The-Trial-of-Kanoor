using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

namespace The_Trial_of_Kanoor
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont fontItemName, fontItemDesc;
        Texture2D texBackground, pixel, evenPix, texPlatforms, texForeground, texBracket, texHealth, texReticule, texSpritesheet;
        Color[,] PlatformArray;
        Camera mapCam, characterCam;
        float[] camLocs = new float[] { 342, 683, 1024, 1366, 1706, 2048, 2389, 2731, 3078, 3410, 3754 };
        int camloc = 10;
        int elapsedFrames;
        MouseState newM, oldM;
        KeyboardState newK, oldK;
        string gameState = "Tower";

        Character Jim = new Character();
        Enemy[] enemylist = new Enemy[128];
        Arrow[] arrowlist = new Arrow[1024];

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            characterCam = new Camera(GraphicsDevice.Viewport);
            mapCam = new Camera(GraphicsDevice.Viewport);
            mapCam.Y = camLocs[camloc];
            oldM = Mouse.GetState();
            oldK = Keyboard.GetState();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fontItemName = Content.Load<SpriteFont>("ItemName");
            fontItemDesc = Content.Load<SpriteFont>("ItemDesc");

            pixel = Content.Load<Texture2D>("Pixel");
            evenPix = Content.Load<Texture2D>("2Pix");
            texBackground = Content.Load<Texture2D>("StaticGradient4096");
            texPlatforms = Content.Load<Texture2D>("PlatformPlan");
            texForeground = Content.Load<Texture2D>("Overlay");
            texSpritesheet = Content.Load<Texture2D>("ArmorSheet");
            texBracket = Content.Load<Texture2D>("HealthBracket");
            texHealth = Content.Load<Texture2D>("Health");
            texReticule = Content.Load<Texture2D>("Reticule");

            PlatformArray = GetColorArray(texPlatforms);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            TowerUpdate(gameState=="Tower");
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, characterCam.Transform);
                    spriteBatch.Draw(texBackground, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(texPlatforms, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(texForeground, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    foreach (Enemy X in enemylist) if (X != null) X.Draw(spriteBatch, evenPix);
                    foreach (Arrow X in arrowlist) if (X != null) X.Draw(spriteBatch, pixel);
                    Jim.CamDraw(graphics, spriteBatch, characterCam, evenPix, texReticule, newM);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, mapCam.Transform);
                spriteBatch.Draw(texBackground, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texPlatforms, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texForeground, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                foreach (Enemy X in enemylist) if (X != null) X.Draw(spriteBatch, evenPix);
                foreach (Arrow X in arrowlist) if (X != null) X.Draw(spriteBatch, pixel);
                Jim.CamDraw(graphics, spriteBatch, mapCam, evenPix, texReticule, newM);
                spriteBatch.End();
            }

            spriteBatch.Begin();
            Jim.HUDDraw(graphics, spriteBatch, mapCam, fontItemName, fontItemDesc, texHealth, pixel, texSpritesheet, newM);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        private Color[,] GetColorArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }
        private void KillDeadEnemies(Enemy[] enemylist)
        {
            for (int i = 0; i < enemylist.Length; i++)
                if (enemylist[i] != null && enemylist[i].health <= 0)
                    enemylist[i] = null;
        }
        private void TowerUpdate(bool Condition)

        {
            if (Condition)
            {
                elapsedFrames++;
                characterCam.X = Jim.location.X;
                characterCam.Y = Jim.location.Y;
                characterCam.Zoom = (float)graphics.PreferredBackBufferWidth / 300;
                mapCam.Zoom = (float)graphics.PreferredBackBufferWidth / 1214;
                newK = Keyboard.GetState();
                newM = Mouse.GetState();
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                Jim.ResolveCollision(PlatformArray, newK, oldK);
                Jim.Update(graphics, mapCam, arrowlist, newK, oldK, newM, oldM);
                if (Jim.location.Y - camLocs[camloc] < 200 && camloc > 0)
                    camloc--;
                if (Jim.location.Y - camLocs[camloc] > 200 && camloc < 10)
                    camloc++;
                mapCam.Y += (camLocs[camloc] - mapCam.Y) / 100;
                mapCam.Update(new Vector2(607, mapCam.Y));
                characterCam.Update(new Vector2(characterCam.X,characterCam.Y));

                foreach (Ghost X in enemylist)
                {
                    if (X != null) { X.Update(arrowlist, Jim); }
                }
                for (int i = 0; i < enemylist.Length; i++)
                    if (enemylist[i] != null && enemylist[i].health <= 0)
                        enemylist[i] = null;
                for (int i = 0; i < arrowlist.Length; i++) if (arrowlist[i] != null)
                    {
                        arrowlist[i].Update();
                        if (PlatformArray[(int)arrowlist[i].location.X, (int)arrowlist[i].location.Y] != Color.Transparent)
                        { arrowlist[i] = null; }
                    }
                if (newK.IsKeyDown(Keys.Space) && oldK.IsKeyUp(Keys.Space))
                {
                    int nullcount = 0;
                    for (int i = 0; i < enemylist.Length; i++)
                        if (enemylist[i] == null)
                        {
                            enemylist[i] = new Ghost(new Vector2(mapCam.X, mapCam.Y));
                            nullcount++;
                            break;
                        }
                    if (nullcount == 0)
                    {
                        int x = 0;
                        for (int i = 0; i < enemylist.Length; i++)
                            if (enemylist[x].lifeSpan < enemylist[i].lifeSpan)
                                x = i;
                        enemylist[x] = new Ghost(new Vector2(mapCam.X, mapCam.Y));
                    }
                }
                if (newK.IsKeyDown(Keys.P))
                { }

                oldK = newK;
                oldM = newM;
            }
        }
    }
}