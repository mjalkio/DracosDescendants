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
        public LavaPlanet(Texture2D texture, Vector2 pos, float radius, float density) :
            base(texture, pos, radius, density)
        {
        }
    }
}
