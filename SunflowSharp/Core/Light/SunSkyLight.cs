using System;
using SunflowSharp.Core;
using SunflowSharp.Image;
using SunflowSharp.Maths;

namespace SunflowSharp.Core.Light
{
    public class SunSkyLight : LightSource, PrimitiveList, IShader
    {
        // sunflow parameters
        private int numSkySamples;
        private OrthoNormalBasis basis;
        // parameters to the model
        private Vector3 sunDirWorld;
        private float turbidity;
        // derived quantities
        private Vector3 sunDir;
        private SpectralCurve sunSpectralRadiance;
        private Color sunColor;
        private float sunTheta;
        private double zenithY, zenithx, zenithy;
        private double[] perezY = new double[5];
        private double[] perezx = new double[5];
        private double[] perezy = new double[5];
        private float jacobian;
        private float[] colHistogram;
        private float[][] imageHistogram;
        // constant data
        private static float[] solAmplitudes = { 165.5f, 162.3f, 211.2f,
            258.8f, 258.2f, 242.3f, 267.6f, 296.6f, 305.4f, 300.6f, 306.6f,
            288.3f, 287.1f, 278.2f, 271.0f, 272.3f, 263.6f, 255.0f, 250.6f,
            253.1f, 253.5f, 251.3f, 246.3f, 241.7f, 236.8f, 232.1f, 228.2f,
            223.4f, 219.7f, 215.3f, 211.0f, 207.3f, 202.4f, 198.7f, 194.3f,
            190.7f, 186.3f, 182.6f };
        private static RegularSpectralCurve solCurve = new RegularSpectralCurve(solAmplitudes, 380, 750);
        private static float[] k_oWaveLengths = { 300, 305, 310, 315, 320,
            325, 330, 335, 340, 345, 350, 355, 445, 450, 455, 460, 465, 470,
            475, 480, 485, 490, 495, 500, 505, 510, 515, 520, 525, 530, 535,
            540, 545, 550, 555, 560, 565, 570, 575, 580, 585, 590, 595, 600,
            605, 610, 620, 630, 640, 650, 660, 670, 680, 690, 700, 710, 720,
            730, 740, 750, 760, 770, 780, 790, };
        private static float[] k_oAmplitudes = { 10.0f, 4.8f, 2.7f, 1.35f,
            .8f, .380f, .160f, .075f, .04f, .019f, .007f, .0f, .003f, .003f,
            .004f, .006f, .008f, .009f, .012f, .014f, .017f, .021f, .025f,
            .03f, .035f, .04f, .045f, .048f, .057f, .063f, .07f, .075f, .08f,
            .085f, .095f, .103f, .110f, .12f, .122f, .12f, .118f, .115f, .12f,
            .125f, .130f, .12f, .105f, .09f, .079f, .067f, .057f, .048f, .036f,
            .028f, .023f, .018f, .014f, .011f, .010f, .009f, .007f, .004f, .0f,
            .0f };
        private static float[] k_gWaveLengths = { 759, 760, 770, 771 };
        private static float[] k_gAmplitudes = { 0, 3.0f, 0.210f, 0 };
        private static float[] k_waWaveLengths = { 689, 690, 700, 710, 720,
            730, 740, 750, 760, 770, 780, 790, 800 };
        private static float[] k_waAmplitudes = { 0f, 0.160e-1f, 0.240e-1f,
            0.125e-1f, 0.100e+1f, 0.870f, 0.610e-1f, 0.100e-2f, 0.100e-4f,
            0.100e-4f, 0.600e-3f, 0.175e-1f, 0.360e-1f };

        private static IrregularSpectralCurve k_oCurve = new IrregularSpectralCurve(k_oWaveLengths, k_oAmplitudes);
        private static IrregularSpectralCurve k_gCurve = new IrregularSpectralCurve(k_gWaveLengths, k_gAmplitudes);
        private static IrregularSpectralCurve k_waCurve = new IrregularSpectralCurve(k_waWaveLengths, k_waAmplitudes);

        public SunSkyLight()
        {
            numSkySamples = 64;
            sunDirWorld = new Vector3(1, 1, 1);
            turbidity = 6;
            basis = OrthoNormalBasis.makeFromWV(new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            initSunSky();
        }

        private SpectralCurve computeAttenuatedSunlight(float theta, float turbidity)
        {
            float[] data = new float[91]; // holds the sunsky curve data
            double alpha = 1.3;
            double lozone = 0.35;
            double w = 2.0;
            double beta = 0.04608365822050 * turbidity - 0.04586025928522;
            // Relative optical mass
            double m = 1.0 / (Math.Cos(theta) + 0.000940 * Math.Pow(1.6386 - theta, -1.253));
            for (int i = 0, lambda = 350; lambda <= 800; i++, lambda += 5)
            {
                // Rayleigh scattering
                double tauR = Math.Exp(-m * 0.008735 * Math.Pow((double)lambda / 1000.0, -4.08));
                // Aerosol (water + dust) attenuation
                double tauA = Math.Exp(-m * beta * Math.Pow((double)lambda / 1000.0, -alpha));
                // Attenuation due to ozone absorption
                double tauO = Math.Exp(-m * k_oCurve.sample(lambda) * lozone);
                // Attenuation due to mixed gases absorption
                double tauG = Math.Exp(-1.41 * k_gCurve.sample(lambda) * m / Math.Pow(1.0 + 118.93 * k_gCurve.sample(lambda) * m, 0.45));
                // Attenuation due to water vapor absorption
                double tauWA = Math.Exp(-0.2385 * k_waCurve.sample(lambda) * w * m / Math.Pow(1.0 + 20.07 * k_waCurve.sample(lambda) * w * m, 0.45));
                // 100.0 comes from solAmplitudes begin in wrong units.
                double amp = /* 100.0 * */solCurve.sample(lambda) * tauR * tauA * tauO * tauG * tauWA;
                data[i] = (float)amp;
            }
            return new RegularSpectralCurve(data, 350, 800);
        }

        private double perezFunction(double[] lam, double theta, double gamma, double lvz)
        {
            double den = ((1.0 + lam[0] * Math.Exp(lam[1])) * (1.0 + lam[2] * Math.Exp(lam[3] * sunTheta) + lam[4] * Math.Cos(sunTheta) * Math.Cos(sunTheta)));
            double num = ((1.0 + lam[0] * Math.Exp(lam[1] / Math.Cos(theta))) * (1.0 + lam[2] * Math.Exp(lam[3] * gamma) + lam[4] * Math.Cos(gamma) * Math.Cos(gamma)));
            return lvz * num / den;
        }

        private void initSunSky()
        {
            // perform all the required initialization of constants
            sunDirWorld.normalize();
            sunDir = basis.untransform(sunDirWorld, new Vector3());
            sunDir.normalize();
            sunTheta = (float)Math.Acos(MathUtils.clamp(sunDir.z, -1, 1));
            if (sunDir.z > 0)
            {
                sunSpectralRadiance = computeAttenuatedSunlight(sunTheta, turbidity);
                // produce color suitable for rendering
                sunColor = RGBSpace.SRGB.convertXYZtoRGB(sunSpectralRadiance.toXYZ().mul(1e-4f)).constrainRGB();
            }
            else
            {
                sunSpectralRadiance = new ConstantSpectralCurve(0);
            }
            // sunSolidAngle = (float) (0.25 * Math.PI * 1.39 * 1.39 / (150 * 150));
            float theta2 = sunTheta * sunTheta;
            float theta3 = sunTheta * theta2;
            float T = turbidity;
            float T2 = turbidity * turbidity;
            double chi = (4.0 / 9.0 - T / 120.0) * (Math.PI - 2.0 * sunTheta);
            zenithY = (4.0453 * T - 4.9710) * Math.Tan(chi) - 0.2155 * T + 2.4192;
            zenithY *= 1000; /* conversion from kcd/m^2 to cd/m^2 */
            zenithx = (0.00165 * theta3 - 0.00374 * theta2 + 0.00208 * sunTheta + 0) * T2 + (-0.02902 * theta3 + 0.06377 * theta2 - 0.03202 * sunTheta + 0.00394) * T + (0.11693 * theta3 - 0.21196 * theta2 + 0.06052 * sunTheta + 0.25885);
            zenithy = (0.00275 * theta3 - 0.00610 * theta2 + 0.00316 * sunTheta + 0) * T2 + (-0.04212 * theta3 + 0.08970 * theta2 - 0.04153 * sunTheta + 0.00515) * T + (0.15346 * theta3 - 0.26756 * theta2 + 0.06669 * sunTheta + 0.26688);

            perezY[0] = 0.17872 * T - 1.46303;
            perezY[1] = -0.35540 * T + 0.42749;
            perezY[2] = -0.02266 * T + 5.32505;
            perezY[3] = 0.12064 * T - 2.57705;
            perezY[4] = -0.06696 * T + 0.37027;

            perezx[0] = -0.01925 * T - 0.25922;
            perezx[1] = -0.06651 * T + 0.00081;
            perezx[2] = -0.00041 * T + 0.21247;
            perezx[3] = -0.06409 * T - 0.89887;
            perezx[4] = -0.00325 * T + 0.04517;

            perezy[0] = -0.01669 * T - 0.26078;
            perezy[1] = -0.09495 * T + 0.00921;
            perezy[2] = -0.00792 * T + 0.21023;
            perezy[3] = -0.04405 * T - 1.65369;
            perezy[4] = -0.01092 * T + 0.05291;

            int w = 32, h = 32;
            imageHistogram = new float[w][];
            for (int i = 0; i < imageHistogram.Length; i++)
                imageHistogram[i] = new float[h];
            colHistogram = new float[w];
            float du = 1.0f / w;
            float dv = 1.0f / h;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    float u = (x + 0.5f) * du;
                    float v = (y + 0.5f) * dv;
                    Color c = getSkyRGB(getDirection(u, v));
                    imageHistogram[x][y] = c.getLuminance() * (float)Math.Sin(Math.PI * v);
                    if (y > 0)
                        imageHistogram[x][y] += imageHistogram[x][y - 1];
                }
                colHistogram[x] = imageHistogram[x][h - 1];
                if (x > 0)
                    colHistogram[x] += colHistogram[x - 1];
                for (int y = 0; y < h; y++)
                    imageHistogram[x][y] /= imageHistogram[x][h - 1];
            }
            for (int x = 0; x < w; x++)
                colHistogram[x] /= colHistogram[w - 1];
            jacobian = (float)(2 * Math.PI * Math.PI) / (w * h);
        }

        public bool update(ParameterList pl, SunflowAPI api)
        {
            Vector3 up = pl.getVector("up", null);
            Vector3 east = pl.getVector("east", null);
            if (up != null && east != null)
                basis = OrthoNormalBasis.makeFromWV(up, east);
            else if (up != null)
                basis = OrthoNormalBasis.makeFromW(up);
            numSkySamples = pl.getInt("samples", numSkySamples);
            sunDirWorld = pl.getVector("sundir", sunDirWorld);
            turbidity = pl.getFloat("turbidity", turbidity);
            // recompute model
            initSunSky();
            return true;
        }

        public void init(string name, SunflowAPI api)
        {
            // register this object with the api properly
            api.geometry(name, this);
            api.shader(name + ".shader", this);
            api.parameter("shaders", name + ".shader");
            api.instance(name + ".instance", name);
            api.light(name + ".light", this);
        }

        private Color getSkyRGB(Vector3 dir)
        {
            if (dir.z < 0)
                return Color.BLACK;
            if (dir.z < 0.001f)
                dir.z = 0.001f;
            dir.normalize();
            double theta = Math.Acos(MathUtils.clamp(dir.z, -1, 1));
            double gamma = Math.Acos(MathUtils.clamp(Vector3.dot(dir, sunDir), -1, 1));
            double x = perezFunction(perezx, theta, gamma, zenithx);
            double y = perezFunction(perezy, theta, gamma, zenithy);
            double Y = perezFunction(perezY, theta, gamma, zenithY) * 1e-4;
            XYZColor c = ChromaticitySpectrum.get((float)x, (float)y);
            // XYZColor c = new ChromaticitySpectrum((float) x, (float) y).toXYZ();
            float X = (float)(c.getX() * Y / c.getY());
            float Z = (float)(c.getZ() * Y / c.getY());
            return RGBSpace.SRGB.convertXYZtoRGB(X, (float)Y, Z);
        }

        public int getNumSamples()
        {
            return 1 + numSkySamples;
        }

        public void getPhoton(double randX1, double randY1, double randX2, double randY2, Point3 p, Vector3 dir, Color power)
        {
            // FIXME: not implemented
        }

        public float getPower()
        {
            return 0;
        }

        public void getSamples(ShadingState state)
        {
            if (Vector3.dot(sunDirWorld, state.getGeoNormal()) > 0 && Vector3.dot(sunDirWorld, state.getNormal()) > 0)
            {
                LightSample dest = new LightSample();
                dest.setShadowRay(new Ray(state.getPoint(), sunDirWorld));
                dest.getShadowRay().setMax(float.MaxValue);
                dest.setRadiance(sunColor, sunColor);
                dest.traceShadow(state);
                state.addSample(dest);
            }
            int n = state.getDiffuseDepth() > 0 ? 1 : numSkySamples;
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
                Vector3 localDir = getDirection(su, sv);
                Vector3 dir = basis.transform(localDir, new Vector3());
                if (Vector3.dot(dir, state.getGeoNormal()) > 0 && Vector3.dot(dir, state.getNormal()) > 0)
                {
                    LightSample dest = new LightSample();
                    dest.setShadowRay(new Ray(state.getPoint(), dir));
                    dest.getShadowRay().setMax(float.MaxValue);
                    Color radiance = getSkyRGB(localDir);
                    dest.setRadiance(radiance, radiance);
                    dest.getDiffuseRadiance().mul(invP);
                    dest.getSpecularRadiance().mul(invP);
                    dest.traceShadow(state);
                    state.addSample(dest);
                }
            }
        }

        public PrimitiveList getBakingPrimitives()
        {
            return null;
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

        public void intersectPrimitive(Ray r, int primID, IntersectionState state)
        {
            if (r.getMax() == float.PositiveInfinity)
                state.setIntersection(0, 0, 0);
        }

        public void prepareShadingState(ShadingState state)
        {
            if (state.includeLights)
                state.setShader(this);
        }

        public Color getRadiance(ShadingState state)
        {
            return getSkyRGB(basis.untransform(state.getRay().getDirection())).constrainRGB();
        }

        public void scatterPhoton(ShadingState state, Color power)
        {
            // let photon escape
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
    }
}