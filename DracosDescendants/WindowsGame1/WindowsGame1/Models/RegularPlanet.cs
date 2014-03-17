#region File Description
//-----------------------------------------------------------------------------
// RegularPlanet.cs
//
// Most basic form of a planet.  Implements no new functionality from 
// PlanetaryObject, it only allows it to be instantiated.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DracosD.Models
{
    class RegularPlanet : PlanetaryObject
    {
        private const float DEFAULT_DENSITY = 8.0f;
        private const float DEFAULT_FRICTION = 0.6f;
        private const float DEFAULT_RESTITUTION = 0.5f;

        public RegularPlanet(Texture2D texture, Vector2 pos, float radius) :
            base(texture, pos, radius, DEFAULT_DENSITY, DEFAULT_FRICTION, DEFAULT_RESTITUTION)
        {
        }
    }
}
