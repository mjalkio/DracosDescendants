using System;
using System.Collections.Generic;
using System.Linq;
using DracosD.Views;
using System.Text;
using DracosD.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DracosD.Controllers
{
    class HUDController
    {
        #region Fields
        LevelController level;
        Dragon racer;
        Dictionary<Dragon, int> currentGates;
        #endregion

        #region Initialization
        public HUDController(LevelController level, Dragon racer, Dictionary<Dragon, int> currentGates)
        {
            this.level = level;
            this.racer = racer;
            this.currentGates = currentGates;
        }


        #endregion

        #region Methods
        //drawing method for the dragon heads
        public void Draw(GameView view, Vector2 relPosition, Vector2 Position, int lapNum, int playerLap, Gate goal, int d_id, float gameOffset)
        {
            if (d_id == 0)
            {
                view.BeginHUDPassPlayer(relPosition, Position, goal.Position, lapNum, playerLap, gameOffset);
            }
            else
            {
                view.BeginHUDPassAI(relPosition, Position, goal.Position, lapNum, playerLap, d_id, gameOffset);
            }
            view.EndHUDPass();
        }

        //drawing method for the progress bar
        public void Draw2(GameView view)
        {
            view.BeginHUDPass2();
            view.EndHUDPass();
            view.BeginHUDPass3();
            view.EndHUDPass();
        }


        #endregion



    }
}
