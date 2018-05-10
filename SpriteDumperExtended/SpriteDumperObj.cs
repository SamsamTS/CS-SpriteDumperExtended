using System;
using ColossalFramework.UI;
using UnityEngine;

namespace SpriteDumperExtended
{
    public class SpriteDumperObj : MonoBehaviour
    {
        private string spritePathBase = "UISprites\\";
        /*private int spritesPerPage = 100;
        private string wikiTemplate = "" +
                ".. WARNING FOR CONTRIBUTORS: Don't modify this file! It's generated with a mod (see below) and all changes made will be lost with the next update.\r\n\r\n" +
                "==========\r\n" +
                "UI Sprites {PAGE}/{TOTALPAGES}\r\n" +
                "==========\r\n" +
                "Here you can find a list of UI sprites.\r\n" + 
                "These are all the sprites available in the game.\r\n" + 
                "You can use these sprites in many of the UI components that have a sprite property for example a button, panel, slider and so on\r\n" +
                "\r\n\r\n {SPRITES} \r\n" +
                "About this page\r\n" +
                "---------------\r\n" +
                "This wiki page was created in game with the `SpriteDumper mod <https://github.com/worstboy32/SpriteDumper>`__ .\r\n" +
                "To modify the text in this document please create a PR on the mod on github.\r\n" +
                "If there are sprites missing you can run the mod and create a PR on the docs repo with the new generated file.\r\n" +
                "\r\n" +
                "Kudos to `Permutation <http://www.skylinesmodding.com/users/permutation/>`__ for sharing the method for dumping sprites.\r\n";*/

        /*void OnEnable()
        {
            Utils.Log("SpriteDumperObj created!");
        }*/

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
            {
                //SimulationManager.instance.AddAction(delegate
                //{
                    Dump();
                //});
            }
        }

        private void Dump()
        {
            Utils.Log("Dumping UI sprites to your disk...");
            Utils.DeleteDir(spritePathBase);

            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++)
            {
                if (atlases[i] == null || atlases[i].name == null || atlases[i].name.Length == 0) continue;

                try
                {
                    string spritePath = spritePathBase + atlases[i].name;

                    if (Utils.CreateDir(spritePath))
                    {
                        Utils.Log("Directory for sprites created at: '" + spritePath + "'");
                    }

                    //System.Collections.Generic.List<UITextureAtlas.SpriteInfo> spritelist = UIView.GetAView().defaultAtlas.sprites;
                    System.Collections.Generic.List<UITextureAtlas.SpriteInfo> spritelist = atlases[i].sprites;

                    int count = 0;

                    //wikiTemplate = wikiTemplate.Replace("{TOTALPAGES}", Mathf.CeilToInt(spritelist.Count / spritesPerPage).ToString());

                    /*string spriteStr = "";
                    int index = 0;
                    int page = 1;*/
                    foreach (UITextureAtlas.SpriteInfo sprite in spritelist)
                    {
                        try
                        {
                            if (sprite.texture != null)
                            {
                                byte[] pngbytes = sprite.texture.EncodeToPNG();
                                String filename = Utils.MakeValidFileName(sprite.name);
                                System.IO.File.WriteAllBytes(spritePath + "\\" + filename + ".png", pngbytes);
                                count++;
                            }
                        }
                        catch (UnityException)
                        {
                            // Locked texture workaround
                            Texture2D texture2D = sprite.texture;

                            if (texture2D != null)
                            {
                                RenderTexture renderTexture = RenderTexture.GetTemporary(texture2D.width, texture2D.height, 0);
                                Graphics.Blit(texture2D, renderTexture);

                                RenderTexture active = RenderTexture.active;
                                texture2D = new Texture2D(renderTexture.width, renderTexture.height);
                                RenderTexture.active = renderTexture;
                                texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
                                texture2D.Apply();
                                RenderTexture.active = active;

                                RenderTexture.ReleaseTemporary(renderTexture);

                                byte[] pngbytes = texture2D.EncodeToPNG();
                                String filename = Utils.MakeValidFileName(sprite.name);
                                System.IO.File.WriteAllBytes(spritePath + "\\" + filename + ".png", pngbytes);
                                count++;

                                Utils.Log("Workaround locked texture '" + sprite.name + "'!!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.LogError("Failed dumping sprite '" + sprite.name + "'!!");
                            Utils.LogError(ex.ToString());
                        }

                        /*spriteStr += ".. figure:: /_static/UISprites/" + sprite.name + ".png\r\n" +
                            "    :alt: " + sprite.name + "\r\n\r\n" +
                            "**" + sprite.name + "**\r\n\r\n";

                        index++;
                        if (index > spritesPerPage)
                        {
                            string wikiPage = wikiTemplate;
                            wikiPage = wikiPage.Replace("{PAGE}", page + "");
                            System.IO.StreamWriter file = new System.IO.StreamWriter("UISprites\\" + "UI-Sprites-" + page + ".rst");
                            file.WriteLine(wikiPage.Replace("{SPRITES}", spriteStr));
                            file.Close();

                            index = 0;
                            spriteStr = "";
                            page++;
                        }*/
                    }

                    Utils.Log("Dumped " + count + " sprites to '" + spritePath + "'");
                }
                catch
                { }
            }

            Utils.Log("Dumping done.");
        }
    }
}
