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
        public void Draw(GameView view, Vector2 relPosition, Vector2 Position, int lapNum, Gate goal)
        {
            view.BeginHUDPass(relPosition, Position, goal.Position, lapNum);
            view.EndHUDPass();
        }


        #endregion



    }
}
