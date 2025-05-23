using System;
using SunflowSharp.Core;
using SunflowSharp.Image;
using SunflowSharp.Maths;

namespace SunflowSharp.Core.Light
{
    public class ImageBasedLight : PrimitiveList, LightSource, IShader
    {
        private Texture texture;
        private OrthoNormalBasis basis;
        private int numSamples;
        private float jacobian;
        private float[] colHistogram;
        private float[][] imageHistogram;
        private Vector3[] samples;
        private Color[] colors;

        public ImageBasedLight()
        {
            texture = null;
            updateBasis(new Vector3(0, 0, -1), new Vector3(0, 1, 0));
            numSamples = 64;
        }

        private void updateBasis(Vector3 center, Vector3 up)
        {
            if (center != null && up != null)
            {
                basis = OrthoNormalBasis.makeFromWV(center, up);
                basis.swapWU();
                basis.flipV();
            }
        }

        public bool update(ParameterList pl, SunflowAPI api) {
        updateBasis(pl.getVector("center", null), pl.getVector("up", null));
        numSamples = pl.getInt("samples", numSamples);

        string filename = pl.getstring("texture", null);
        if (filename != null)
            texture = TextureCache.getTexture(api.resolveTextureFilename(filename), true);

        // no texture provided
        if (texture == null)
            return false;
        Bitmap b = texture.getBitmap();
        if (b == null)
            return false;

        // rebuild histograms if this is a new texture
        if (filename != null) {
            imageHistogram = new float[b.Width][];
            for(int i = 0;i < imageHistogram.Length;i++)
                imageHistogram[i] = new float[b.Height];
            colHistogram = new float[b.Width];
            float du = 1.0f / b.Width;
            float dv = 1.0f / b.Height;
            for (int x = 0; x < b.Width; x++) {
                for (int y = 0; y < b.Height; y++) {
                    float u = (x + 0.5f) * du;
                    float v = (y + 0.5f) * dv;
                    Color c = texture.getPixel(u, v);
                    // box filter the image
                    // c.add(texture.getPixel(u + du, v));
                    // c.add(texture.getPixel(u + du, v+ dv));
                    // c.add(texture.getPixel(u, v + dv));
                    // c.mul(0.25f);
                    imageHistogram[x][y] = c.getLuminance() * (float) Math.Sin(Math.PI * v);
                    if (y > 0)
                        imageHistogram[x][y] += imageHistogram[x][y - 1];
                }
                colHistogram[x] = imageHistogram[x][b.Height - 1];
                if (x > 0)
                    colHistogram[x] += colHistogram[x - 1];
                for (int y = 0; y < b.Height; y++)
                    imageHistogram[x][y] /= imageHistogram[x][b.Height - 1];
            }
            for (int x = 0; x < b.Width; x++)
                colHistogram[x] /= colHistogram[b.Width - 1];
            jacobian = (float) (2 * Math.PI * Math.PI) / (b.Width * b.Height);
        }
        // take fixed samples
        if (pl.getbool("fixed", samples != null)) {
            // Bitmap loc = new Bitmap(filename);
            samples = new Vector3[numSamples];
            colors = new Color[numSamples];
            for (int i = 0; i < numSamples; i++) {
                double randX = (double) i / (double) numSamples;
                double randY = QMC.halton(0, i);
                int x = 0;
                while (randX >= colHistogram[x] && x < colHistogram.Length - 1)
                    x++;
                float[] rowHistogram = imageHistogram[x];
                int y = 0;
                while (randY >= rowHistogram[y] && y < rowHistogram.Length - 1)
                    y++;
                // sample from (x, y)
                float u = (float) ((x == 0) ? (randX / colHistogram[0]) : ((randX - colHistogram[x - 1]) / (colHistogram[x] - colHistogram[x - 1])));
                float v = (float) ((y == 0) ? (randY / rowHistogram[0]) : ((randY - rowHistogram[y - 1]) / (rowHistogram[y] - rowHistogram[y - 1])));

                float px = ((x == 0) ? colHistogram[0] : (colHistogram[x] - colHistogram[x - 1]));
                float py = ((y == 0) ? rowHistogram[0] : (rowHistogram[y] - rowHistogram[y - 1]));

                float su = (x + u) / colHistogram.Length;
                float sv = (y + v) / rowHistogram.Length;

                float invP = (float) Math.Sin(sv * Math.PI) * jacobian / (numSamples * px * py);
                samples[i] = getDirection(su, sv);
                basis.transform(samples[i]);
                colors[i] = texture.getPixel(su, sv).mul(invP);
                // loc.setPixel(x, y, Color.YELLOW.copy().mul(1e6f));
            }
            // loc.save("samples.hdr");
        } else {
            // turn off
            samples = null;
            colors = null;
        }
        return true;
    }

        public void init(string name, SunflowAPI api)
        {
            // register this object with the api properly
            api.geometry(name, this);
            if (api.lookupGeometry(name) == null)
            {
                // quit if we don't see our geometry in here (error message
                // will have already been printed)
                return;
            }
            api.shader(name + ".shader", this);
            api.parameter("shaders", name + ".shader");
            api.instance(name + ".instance", name);
            api.light(name + ".light", this);
        }

        public void prepareShadingState(ShadingState state)
        {
            if (state.includeLights)
                state.setShader(this);
        }

        public void intersectPrimitive(Ray r, int primID, IntersectionState state)
        {
            if (r.getMax() == float.PositiveInfinity)
                state.setIntersection(0, 0, 0);
        }

        public int getNumPrimitives()
        {
            return 1;
        }

        public float getPrimitiveBound(int primID, int i)
        {
            return 0;
        }

        public BoundingBox getWorldBounds(Matrix4 o2w)
        {
            return null;
        }

        public PrimitiveList getBakingPrimitives()
        {
            return null;
        }

        public int getNumSamples()
        {
            return numSamples;
        }

        public void getSamples(ShadingState state)
        {
            if (samples == null)
            {
                int n = state.getDiffuseDepth() > 0 ? 1 : numSamples;
                for (int i = 0; i < n; i++)
                {
                    // random offset on unit square, we use the infinite version of
                    // getRandom because the light sampling is adaptive
                    double randX = state.getRandom(i, 0, n);
                    double randY = state.getRandom(i, 1, n);
                    int x = 0;
                    while (randX >= colHistogram[x] && x < colHistogram.Length - 1)
                        x++;
                    float[] rowHistogram = imageHistogram[x];
                    int y = 0;
                    while (randY >= rowHistogram[y] && y < rowHistogram.Length - 1)
                        y++;
                    // sample from (x, y)
                    float u = (float)((x == 0) ? (randX / colHistogram[0]) : ((randX - colHistogram[x - 1]) / (colHistogram[x] - colHistogram[x - 1])));
                    float v = (float)((y == 0) ? (randY / rowHistogram[0]) : ((randY - rowHistogram[y - 1]) / (rowHistogram[y] - rowHistogram[y - 1])));

                    float px = ((x == 0) ? colHistogram[0] : (colHistogram[x] - colHistogram[x - 1]));
                    float py = ((y == 0) ? rowHistogram[0] : (rowHistogram[y] - rowHistogram[y - 1]));

                    float su = (x + u) / colHistogram.Length;
                    float sv = (y + v) / rowHistogram.Length;
                    float invP = (float)Math.Sin(sv * Math.PI) * jacobian / (n * px * py);
                    Vector3 dir = getDirection(su, sv);
                    basis.transform(dir);
                    if (Vector3.dot(dir, state.getGeoNormal()) > 0)
                    {
                        LightSample dest = new LightSample();
                        dest.setShadowRay(new Ray(state.getPoint(), dir));
                        dest.getShadowRay().setMax(float.MaxValue);
                        Color radiance = texture.getPixel(su, sv);
                        dest.setRadiance(radiance, radiance);
                        dest.getDiffuseRadiance().mul(invP);
                        dest.getSpecularRadiance().mul(invP);
                        dest.traceShadow(state);
                        state.addSample(dest);
                    }
                }
            }
            else
            {
                for (int i = 0; i < numSamples; i++)
                {
                    if (Vector3.dot(samples[i], state.getGeoNormal()) > 0 && Vector3.dot(samples[i], state.getNormal()) > 0)
                    {
                        LightSample dest = new LightSample();
                        dest.setShadowRay(new Ray(state.getPoint(), samples[i]));
                        dest.getShadowRay().setMax(float.MaxValue);
                        dest.setRadiance(colors[i], colors[i]);
                        dest.traceShadow(state);
                        state.addSample(dest);
                    }
                }
            }
        }

        public void getPhoton(double randX1, double randY1, double randX2, double randY2, Point3 p, Vector3 dir, Color power)
        {
        }

        public Color getRadiance(ShadingState state)
        {
            // lookup texture based on ray direction
            return state.includeLights ? getColor(basis.untransform(state.getRay().getDirection(), new Vector3())) : Color.BLACK;
        }

        private Color getColor(Vector3 dir)
        {
            float u, v;
            // assume lon/lat format
            double phi = 0, theta = 0;
            phi = Math.Acos(dir.y);
            theta = Math.Atan2(dir.z, dir.x);
            u = (float)(0.5 - 0.5 * theta / Math.PI);
            v = (float)(phi / Math.PI);
            return texture.getPixel(u, v);
        }

        private Vector3 getDirection(float u, float v)
        {
            Vector3 dest = new Vector3();
            double phi = 0, theta = 0;
            theta = u * 2 * Math.PI;
            phi = v * Math.PI;
            double sin_phi = Math.Sin(phi);
            dest.x = (float)(-sin_phi * Math.Cos(theta));
            dest.y = (float)Math.Cos(phi);
            dest.z = (float)(sin_phi * Math.Sin(theta));
            return dest;
        }

        public void scatterPhoton(ShadingState state, Color power)
        {
        }

        public float getPower()
        {
            return 0;
        }
    }
}