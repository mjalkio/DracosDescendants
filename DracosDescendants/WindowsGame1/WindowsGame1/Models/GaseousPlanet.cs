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
        public GaseousPlanet(Texture2D texture, Vector2 pos, float radius, float density) :
            base(texture, pos, radius, density)
        {
        }
    }
}
