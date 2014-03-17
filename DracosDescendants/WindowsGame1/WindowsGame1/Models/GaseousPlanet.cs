#region File Description
//-----------------------------------------------------------------------------
// GaseousPlanet.cs
//
// Class is a model for a GaseousPlanet.  Gaseous planets can be lit on fire, 
// so this class needs to add that functionality to PlanetaryObject.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DracosD.Models
{
    class GaseousPlanet : PlanetaryObject
    {
        private const float DEFAULT_DENSITY = 2.0f;
        private const float DEFAULT_FRICTION = 0.0f;
        private const float DEFAULT_RESTITUTION = 0.0f;

        public GaseousPlanet(Texture2D texture, Vector2 pos, float radius) :
            base(texture, pos, radius, DEFAULT_DENSITY, DEFAULT_FRICTION, DEFAULT_RESTITUTION)
        {
        }
    }
}
