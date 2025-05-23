using System;
using SunflowSharp.Core;
using SunflowSharp.Image;
using SunflowSharp.Maths;

namespace SunflowSharp.Core.Shader
{
    public class DiffuseShader : IShader
    {
        private Color diff;

        public DiffuseShader()
        {
            diff = Color.WHITE;
        }

        public virtual bool update(ParameterList pl, SunflowAPI api)
        {
            diff = pl.getColor("diffuse", diff);
            return true;
        }

        public virtual Color getDiffuse(ShadingState state)
        {
            return diff;
        }

        public Color getRadiance(ShadingState state)
        {
            // make sure we are on the right side of the material
            state.faceforward();
            // setup lighting
            state.initLightSamples();
            state.initCausticSamples();
            return state.diffuse(getDiffuse(state));
        }

        public void scatterPhoton(ShadingState state, Color power)
        {
            Color diffuse;
            // make sure we are on the right side of the material
            if (Vector3.dot(state.getNormal(), state.getRay().getDirection()) > 0.0)
            {
                state.getNormal().negate();
                state.getGeoNormal().negate();
            }
            diffuse = getDiffuse(state);
            state.storePhoton(state.getRay().getDirection(), power, diffuse);
            float avg = diffuse.getAverage();
            double rnd = state.getRandom(0, 0, 1);
            if (rnd < avg)
            {
                // photon is scattered
                power.mul(diffuse).mul(1.0f / avg);
                OrthoNormalBasis onb = state.getBasis();
                double u = 2 * Math.PI * rnd / avg;
                double v = state.getRandom(0, 1, 1);
                float s = (float)Math.Sqrt(v);
                float s1 = (float)Math.Sqrt(1.0 - v);
                Vector3 w = new Vector3((float)Math.Cos(u) * s, (float)Math.Sin(u) * s, s1);
                w = onb.transform(w, new Vector3());
                state.traceDiffusePhoton(new Ray(state.getPoint(), w), power);
            }
        }
    }
}