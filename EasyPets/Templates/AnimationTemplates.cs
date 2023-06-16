using RogueGenesia.Data;
using UnityEngine;
using Paths = System.IO.Path;

namespace EasyPets.EasyPets.Templates
{
    public class AnimationTemplate
    {
        public string Path;
        public int Rows;
        public int FramesPerRow;

        public Result<PixelAnimationData> ToPixelAnimationData(string assetBasePath)
        {
            var fullPath = Paths.Combine(assetBasePath, Path);
            var texture = ModGenesia.ModGenesia.LoadPNGTexture(fullPath);

            if (texture == null)
            {
                return Results.Fail<PixelAnimationData>($"Unable to load texture from path {fullPath}");
            }

            if (FramesPerRow == 0)
            {
                return Results.Fail<PixelAnimationData>("FramesPerRow can not be 0");
            }

            if (Rows == 0)
            {
                return Results.Fail<PixelAnimationData>("Rows can not be 0");
            }

            var frames = new Vector2Int(FramesPerRow, Rows);

            return Results.Ok(new PixelAnimationData
            {
                Frames = frames,
                Texture = texture
            });
        }
    }
}