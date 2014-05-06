#region File Description
//-----------------------------------------------------------------------------
// PolygonObject.cs
//
// Arbitrary polygonal-shaped model to support collisions.
//
// Sometimes boxes and shapes do not cut it.  In that case, you have to bring
// out the polygons.  This class is substantially more complex than the other
// physics objects, but it will allow you to draw arbitrary shapes.
//
// Be careful with modifying this file.  Even if you have had computer
// graphics, there are A LOT of subtleties in handling the physics of
// polygons.
//
// Author: Walker M. White
// Based on original PhysicsDemo Lab by Don Holden, 2007
// MonoGame version, 2/14/2014
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DracosD.Views;
#endregion

/// <summary>
/// Subnamespace for Farseer-aware models.
/// </summary>
namespace DracosD.Objects {

    /// <summary>
    /// An arbitrary polygonal object (need not be convex)
    /// </summary>
    /// <remarks>
    /// The user has no control over the texture coordinates.
    /// The texture corespond to shape coordinates, but with
    /// a mapping adjusted by the TextureScale attribute.
    /// </remarks>
    public class PolygonObject : SimplePhysicsObject {

    #region Constants
        // Tolerance factor
        private const float EPSILON = 0.001f;
    #endregion

    #region Fields
        // Geometry triangles and the drawable vertices
        protected List<Vector2[]> triangles;
        protected VertexPositionTexture[] vertices;

        // Additional physics object for geometry.
        protected LinkedList<Fixture> fixtures = new LinkedList<Fixture>();

        // buffer variables for creation.
        protected Vector2 dimension;
        protected Vector2 sizeScale;
        protected Vector2 drawScale;
    #endregion
    
    #region Properties (READ-WRITE)
        /// <summary>
        /// Bounding box dimensions of this shape
        /// </summary>
        /// <remarks>
        /// Reassigning this value with stretch or squeeze the vertices uniformly
        /// to fit in the bounding box. All changes are buffered until Update is called.
        /// </remarks>
        public Vector2 Dimension {
            get { return dimension; }
            set {
                Debug.Assert(value.X > 0 && value.Y > 0, "Dimension attributes must be > 0");
                isDirty = true;
                sizeScale = value / dimension;
                dimension = value;
            }
        }

        public LinkedList<Fixture> Fixtures
        {
            get { return fixtures; }
        }

        /// <summary>
        /// Width of the polygon bounding box
        /// </summary>
        /// <remarks>
        /// Reassigning this value with stretch or squeeze the vertices uniformly
        /// to fit in the bounding box. All changes are buffered until Update is called.
        /// </remarks>
        public float Width {
            get { return dimension.X; }
            set {
                Debug.Assert(value > 0, "Width must be > 0");
                sizeScale.X = value / dimension.X;
                dimension.X = value;
                isDirty = true;
            }
        }

        /// <summary>
        /// Height of the polygon bounding box
        /// </summary>
        /// <remarks>
        /// Reassigning this value with stretch or squeeze the vertices uniformly
        /// to fit in the bounding box. All changes are buffered until Update is called.
        /// </remarks>
        public float Height {
            get { return dimension.Y; }
            set {
                Debug.Assert(value > 0, "Height must be > 0");
                sizeScale.Y = value / dimension.Y;
                dimension.Y = value;
                isDirty = true;
            }
        }

        /// <summary>
        /// Scale to map the texture onto vertices
        /// </summary>
        /// <remarks>
        /// The user has no control over the texture coordinates.
        /// The texture corespond to shape coordinates, but with
        /// a mapping adjusted by this property.
        /// </remarks>
        public Vector2 TextureScale {
            get { return drawScale; }
            set { drawScale = value; }
        }
    #endregion

    #region Shape Initialization
        /// <summary>
        /// Create a (not necessarily convex) polygon
        /// </summary>
        /// <param name="texture">Object image texture</param>
        /// <param name="points">Polygon vertices</param>
        public PolygonObject(Texture2D texture, Vector2[] points)
            : this(texture, points, Vector2.One) { }

        /// <summary>
        /// Create a (not necessarily convex) polygon
        /// </summary>
        /// <param name="texture">Object image texture</param>
        /// <param name="points">Polygon vertices</param>
        /// <param name="scale">Scale to map the texture onto vertices</param>
        public PolygonObject(Texture2D texture, Vector2[] points, Vector2 scale) : 
            base(texture, Vector2.Zero) {
            Debug.Assert(points.Length >= 3, "Polygon is degenerate");

            // Initialize and build triangles.
            this.texture = texture;
            dimension = new Vector2();
            sizeScale = Vector2.One;
            drawScale = scale;
            drawState = DrawState.PolygonPass;
            BuildTriangles(points);
        }

        /// <summary>
        /// Triangulate the polygon so that it is a collection of convex bodies.
        /// </summary>
        /// <remarks>
        /// While this polygonal body does not have to be convex, convexity is
        /// a Box-2D requirement. We use a divide-and-conquer algorithm
        /// to triangulate the polygon, since triangles are guaranteed to be 
        /// convex, a Box2D requirement.  Colinear consecutive points are okay, 
        /// but the polygon must be simple (i.e. no crossing edges).
        /// </remarks>
        /// <param name="points">Polygon vertices</param>
        private void BuildTriangles(Vector2[] points) {
            // Triangles generated
            triangles = new List<Vector2[]>();
            LinkedList<int> polygon = new LinkedList<int>();

            // Make polygon list and initialize width, height
            Vector2 min = points[0];
            Vector2 max = points[0];
            Vector2 avg = new Vector2();
            for (int ii = 0; ii < points.Length; ii++) {
                polygon.AddLast(ii);
                avg += points[ii];
                if (points[ii].X < min.X) { min.X = points[ii].X; }
                if (points[ii].X > max.X) { max.X = points[ii].X; }
                if (points[ii].Y < min.Y) { min.Y = points[ii].Y; }
                if (points[ii].Y > min.Y) { max.Y = points[ii].Y; }
            }
            position = avg / points.Length;
            dimension = max - min;

            Split(polygon, points, triangles);
            foreach (Vector2[] triangle in triangles) {
                for (int ii = 0; ii < triangle.Length; ii++) {
                    triangle[ii] -= position;
                }
            }
            CreateDrawable(triangles);
        }

        /// <summary>
        /// Recursive helper method for BuildTriangles
        /// </summary>
        /// <param name="polygon">Indices identifying convex polygons</param>
        /// <param name="points">Polygon vertices</param>
        /// <param name="triangles">Triangles created so far</param>
        private void Split(LinkedList<int> polygon, Vector2[] points, List<Vector2[]> triangles) {
            // It's a triangle - make it and terminate recursion
            if (polygon.Count == 3) {
                Vector2[] v = new Vector2[3];
                v[0] = points[polygon.First.Value];
                v[1] = points[polygon.First.Next.Value];
                v[2] = points[polygon.First.Next.Next.Value];
                triangles.Add(v);
                return;
            }

            float smallestColinearity = float.MaxValue; // Colinearity measure for best diagonal
            LinkedListNode<int> diag1 = null; // Points in the diagonal
            LinkedListNode<int> diag2 = null;

            // Look at all diagonals in the polygon
            for (LinkedListNode<int> i1 = polygon.First; i1.Next != null; i1 = i1.Next) {
                Vector3 testEdge1 = new Vector3(points[i1.Next.Value] - points[i1.Value], 0);
                LinkedListNode<int> prev = i1.Previous;
                if (prev == null) {
                    prev = polygon.Last;
                }
                Vector3 testEdge2 = new Vector3(points[prev.Value] - points[i1.Value], 0);

                for (LinkedListNode<int> i2 = i1.Next.Next; i2 != null; i2 = i2.Next) {
                    Vector2 p1 = points[i1.Value];
                    Vector2 p2 = points[i2.Value];

                    // Indicates that the segment travels through
                    // some empty space, so it's not a diagonal edge
                    Vector3 edge = new Vector3(p2 - p1, 0);
                    if (Vector3.Cross(testEdge1, edge).Z <= 0)
                        continue;
                    if (Vector3.Cross(edge, testEdge2).Z <= 0)
                        continue;

                    // If no polygon edge intersects our edge, 
                    // then our edge is guaranteed to be a diagonal
                    // find out whether or not this is a diagonal
                    bool isDiagonal = true;
                    for (LinkedListNode<int> i3 = polygon.First; isDiagonal && i3 != null; i3 = i3.Next) {
                        LinkedListNode<int> i4 = i3.Next;
                        if (i4 == null) {
                            i4 = polygon.First;
                        }

                        // Ignore edges eminating from one of our vertices
                        if (i3 == i1 || i3 == i2 || i4 == i1 || i4 == i2) {
                            continue;
                        }

                        Vector2 p3 = points[i3.Value];
                        Vector2 p4 = points[i4.Value];

                        // Test for intersection
                        Vector2 d1 = p2 - p1;
                        Vector2 d2 = p4 - p3;

                        Vector2 b = p3 - p1;
                        float d = d1.X*d2.Y - d2.X*d1.Y;
                        if (Math.Abs(d) < EPSILON) {
                            continue;
                        }
                        d = 1.0f/d;

                        float t = d2.Y*b.X - d2.X*b.Y;
                        float s = d1.Y*b.X - d1.X*b.Y;
                        t *= d;
                        s *= d;

                        // Just found an intersection - this is not a diagonal!
                        if (s > 0 && s < 1 && t > 0 && t < 1) {
                            isDiagonal = false;
                        }
                    }

                    // Don't consider it if it's not a diagonal
                    if (!isDiagonal) {
                        continue;
                    }

                    Vector2 dir = p1 - p2;
                    dir.Normalize();

                    // Measure colinearity with nearby points
                    float ourColinearity = 0;

                    LinkedListNode<int> t1 = i1.Previous;
                    if (t1 == null) t1 = polygon.Last;
                    ourColinearity = Math.Max(ourColinearity, Math.Abs(GetColinearity(points[t1.Value], p1, dir)));

                    LinkedListNode<int> t2 = i1.Next;
                    if (t2 == null) t2 = polygon.First;
                    ourColinearity = Math.Max(ourColinearity, GetColinearity(points[t2.Value], p1, dir));

                    LinkedListNode<int> t3 = i2.Previous;
                    if (t3 == null) t3 = polygon.Last;
                    ourColinearity = Math.Max(ourColinearity, GetColinearity(points[t3.Value], p2, dir));

                    LinkedListNode<int> t4 = i2.Next;
                    if (t4 == null) t4 = polygon.First;
                    ourColinearity = Math.Max(ourColinearity, GetColinearity(points[t4.Value], p2, dir));

                    // If our colinearity is smaller than our best bet,
                    // make us into the new best bet!
                    if (ourColinearity < smallestColinearity) {
                        smallestColinearity = ourColinearity;
                        diag1 = i1;
                        diag2 = i2;
                    }
                }
            }

            Debug.Assert(diag1 != null);

            LinkedList<int> left = new LinkedList<int>();
            for (LinkedListNode<int> i = diag1; i != diag2; i = (i.Next != null) ? i.Next : polygon.First) {
                left.AddLast(i.Value);
            }
            left.AddLast(diag2.Value);

            LinkedList<int> right = new LinkedList<int>();
            for (LinkedListNode<int> i = diag2; i != diag1; i = (i.Next != null) ? i.Next : polygon.First) {
                right.AddLast(i.Value);
            }
            right.AddLast(diag1.Value);

            // Recursively search for triangles.
            Split(left, points, triangles);
            Split(right, points, triangles);
        }

        /// <summary>
        /// Checks the degree of colinearity
        /// </summary>
        /// <param name="testPoint">Point to test</param>
        /// <param name="origin">Coordinate origin</param>
        /// <param name="direction">Direction vector to compare</param>
        /// <returns>Dot product of vector to testPoint and direction</returns>
        private float GetColinearity(Vector2 testPoint, Vector2 origin, Vector2 direction) {
            Vector2 dir = testPoint - origin;
            dir.Normalize();

            return Math.Abs(Vector2.Dot(dir, direction));
        }


        /// <summary>
        /// Creates vertices that can be sent to the graphics card for drawing.
        /// </summary>
        /// <remarks>
        /// This is the method that maps the texture to the triangles.
        /// </remarks>
        /// <param name="triangles">List of triangles</param>
        private void CreateDrawable(List<Vector2[]> triangles) {
            // Loop over triangles to create vertices
            vertices = new VertexPositionTexture[triangles.Count * 3];
            for (int i = 0; i < triangles.Count; i++) {
                Vector2[] points = triangles[i];

                // Loop over points in triangle to create vertices
                for (int j = 0; j < 3; j++) {
                    Vector3 v = new Vector3(points[j], 0);

                    // Generate texture coordinate around the center of the texture
                    Vector2 t = drawScale * points[j] / new Vector2(Texture.Width, Texture.Height) +
                                new Vector2(0.5f);

                    vertices[3 * i + j] = new VertexPositionTexture(v, t);
                }
            }
        }
    #endregion

    #region Physics Initialization
        /// <summary>
        /// Create a new polygonal shape.
        /// </summary>
        /// <remarks>
        /// Note that we have to recreate a lot of polygonal information.  Try to
        /// minimize the number of times this method is called (e.g limit properties
        /// that dirty the structure).
        /// </remarks>
        protected override void CreateShape() {
            if (fixtures.Count > 0) {
                foreach (Fixture fixture in fixtures) {
                    body.DestroyFixture(fixture);
                }
            }
            CalculateTriangles();
            foreach (Vector2[] triangle in triangles) {
                PolygonShape polygon = new PolygonShape(new Vertices(triangle), density);
                fixtures.AddLast(body.CreateFixture(polygon, this));
            }
            body.Friction = friction;
            body.Restitution = restitution;
            isDirty = false;
        }

        /// <summary>
        /// Adjusts the vertices to fill the new bounding box if necessary.
        /// </summary>
        /// <remarks>
        /// This method is NOT cheap.  Use it sparingly.
        /// </remarks>
        private void CalculateTriangles() {
            if (sizeScale == Vector2.One) {
                return;
            }

            // Resize the triangles
            for (int ii = 0; ii < triangles.Count; ii++) {
                for (int jj = 0; jj < triangles[ii].Length; jj++) {
                    triangles[ii][jj].X *= sizeScale.X;
                    triangles[ii][jj].Y *= sizeScale.Y;
                }
            }

            // Rebuild the drawing information
            CreateDrawable(triangles);

            // Do not need to rescale again.
            sizeScale = Vector2.One;
        }
    #endregion

    #region Game Loop (Update and Draw)
        /// <summary>
        /// Draws the physics object.
        /// </summary>
        /// <param name="view">Drawing context</param>
        public override void Draw(GameView view) {
            view.DrawPolygons(vertices, Texture, Position, Rotation, 1.0f, BlendState.AlphaBlend,false,Vector2.Zero);
        }
    #endregion

    }
}