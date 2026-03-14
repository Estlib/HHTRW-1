using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public class Block
    {
        //this is data for block, it needs:

        //sprites or list of sprites
        //is it animated or not
        //type - in the current version, its a tag like "ground" air and whatnot
        //currentstate - solid or notsolid, it is for platforms that are not solid when player is not higher
        //solidstate - yes or no, this is instantiated at level rendertime
        //is it destructable
        //health of block
        //does it block bullets
        //does it stop enemies
        //list of enemies that it stops
        //is it square or slope?
        //slopetype, is enum, can be: 16,32top,32bottom,64hightop,64midtop,64midbottom,64lowbase, custom,
        //is the slope flipped horizontally
        //is the slope flipped vertically
    }
    //has constructors for combinations of properties, added to the class as needed
}
