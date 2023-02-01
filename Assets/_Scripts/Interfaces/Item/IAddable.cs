using JetBrains.Annotations;

namespace _Scripts.Interfaces.Item
{
    public interface IAddable
    {
        void AddItem([CanBeNull] ItemSlot itemSlot, TileSlot tileSlot,out bool isAdded);
    }
}
