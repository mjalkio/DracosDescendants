#region File Description
//-----------------------------------------------------------------------------
// LavaPlanet.cs
//
// Class is a model for a LavaPlanet.  LavaPlanets can occasionally shoot 
// solar flares, so this class needs to track that.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DracosD.Models
{
    class LavaPlanet : PlanetaryObject
    {
        private const float DEFAULT_DENSITY = 5.0f;
        private const float DEFAULT_FRICTION = 0.0f;
        private const float DEFAULT_RESTITUTION = 0.0f;

        public LavaPlanet(Texture2D texture, Vector2 pos, float radius) :
            base(texture, pos, radius, DEFAULT_DENSITY, DEFAULT_FRICTION, DEFAULT_RESTITUTION)
        {
        }
    }
}
