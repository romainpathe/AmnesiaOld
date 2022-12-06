namespace Amnesia.interfaces
{
    public interface IDrawable
    {
        /// <summary>
        /// Draw the object in console
        /// </summary>
        void Draw();
        /// <summary>
        /// Clear the object from console
        /// </summary>
        /// <param name="full">If i need clear all object or only ce value of this object (Use with card)</param>
        void Clear(bool full);
    }
}