using System;
using SunflowSharp.Image;
using SunflowSharp.Maths;

namespace SunflowSharp.Core
{
    /**
     * This interface is used to represent any light emitting primitive. It permits
     * efficient sampling of direct illumination and photon shooting.
     */
    public interface LightSource : RenderObject
    {
        /**
         * Get the maximum number of samples that can be taken from this light
         * source. This is currently only used for statistics reporting.
         * 
         * @return maximum number of samples to be taken from this light source
         */
        int getNumSamples();

        /**
         * Samples the light source to compute direct illumination. Light samples
         * can be created using the {@link LightSample} class and added to the
         * current {@link ShadingState}. This method is responsible for the
         * shooting of shadow rays which allows for non-physical lights that don't
         * cast shadows. It is recommended that only a single shadow ray be shot if
         * {@link ShadingState#getDiffuseDepth()} is greater than 0. This avoids an
         * exponential number of shadow rays from being traced.
         * 
         * @param state current state, including point to be shaded
         * @see LightSample
         */
        void getSamples(ShadingState state);

        /**
         * Gets a photon to emit from this light source by setting each of the
         * arguments. The two sampling parameters are points on the unit square that
         * can be used to sample a position and/or direction for the emitted photon.
         * 
         * @param randX1 sampling parameter
         * @param randY1 sampling parameter
         * @param randX2 sampling parameter
         * @param randY2 sampling parameter
         * @param p position to shoot the photon from
         * @param dir direction to shoot the photon in
         * @param power power of the photon
         */
        void getPhoton(double randX1, double randY1, double randX2, double randY2, Point3 p, Vector3 dir, Color power);

        /**
         * Get the total power emitted by this light source. Lights that have 0
         * power will not emit any photons.
         * 
         * @return light source power
         */
        float getPower();
    }
}