using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawPhysix
{
    public class RigidBody
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Mass;
        public float Size;
        public bool IsStatic;

        // Constructor for 2D objects
        public RigidBody(Vector2 position, float mass = 1.0f, float size = 1.0f)
        {
            Position = position;
            Velocity = Vector2.Zero;
            Mass = mass;
            Size = size;
            IsStatic = mass == 0;  // Static objects have 0 mass
        }

        // Update the physics for the rigid body (gravity, velocity, etc.)
        public void Update(float deltaTime)
        {
            if (!IsStatic)
            {
                // Apply gravity (simple gravity in the Y-direction)
                Velocity += new Vector2(0, -9.81f) * deltaTime;  // Gravity in 2D

                // Update position based on velocity
                Position += Velocity * deltaTime;
            }
        }
    }

}
