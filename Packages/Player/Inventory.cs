using Flinty.Assets;

namespace Flinty.Player
{
    public class Inventory(PlayerNode player)
    {
        public int SelectedIndex { get; protected set; } = 0;

        public PlayerNode Player { get; protected set; } = player;

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
            string key = BlockRegistry.VisibleRegistry.ElementAt(SelectedIndex);

            return key;
        }

        public void SetSelection(string o)
        {
            var registry = BlockRegistry.VisibleRegistry;

            int index = registry.IndexOf(o);

            if (index > -1)
            {
                SelectedIndex = index;
            }
        }
    }
}