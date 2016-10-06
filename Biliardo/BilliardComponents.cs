using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Biliardo
{
    public class BilliardComponents
    {
        protected Texture2D Texture;        
        protected SpriteBatch SpriteBatch;
        protected Rectangle where;

        protected BilliardComponents() { }

        //The texture can be only loaded, 
        //The spriteBatch will be the same object for all components
        //The position will be updated so it need to be accessible
        protected BilliardComponents(SpriteBatch spriteBatch, Texture2D texture)
        {
            Texture = texture;
            SpriteBatch = spriteBatch;
            where = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        //Every child object gives a personal interpretation of the rectangle, depending on their position
        virtual public void Draw()
        {
            SpriteBatch.Draw(Texture, where, Color.White);
        }
    }
}
