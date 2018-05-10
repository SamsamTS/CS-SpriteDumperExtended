using ICities;
using UnityEngine;

namespace SpriteDumperExtended
{
    public class SpriteDumper : LoadingExtensionBase, IUserMod
    {
        private GameObject gameObj;

        public string Name
        {
            get { return "Sprite Dumper Extended"; }
        }

        public string Description
        {
            get { return "Dump sprites (CTRL + SHIFT + D)"; } //and wiki page for http://docs.skylinesmodding.com"; }
        }


        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            if (gameObj == null)
            {
                gameObj = new GameObject("SpriteDumper");
                gameObj.AddComponent<SpriteDumperObj>();
            }
            Utils.Log("Loaded");
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            GameObject.Destroy(gameObj);
        }

    }
}
