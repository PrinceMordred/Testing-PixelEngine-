using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing__PixelEngine_
{
    public abstract class Element
    {
        public virtual void Step() { }
    }
    public abstract class Liquid : Element
    {

    }
    public abstract class Solid: Element
    {

    }
    public abstract class Gas : Element
    {

    }
    public abstract class ImmoveableSolid : Solid
    {

    }
    public abstract class MoveableSolid : Solid
    {
        public override void Step()
        {
            Element ElemUnder = 
            if (ElemUnder is null)
            {
                
            }
        }
    }

    public class Sand : MoveableSolid
    {
        
    }

}
