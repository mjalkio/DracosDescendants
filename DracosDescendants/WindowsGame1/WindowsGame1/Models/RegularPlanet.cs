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
        public RegularPlanet(Texture2D texture, Vector2 pos, float radius, float density) :
            base(texture, pos, radius, density)
        {
        }
    }
}
