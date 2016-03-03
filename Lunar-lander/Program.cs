using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;

namespace Game
{
    class RootThingy
    {
        public static int windowX = 640;
        public static int windowY = 480;
        public static string exePath = Environment.CurrentDirectory;
        public static double sceneX = 640;
        public static int sceneY = 480;

        public static Random rnd = new Random();

        public static int moon;

        public struct Point
        {
            public double x;
            public double y;
        }

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [STAThread]
        public static void Main()
        {
            double gravity = 0.02;
            
            int z = 60;
            int w = 30;
            int h = 40;
            Point[] shPt = new Point[4];

            shPt[0].x = (w / 2 - (w / 2));        //
            shPt[0].y = (h * 0.75 - (h / 2));     //
            shPt[1].x = (w - (w / 2));            //
            shPt[1].y = (h - (h / 2));            //Calculate and
            shPt[2].x = (w / 2 - (w / 2));        //copy ship vertexes into one array
            shPt[2].y = (0 - (h / 2));            //
            shPt[3].x = (0 - (w / 2));            //
            shPt[3].y = (h - (h / 2));            //

            Rect[] shpColls = new Rect[(int)w * h / 16];
            int shpCollsI = 0;

            //Player data
            double fuel = 300;
            double thrust = 0.25;
            double friction = 0.01;
            double x = 400;
            double y = 41;
            double xVel = 0;
            double yVel = 0;
            double angle = 0;
            double angleVel = 0;
            double speed = 0;
            Rect shpBndBox = new Rect();

            
            double moonR = 256;
            double moonX = (sceneX / 2) - moonR;
            double moonY = sceneY - (moonR / 2);

            int tileNr = 0;

            Random rnd = new Random();
            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.X = 32;
                    game.Y = 16;
                    game.VSync = VSyncMode.On;
                    game.Width = windowX;
                    game.Height = windowY;
                    game.WindowBorder = WindowBorder.Fixed; //Disables the resizable windowframe
                    GL.Enable(EnableCap.Blend);                                                     //These lines
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  //enable transparency using alpha-channel

                    moon = LoadTexture(exePath + @"\moon.bmp");


                }; String version = Environment.OSVersion.VersionString;

                game.Resize += (sender, e) =>
                {
                    //sceneX = game.Height;
                    //sceneY = game.Width;
                    GL.Viewport(0, 0, windowX, windowY);
                };
                var mouse = Mouse.GetState();
                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    mouse = Mouse.GetState();

                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }

                    if ((game.Keyboard[Key.A]) && fuel > 0)
                    {
                        angleVel += 0.1;
                        fuel += thrust / 2 * -1;
                    }
                    if ((game.Keyboard[Key.D]) && fuel > 0)
                    {
                        angleVel -= 0.1;
                        fuel += thrust / 2 * -1;
                    }

                    if ((game.Keyboard[Key.S]) && fuel > 0)
                    {
                        speed += thrust;
                        fuel -= thrust / 2;
                    }
                    if ((game.Keyboard[Key.W]) && fuel > 0)
                    {
                        speed -= thrust;
                        fuel -= thrust / 2;
                    }

                    if (game.Keyboard[Key.R])
                    {
                        x = 400;
                        y = 400;
                        xVel = 0;
                        yVel = 0;
                        angle = 0;
                        angleVel = 0;
                        speed = 0;
                        fuel = 300;
                        shPt[0].x = (w / 2 - (w / 2));        //
                        shPt[0].y = (h * 0.75 - (h / 2));     //
                        shPt[1].x = (w - (w / 2));            //
                        shPt[1].y = (h - (h / 2));            //Calculate and
                        shPt[2].x = (w / 2 - (w / 2));        //copy ship vertexes into one array
                        shPt[2].y = (0 - (h / 2));            //
                        shPt[3].x = (0 - (w / 2));            //
                        shPt[3].y = (h - (h / 2));            //
                    }

                    if (game.Keyboard[Key.Keypad9])
                    { z += 5; }
                    if (game.Keyboard[Key.Keypad3])
                    { z -= 5; }

                    if (game.Keyboard[Key.KeypadAdd])
                    { tileNr++; }
                    if (game.Keyboard[Key.KeypadMinus])
                    { tileNr--; }

                    if (game.Keyboard[Key.F1])  //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    { ; }

                    if (angle > 360)
                        angle -= 360;
                    if (angle < 0)
                        angle += 360;

                    if (speed > 0)
                        speed -= friction;
                    if (speed < 0)
                        speed += friction;

                    if (angleVel > 0.01)
                        angleVel -= friction;
                    else
                        angleVel -= friction * -1;
                    angle = angle + angleVel;

                    xVel = Math.Sin(angle * 0.0174532925) * speed;
                    yVel = Math.Cos(angle * 0.0174532925) * speed;


                    x += xVel;
                    y += yVel;



                    Console.Clear();
                    Console.WriteLine("Fuel left.: " + fuel);
                    Console.WriteLine("x.........: " + x);
                    Console.WriteLine("y.........: " + y);
                    Console.WriteLine("xVel......: " + xVel);
                    Console.WriteLine("yVel......: " + yVel);
                    Console.WriteLine("speed.....: " + speed);
                    Console.WriteLine("Angle.....: " + angle);
                    Console.WriteLine("AngleVel..: " + Math.Round(angleVel, 5));
                    Console.WriteLine("EXE-Path..: " + exePath);
                    Console.WriteLine("FPS.......: " + (int)(game.RenderFrequency));
                    Console.WriteLine("======================================");


                    shPt[0] = rotate(shPt[0], angleVel);   //
                    shPt[1] = rotate(shPt[1], angleVel);   //Rotate all ship-points
                    shPt[2] = rotate(shPt[2], angleVel);   //
                    shPt[3] = rotate(shPt[3], angleVel);   //



                    //Check for collision with Point-Array Object
                    for (int i = 0; i != shPt.Length; i++)  //collision with old rect moon
                    {
                        if (shPt[i].y+y >= 500)
                        {
                            speed = 0;
                            yVel = 0;
                            xVel = 0;
                            while (shPt[i].y+y > 500)
                            {
                                y -= 0.1;
                                for (int j = 0; j != shPt.Length; j++)
                                {shPt[j].y -= 0.01;}
                            }
                        }
                    }

                    Console.WriteLine("Mouse X: " + game.Mouse.X);
                    Console.WriteLine("Mouse Y: " + game.Mouse.Y);
                    if (Circlecollision(moonX + moonR, moonY + moonR, moonR, game.Mouse.X, game.Mouse.Y))
                        Console.WriteLine("Mouse Collision: Yes");
                    else
                        Console.WriteLine("Mouse Collision: No");

                    for (int i = 0; i != shPt.Length; i++)
                    {
                        if (Circlecollision(moonX+moonR, moonY+moonR, moonR, shPt[i].x+x, shPt[i].y+y))
                        {
                            Console.WriteLine("Collision: Yes");
                            //speed = 0;
                            //yVel = 0;
                            //xVel = 0;
                        }
                        else
                        { Console.WriteLine("Collision: No"); }
                    }




                    game.Title = (("FPS: " + (int)(game.RenderFrequency) + " ; " + Math.Round(game.RenderTime * 1000, 2) + "ms/frame"));
                };

                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(0, windowX, windowY, 0, -1000, 1000);  //Render  distant objects smaller
                    
                    GL.MatrixMode(MatrixMode.Modelview);


                    GL.Begin(PrimitiveType.Quads); //Background
                    GL.Color3(0.5f, 0.5f, 0.5f);
                    GL.Vertex2(0, 0);
                    GL.Vertex2(sceneX, 0);
                    GL.Vertex2(sceneX, sceneY);
                    GL.Vertex2(0, sceneY);
                    GL.End();

                    GL.Color3(1.0f, 1.0f, 1.0f);
                    beginDraw2D();
                    drawImage(moon , moonX, moonY);    //Moon
                    endDraw2D();

                    
                    



                    GL.Begin(PrimitiveType.TriangleFan); //ship
                    GL.Color3(0.0f, 0.0f, 1.0f);
                    GL.Vertex2(shPt[0].x + x, shPt[0].y + y);
                    GL.Color3(0.0f, 1.0f, 0.0f);
                    GL.Vertex2(shPt[1].x + x, shPt[1].y + y);
                    GL.Color3(1.0f, 0.0f, 0.0f);
                    GL.Vertex2(shPt[2].x + x, shPt[2].y + y);
                    GL.Color3(1.0f, 0.0f, 1.0f);
                    GL.Vertex2(shPt[3].x + x, shPt[3].y + y);
                    GL.End();

                    GL.Begin(PrimitiveType.LineLoop);   //ship bounding collisionBox
                    GL.Color3(0.0f, 1.0f, 0.0f);
                    GL.Vertex2(shpBndBox.bottom, shpBndBox.right);
                    GL.Vertex2(shpBndBox.bottom, shpBndBox.left);
                    GL.Vertex2(shpBndBox.top, shpBndBox.left);
                    GL.Vertex2(shpBndBox.top, shpBndBox.right);
                    GL.End();


                    /////////////////////////////ship-Collision NEEDS TO BE MOVED
                    shpBndBox.left = 0;
                    shpBndBox.right = 0;
                    shpBndBox.top = 0;
                    shpBndBox.bottom = 0;
                    foreach (Point pos in shPt) //calculate the rectangular hitbox of ship (or any given points-array)
                    {
                        if (pos.y < shpBndBox.left)
                            shpBndBox.left = (int)pos.y;
                        if (pos.y > shpBndBox.right)
                            shpBndBox.right = (int)pos.y;
                        if (pos.x < shpBndBox.top)
                            shpBndBox.top = (int)pos.x;
                        if (pos.x > shpBndBox.bottom)
                            shpBndBox.bottom = (int)pos.x;
                    }
                    shpBndBox.left += (int)y;
                    shpBndBox.right += (int)y;
                    shpBndBox.top += (int)x;
                    shpBndBox.bottom += (int)x;

                    GL.Begin(PrimitiveType.Quads);  //Square moon
                    GL.Color3(0.8f, 0.8f, 0.8f);
                    GL.Vertex2(0, 500);
                    GL.Vertex2(0, 600);
                    GL.Vertex2(800, 600);
                    GL.Vertex2(800, 500);
                    GL.End();



                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }

        //End of Root =============================================================


        static Point rotate(double x, double y, double angle)
        {
            angle = angle * 0.0174532925;
            Point rotated = new Point();
            rotated.x = x * Math.Cos(angle) + y * Math.Sin(angle);
            rotated.y = y * Math.Cos(angle) - x * Math.Sin(angle);

            return rotated;
        }

        static Point rotate(Point p, double angle)
        {
            angle = angle * 0.0174532925;
            Point rotated = new Point();
            rotated.x = p.x * Math.Cos(angle) + p.y * Math.Sin(angle);
            rotated.y = p.y * Math.Cos(angle) - p.x * Math.Sin(angle);

            return rotated;
        }

        static double distance(double x1, double y1, double x2, double y2)
        {
            double x = Math.Abs(x1 - x2);
            double y = Math.Abs(y1 - y2);
            return Math.Sqrt((x * x) + (y * y));
        }

        static double distance(Point p1, Point p2)
        {
            double x = Math.Abs(p1.x - p2.x);
            double y = Math.Abs(p1.x - p2.y);
            return Math.Sqrt((x * x) + (y * y));
        }




        public static int LoadTexture(string file)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("Texture '" + file + "' could not be loaded!");
                Console.ReadKey();
                return 0;
            }

            Bitmap bitmap = new Bitmap(file);
            bitmap.MakeTransparent(Color.Magenta);
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return tex;
        }


        public static void drawImage(int image, double x, double y, bool flipV = false, bool flipH = false)
        {
            int w;
            int h;
            GL.Color4(1, 1, 1, 1.0f);
            GL.BindTexture(TextureTarget.Texture2D, image);
            
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            GL.Begin(PrimitiveType.Quads);
            if (!flipV && !flipH)    //No flip
            {
                GL.TexCoord2(0, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x, y + h);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2(1, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x, y + h);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2(0, 1);
                GL.Vertex2(x, y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x, y + h);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2(1, 1);
                GL.Vertex2(x, y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x, y + h);
            }
            GL.End();
        }


        public static void beginDraw2D()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, RootThingy.windowX, RootThingy.windowY, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        public static void endDraw2D()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }


        public static bool Circlecollision(double cx, double cy, double r, Point pExt)
        {
            Point circle = new Point();
            circle.x = cx;
            circle.y = cy;
            double dis = distance(circle, pExt);
            if (dis <= r)
                return true;
            return false;
        }

        public static bool Circlecollision(double cx, double cy, double r, double px, double py)
        {
            Point circle = new Point();
            circle.x = cx;
            circle.y = cy;
            Point pExt = new Point();
            pExt.x = px;
            pExt.y = py;
            double dis = distance(circle, pExt);
            Console.WriteLine("Distance: " + dis);
            if (dis <= r)
                return true;
            return false;
        }




    }
}
