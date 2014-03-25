using System;
using System.Diagnostics;
using System.Collections.Generic;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DracosD.Models;

namespace DracosD.Controllers
{
    class ForceController : Controller
    {
        #region Fields
        // Reference to the rocket
        private Dragon dragon;
        private List<PlanetaryObject> planets;
        private float G = 1.1f;
    #endregion

    #region Properties (READ-WRITE)
        /// <summary>
        /// The currently active dragon
        /// </summary>
        /// <remarks>
        /// The controller can only affect one dragon at a time.
        /// </remarks>
        public Dragon Dragon {
            get { return dragon;  }
            set { dragon = value; }
        }

        public List<PlanetaryObject> Planets
        {
            get { return planets; }
            set { planets = value; }
        }

        public float Gravity
        {
            get { return G; }
            set { G = value;  }
        }
    #endregion

    #region Methods
        /// <summary>
        /// Create a new controller for the given rocket
        /// </summary>
        /// <param name="rocket">The rocket</param>
        public ForceController(Dragon dragon, List<PlanetaryObject> planets)
            : base(ControllerType.AbstractForceController) {
            this.dragon = dragon;
            this.planets = planets;
        }


        public void applyPlanetGrav(List<PlanetaryObject> planets)
        {
            foreach (PlanetaryObject p in planets)
            {
                Vector2 gravity = p.Position - dragon.Position;
                float force = (dragon.Body.Mass * p.Body.Mass * G) / gravity.Length();
                gravity = gravity / gravity.Length();
                gravity = gravity * force;

                dragon.Body.ApplyForce(gravity);

                //Debug.Print("GRAVITY: " + gravity);
            }
                
        }


        /// <summary>
        /// Apply appropriate forces while collisions are processed
        /// </summary>
        /// <param name="dt">Timing values from parent loop</param>
        public override void Update(float dt) {
            if (!dragon.Body.Enabled) {
                return;
            }

            // Get thrust from rocket and orient with rotation.
            Vector2 force = dragon.Force;
            //Debug.Print("Dragonforce: " + force);
            force = Vector2.Transform(force, Matrix.CreateRotationZ(dragon.Rotation));

            // Apply force to the rocket BODY, not the rocket
            dragon.Body.ApplyForce(force);

            applyPlanetGrav(planets);
        }
    #endregion


    }
}
