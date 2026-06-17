using Flinty.Assets;

namespace Flinty.Player
{
    public class Inventory(PlayerEntity player)
    {
        public int SelectedIndex { get; protected set; } = 0;

        public PlayerEntity Player { get; protected set; } = player;

        public void AdvanceSelection()
        {
            SelectedIndex += 1;
            if (SelectedIndex >= BlockRegistry.VisibleRegistry.Count)
            {
                SelectedIndex = 0;
            }
        }

        public string GetSelection()
        {
            string key = BlockRegistry.VisibleRegistry.Keys.ElementAt(SelectedIndex);

            return key;
        }
    }
}