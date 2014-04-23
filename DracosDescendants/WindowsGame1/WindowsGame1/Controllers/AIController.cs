#region File Description
//-----------------------------------------------------------------------------
// AIController.cs
//
// This class implements all of the AI in the game.
// The approach is to used a continuous potential field
// computation each frame to get the direction vector 
// the AI should move next.
//
// Author: Justin Bard
// Based on formulas from http://students.cs.byu.edu/~cs470ta/goodrich/fall2004/lectures/Pfields.pdf
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using DracosD.Models;
#endregion

namespace DracosD.Controllers
{
    class AIController
    {
        #region Fields
        private LevelController level;
        private Dragon racer;
        private Vector2 nextDirection; //normalized vector for the next direction the AI will move
        private float horizontal;
        private float vertical;
        private Dictionary<Dragon, int> currentGates; //map to look up racer's current gate that he's on and access that by index in the level
        private Gate goalGate;
        private Vector2 goalEdge;
        private Vector2 goal; //the position in the world the dragon is trying to get to next
        #endregion

        #region Properties
        public Dragon Racer
        {
            get { return racer; }
            set { racer = value; }
        }

        /// <summary>
        /// Gets or sets the nextDirection the AI will move 
        /// </summary>
        public Vector2 NextDirection
        {
            get { return nextDirection; }
            set { nextDirection = value; }
        }

        /// <summary>
        /// Gets or sets the current gates that all racers are on...used to retrieve the current gate for this AI
        /// </summary>
        public Dictionary<Dragon, int> CurrentGates
        {
            get { return currentGates; }
            set { currentGates = value; }
        }

        //READ-ONLY
        public float Horizontal
        {
            get
            {
                if (NextDirection != null) return NextDirection.X;
                else return 0.0f;
            }
        }

        public float Vertical
        {
            get
            {
                if (NextDirection != null) return NextDirection.Y;
                else return 0.0f;
            }
        }

        public Gate GoalGate
        {
            get { return level.Gates[currentGates[racer]]; }
        }
        
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize an AI controller with the current level, the racer, and a map of all of the current gates the racers are at
        /// </summary>
        /// <param name="ai">The Dragon that is controlled by this AI</param>
        /// <param name="currLevel">The current level the AI is navigating</param>
        /// <param name="currGates">The current gates of all of the racers in the game</param>
        public AIController(Dragon ai, LevelController currLevel, Dictionary<Dragon, int> currGates)
        {
            racer = ai;
            level = currLevel;
            currentGates = currGates;
            nextDirection = Vector2.Zero;
            goal = new Vector2(0.0f, 0.0f);
            
            //select the first goal
            SelectGoal();
            //Debug.Print(""+GoalPosition());
            /*foreach (Vector2 v in GoalPositions())
            {
                Debug.Print(""+v);
            }*/
            //Debug.Print(""+gradientPotentialGoal(racer.Position, GoalPosition(), 10.0f));
        }
        #endregion

        #region gameloop
        /// <summary>
        /// This function examines the environment and uses potential fields to determine the AI's next direction to move in
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values.</param>
        /// <returns>Vector2 encoding the next direction this dragon should move in.</returns>
        public Vector2 GetAction(GameTime gameTime, Dictionary<Dragon, int> gates)
        {
            
            // Do not need to recalculate our direction every frame.  Just every 10 ticks.
            if ((racer.Id + gameTime.TotalGameTime.Ticks) % 10 == 0)
            {

                //Set the current gates
                currentGates = gates;

                //Process the world
                //ChangeStateIfApplicable();

                //Potential fields and computation
                if(currentGates[racer] != level.Gates.Count){
                    SelectGoal();
                }
                //MarkGoalTiles();
                //move = GetMoveAlongShortestShortestPathToAGoalTile();
                List<Vector2> potentials = new List<Vector2>();
                potentials.Add(gradientPotentialGoal(racer.Position, GoalPosition(), 1.0f)); //change radius of goal
                if (currentGates[racer] == 0)
                {
                    potentials.Add(gradientPotentialEdge(racer.Position, new Vector2(level.Width, level.Height / 2.0f), 1.0f));
                } 

                //TODO come up with a way to not loop through all objects AND detect the lava projectiles
                foreach (PlanetaryObject planet in level.Planets)
                {
                    GaseousPlanet gp = null;
                    LavaPlanet lp = null;
                    if (planet is GaseousPlanet)
                    {
                        gp = planet as GaseousPlanet;
                    }
                    if (planet is LavaPlanet)
                    {
                        lp = planet as LavaPlanet;
                    }
                    if (lp != null)
                    {
                        potentials.Add(gradientPotentialLava(racer.Position, planet.Position, planet.Radius));
                    }
                    else if (gp == null)
                    {
                        potentials.Add(gradientPotentialObstacle(racer.Position, planet.Position, planet.Radius));
                    }
                }

                Vector2 totalPotential = totalGradientPotential(potentials);
                goal = Vector2.Normalize(totalPotential);
                if (currentGates[racer] == level.Gates.Count)
                {
                    goal = new Vector2(0.0f, 0.0f);
                }
                return goal;
                //return totalPotential;
            }

            //TODO: Consider adding period fire breath to AI

            // If we're in front of a gaseous planet or a player then light it on fire
            /*if (state == FSMState.Attacking && CanShootTargetNow())
            {
                action |= ControlCode.Fire;
            }*/

            return goal;
        }
        #endregion

        #region Potential function pathfinding and other methods
        /// <summary>
        /// Acquire the next gate to travel to (and put it in field <c>goalGate</c>).
        /// </summary>
        private void SelectGoal()
        {
            goalGate = GoalGate;
        }

        /// <summary>
        /// Gets the Vector2 position of the current goal gate
        /// </summary>
        /// <returns>Vector2 the center of the gate</returns>
        public Vector2 GoalPosition()
        {
            return goalGate.Position;
        }

        /// <summary>
        /// Gets a few points around the current goal position to prevent local minima in the potential function
        /// </summary>
        /// <returns>A list of nearby points to the center of the gate</returns>
        public List<Vector2> GoalPositions()
        {
            List<Vector2> posList = new List<Vector2>();
            posList.Add(goalGate.Position);
            posList.Add(new Vector2(goalGate.Position.X+1.0f,goalGate.Position.Y));
            posList.Add(new Vector2(goalGate.Position.X, goalGate.Position.Y + 1.0f));
            posList.Add(new Vector2(goalGate.Position.X, goalGate.Position.Y - 1.0f));
            return posList;
        }

        /// <summary>
        /// Calculates the gradient of the potential generated from the edge of the map. 
        /// The potential field associated with the Goal behavior is an example of an attractive potential because the field
        /// causes the ai to be attracted to the goal (i.e., all vectors point to the goal)
        /// </summary>
        /// <param name="aiPos">The current position of this ai</param>
        /// <param name="goalPos">The current position of the goal</param>
        /// <param name="r">The radius of the goal</param>
        /// <returns></returns>
        private Vector2 gradientPotentialEdge(Vector2 aiPos, Vector2 goalPos, float r)
        {
            float alpha = 7.0f; //the scaling factor
            float gradX;
            float gradY;
            //find he distance between the goal and the ai
            float distToGoal = Vector2.Distance(aiPos, goalPos);

            //find the angle between the ai and the goal
            float theta = (float)Math.Atan2(goalPos.Y - aiPos.Y, goalPos.X - aiPos.X);

            //set gradX and gradY accordingly
            if (distToGoal < r) //ai reached the goal, so no forces from the goal act on it
            {
                gradX = 0.0f;
                gradY = 0.0f;
            }
            else if (r <= distToGoal && distToGoal <= r + 2 * r) //play with radius of goal
            {
                gradX = alpha * (distToGoal - r) * (float)Math.Cos(theta);
                gradY = alpha * (distToGoal - r) * (float)Math.Sin(theta);
            }
            else //outside of circle of extent, so gets max value
            {
                gradX = alpha * 2 * r * (float)Math.Cos(theta);
                gradY = alpha * 2 * r * (float)Math.Sin(theta);
            }
            return new Vector2(gradX, gradY);
        }

        /// <summary>
        /// Calculates the gradient of the potential generated from the goal gates. 
        /// The potential field associated with the Goal behavior is an example of an attractive potential because the field
        /// causes the ai to be attracted to the goal (i.e., all vectors point to the goal)
        /// </summary>
        /// <param name="aiPos">The current position of this ai</param>
        /// <param name="goalPos">The current position of the goal</param>
        /// <param name="r">The radius of the goal</param>
        /// <returns></returns>
        private Vector2 gradientPotentialGoal(Vector2 aiPos, Vector2 goalPos, float r)
        {
            float alpha = 5.5f; //the scaling factor
            float gradX;
            float gradY;
            //find he distance between the goal and the ai
            float distToGoal = Vector2.Distance(aiPos,goalPos);

            //find the angle between the ai and the goal
            float theta = (float)Math.Atan2(goalPos.Y-aiPos.Y,goalPos.X-aiPos.X);

            //set gradX and gradY accordingly
            if (distToGoal < r) //ai reached the goal, so no forces from the goal act on it
            {
                gradX = 0.0f;
                gradY = 0.0f;
            }
            else if (r <= distToGoal && distToGoal <= r + 2 * r) //play with radius of goal
            {
                gradX = alpha * (distToGoal - r) * (float)Math.Cos(theta);
                gradY = alpha * (distToGoal - r) * (float)Math.Sin(theta);
            }
            else //outside of circle of extent, so gets max value
            {
                gradX = alpha * 2 * r * (float)Math.Cos(theta);
                gradY = alpha * 2 * r * (float)Math.Sin(theta);
            }
            return new Vector2(gradX,gradY);
        }

        /// <summary>
        /// Calculates the gradient of the potential generated from a lava planet obstacle.
        /// The potential field associated with the ObstableAvoidance behavior is an example of rejection potential
        /// because the field causes the ai to be repelled away from the obstacle (i.e., all vector point away from the obstacle)
        /// </summary>
        /// <param name="aiPos">The current position of this ai</param>
        /// <param name="obstaclePos">The current postition of this obstacle</param>
        /// <param name="r">The radius of the obstacle</param>
        /// <returns></returns>
        private Vector2 gradientPotentialLava(Vector2 aiPos, Vector2 obstaclePos, float r)
        {
            float alpha = 2.0f; //the scaling factor
            float gradX;
            float gradY;
            //find he distance between the goal and the ai
            float distToObstacle = Vector2.Distance(aiPos, obstaclePos);

            //find the angle between the ai and the goal
            float theta = (float)Math.Atan2(obstaclePos.Y - aiPos.Y, obstaclePos.X - aiPos.X);

            //set gradX and gradY accordingly
            if (distToObstacle < r) //ai reached the goal, so no forces from the goal act on it
            {
                gradX = -Math.Sign(Math.Cos(theta)) * float.MaxValue;
                gradY = -Math.Sign(Math.Sin(theta)) * float.MaxValue;
            }
            else if (r <= distToObstacle && distToObstacle <= r + 5 * r) //play with radius of goal
            {
                gradX = -alpha * (r + (5 * r) - distToObstacle) * (float)Math.Cos(theta);
                gradY = -alpha * (r + (5 * r) - distToObstacle) * (float)Math.Sin(theta);
            }
            else //outside of circle of extent, so gets max value
            {
                gradX = 0.0f;
                gradY = 0.0f;
            }
            return new Vector2(gradX, gradY);
        }

        /// <summary>
        /// Calculates the gradient of the potential generated from an obstacle.
        /// The potential field associated with the ObstableAvoidance behavior is an example of rejection potential
        /// because the field causes the ai to be repelled away from the obstacle (i.e., all vector point away from the obstacle)
        /// </summary>
        /// <param name="aiPos">The current position of this ai</param>
        /// <param name="obstaclePos">The current postition of this obstacle</param>
        /// <param name="r">The radius of the obstacle</param>
        /// <returns></returns>
        private Vector2 gradientPotentialObstacle(Vector2 aiPos, Vector2 obstaclePos, float r)
        {
            float alpha = 1.0f; //the scaling factor
            float gradX;
            float gradY;
            //find he distance between the goal and the ai
            float distToObstacle = Vector2.Distance(aiPos, obstaclePos);

            //find the angle between the ai and the goal
            float theta = (float)Math.Atan2(obstaclePos.Y - aiPos.Y, obstaclePos.X - aiPos.X);

            //set gradX and gradY accordingly
            if (distToObstacle < r) //ai reached the goal, so no forces from the goal act on it
            {
                gradX = -Math.Sign(Math.Cos(theta))*float.MaxValue;
                gradY = -Math.Sign(Math.Sin(theta)) * float.MaxValue;
            }
            else if (r <= distToObstacle && distToObstacle <= r + 5 * r) //play with radius of goal
            {
                gradX = -alpha * (r + (5 * r) - distToObstacle) * (float)Math.Cos(theta);
                gradY = -alpha * (r + (5 * r) - distToObstacle) * (float)Math.Sin(theta);
            }
            else //outside of circle of extent, so gets max value
            {
                gradX = 0.0f;
                gradY = 0.0f;
            }
            return new Vector2(gradX, gradY);
        }

        /// <summary>
        /// Computes the overall gradient(nextDirection) for the potential field generated by the goal and obstacles
        /// To compute the overall gradient, we sum up the gradients generated from all attractive and reject potentials
        /// </summary>
        /// <param name="potentials">List of all potentials to be summed.</param>
        /// <returns></returns>
        private Vector2 totalGradientPotential(List<Vector2> potentials)
        {
            Vector2 totalPotential = Vector2.Zero;
            foreach (Vector2 potential in potentials)
            {
                totalPotential = totalPotential + potential;
            }
            return totalPotential;
        }
        #endregion


    }
}
