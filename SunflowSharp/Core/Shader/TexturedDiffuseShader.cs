using System;
using SunflowSharp.Core;
using SunflowSharp.Image;

namespace SunflowSharp.Core.Shader
{

    public class TexturedDiffuseShader : DiffuseShader
    {
        private Texture tex;

        public TexturedDiffuseShader()
        {
            tex = null;
        }

        public override bool update(ParameterList pl, SunflowAPI api)
        {
            string filename = pl.getstring("texture", null);
            if (filename != null)
                tex = TextureCache.getTexture(api.resolveTextureFilename(filename), false);
            return tex != null && base.update(pl, api);
        }

        public override Color getDiffuse(ShadingState state)
        {
            return tex.getPixel(state.getUV().x, state.getUV().y);
        }
    }
}