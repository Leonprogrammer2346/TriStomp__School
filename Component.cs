using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class Component
    {
        public virtual void Update(float dt, KeyboardState input) 
        {
         
        }

        public virtual Component Clone(GameObject newObj)
        {
        return null;
        }
    }

