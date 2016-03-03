using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;

namespace Game
{
    class ship
    {

        struct Point
        {
            public double x;
            public double y;
        }
        
        public string type = "Ship";
        public bool showInfo = false;
        double x, y, w, h;
        double fuel = 300;
        double thrust = 0.25;
        double xVel = 0;
        double yVel = 0;
        double angle = 0;
        double angleVel = 0;
        double speed = 0;
        Point[] shPt = new Point[4];

        public ship(double x = 400, double y = 40, short w = 30, short h = 40)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            shPt[0].x = (w / 2 - (w / 2));        //
            shPt[0].y = (h * 0.75 - (h / 2));     //
            shPt[1].x = (w - (w / 2));            //
            shPt[1].y = (h - (h / 2));            //Calculate and
            shPt[2].x = (w / 2 - (w / 2));        //copy ship vertexes into one array
            shPt[2].y = (0 - (h / 2));            //
            shPt[3].x = (0 - (w / 2));            //
            shPt[3].y = (h - (h / 2));            //
        }
        
        public void tick(GameWindow game)
        {
            inputs(game);
            if (showInfo) info();
            move(game);
        }

        private void inputs(GameWindow game)
        {
            if ((game.Keyboard[Key.A]) && fuel > 0)
            { 
                angleVel+=0.1;
                fuel += thrust/2*-1;
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
        }

        private void info()
        {
            Console.Clear();
            Console.WriteLine("Fuel left.: " + fuel);
            Console.WriteLine("x.........: " + x);
            Console.WriteLine("y.........: " + y);
            Console.WriteLine("xVel......: " + xVel);
            Console.WriteLine("yVel......: " + yVel);
            Console.WriteLine("speed.....: " + speed);
            Console.WriteLine("Angle.....: " + angle);
            Console.WriteLine("AngleVel..: " + angleVel);
        }

        private void move(GameWindow game)
        {
            
        }
        
    }
}
