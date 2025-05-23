using System;
using SunflowSharp.Core.Primitive;
using SunflowSharp.Maths;

namespace SunflowSharp.Core
{

    /**
     * Represents an object which can be tesselated into a list of primitives such
     * as a {@link TriangleMesh}.
     */
    public interface ITesselatable : RenderObject
    {
        /**
         * Tesselate this object into a {@link PrimitiveList}. This may return
         * <code>null</code> if tesselation fails.
         * 
         * @return a list of primitives generated by the tesselation
         */
        PrimitiveList tesselate();

        /**
         * Compute a bounding box of this object in world space, using the specified
         * object-to-world transformation matrix. The bounds should be as exact as
         * possible, if they are difficult or expensive to compute exactly, you may
         * use {@link Matrix4#transform(BoundingBox)}. If the matrix is
         * <code>null</code> no transformation is needed, and object space is
         * equivalent to world space. This method may return <code>null</code> if
         * these bounds are difficult or impossible to compute, in which case the
         * tesselation will be executed right away and the bounds of the resulting
         * primitives will be used.
         * 
         * @param o2w object to world transformation matrix
         * @return object bounding box in world space
         */
        BoundingBox getWorldBounds(Matrix4 o2w);
    }
}