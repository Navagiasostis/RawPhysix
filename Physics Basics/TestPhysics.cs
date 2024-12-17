using OpenTK.Mathematics;
using RawPhysix.Rendering;

namespace RawPhysix
{
    class TestPhysics
    {
        public static void Main(string[] args)
        {
            // Create world
            PhysicsWorld world = new PhysicsWorld();

            // test body
            RigidBody testBody = new RigidBody(new Vector2(0, 10), 1.0f, 1.0f);
            world.AddBody(testBody);

            // Add a floor for testing collision
            RigidBody floor = new RigidBody(new Vector2(0, 0), 0.0f); // Static floor at (0, 0, 0)
            world.AddBody(floor);

            // Create the renderer
            Renderer renderer = new Renderer();

            // Initialize the physics simulation and rendering
            renderer.Start(); // Must be in main Thread!
        }
    }

}
