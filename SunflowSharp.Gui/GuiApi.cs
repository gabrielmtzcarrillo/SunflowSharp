using System;
using SunflowSharp.Core;
using SunflowSharp.Core.Shader;

namespace SunflowSharp.Gui
{
    public class GuiApi : SunflowAPI
    {
        private readonly string sc;

        public GuiApi(string sc)
        {
            this.sc = sc;
        }

        public override void build()
        {
            parameter("width", (float)(Math.PI * 0.5 / 8192));
            //shader("ao_wire", new Wire());
            // you can put the path to your own scene here to use this rendering technique
            // just copy this file to the same directory as your main .sc file, and swap
            // the filename in the line below
            parse(sc != null ? sc : "gumbo_and_teapot.sc.gz");
            shaderOverride("ao_wire", true);

            // this may need to be tweaked if you want really fine lines
            // this is higher than most scenes need so if you render with ambocc = false, make sure you turn down
            // the sampling rates of dof/lights/gi/reflections accordingly
            parameter("aa.min", 0);
            parameter("aa.max", 0);
            parameter("filter", "catmull-rom");
            parameter("sampler", "bucket");
            options(DEFAULT_OPTIONS);
        }

        public class Wire : WireframeShader
        {
            public bool ambocc = true;

            public override SunflowSharp.Image.Color getFillColor(ShadingState state)
            {
                return ambocc ? state.occlusion(16, 6.0f) : state.getShader().getRadiance(state);
            }
        }
    }
}
