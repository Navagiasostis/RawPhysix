using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawPhysix
{
    public class PhysicsWorld
    {
        private List<RigidBody> _bodies;

        public PhysicsWorld()
        {
            _bodies = new List<RigidBody>();
        }

        // Add a rigid body to the world
        public void AddBody(RigidBody body)
        {
            _bodies.Add(body);
        }

        // Update the physics world (move objects, handle collisions)
        public void Step(float deltaTime)
        {
            foreach (var body in _bodies)
            {
                body.Update(deltaTime);
            }

            // Handle basic collision detection (for simplicity, we'll just check floor collision)
            foreach (var body in _bodies)
            {
                // Collision with the floor (Y = 0)
                if (body.Position.Y <= 0)
                {
                    body.Position = new Vector2(body.Position.X, 0);  // Reset to floor level
                    body.Velocity = Vector2.Zero;  // Stop movement after collision
                }
            }
        }
    }

}
