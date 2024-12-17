using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using RawPhysix.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RawPhysix.Rendering
{
    public class Renderer
    {
        private GameWindow _window;
        private Shader _shader;
        private int _vao;
        private RigidBody _cubeBody;
        private PhysicsWorld _world;
        private float _timeStep = 1.0f / 60.0f;
        private float _accumulatedTime = 0.0f;
        private float _lastFrameTime;
        private Vector3 _cubeColor = new Vector3(1.0f, 0.0f, 0.0f);  // Default red color for cube

        public void Start()
        {
            var settings = new GameWindowSettings();
            var nativeSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "2D Physics Visualization",
            };

            _window = new GameWindow(settings, nativeSettings);
            _window.Load += OnLoad;
            _window.RenderFrame += OnRenderFrame;
            _window.UpdateFrame += OnUpdateFrame;
            _window.Run();
        }

        private void OnLoad()
        {
            // Initialize shaders
            _shader = new Shader("Rendering/vertex.glsl", "Rendering/fragment.glsl");

            // Set up vertex data (square for 2D)
            float[] vertices = {
            // Position (X, Y)
                -0.5f, -0.5f,  // Bottom-left corner
                 0.5f, -0.5f,  // Bottom-right corner
                 0.5f,  0.5f,  // Top-right corner
                -0.5f,  0.5f   // Top-left corner
            };

            uint[] indices = {
                0, 1, 2,  // First triangle
                2, 3, 0   // Second triangle
            };

            // Generate VAO, VBO, and EBO
            int vbo, ebo;
            _vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Unbind
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            // Initialize physics world
            _world = new PhysicsWorld();

            // Create the cube's rigid body with initial position and mass
            _cubeBody = new RigidBody(new Vector2(400, 300), 1.0f);  // Start at the center (400, 300)
            _world.AddBody(_cubeBody);

            // Create a static floor (Y = 0)
            RigidBody floor = new RigidBody(new Vector2(0, 0), 0.0f);  // Static body
            floor.Mass = 0;  // Static object has no mass
            _world.AddBody(floor);

            // Store initial frame time
            _lastFrameTime = (float)GLFW.GetTime();
        }

        private void OnUpdateFrame(OpenTK.Windowing.Common.FrameEventArgs e)
        {
            // Get deltaTime
            float currentFrameTime = (float)GLFW.GetTime();
            float deltaTime = currentFrameTime - _lastFrameTime;

            _accumulatedTime += deltaTime;

            while (_accumulatedTime >= _timeStep)
            {
                _world.Step(_timeStep);
                _accumulatedTime -= _timeStep;
            }

            _lastFrameTime = currentFrameTime;

            // Check for collision with the floor
            if (_cubeBody.Position.Y <= 0.1f)
            {
                _cubeColor = new Vector3(0.0f, 1.0f, 0.0f);  // Change to green on collision
            }
            else
            {
                _cubeColor = new Vector3(1.0f, 0.0f, 0.0f);  // Default red
            }
        }

        private void OnRenderFrame(OpenTK.Windowing.Common.FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Use shader and bind VAO
            _shader.Use();

            // Send cube color to shader
            GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "cubeColor"), _cubeColor);

            // Set up model, view, projection matrices (2D Orthographic)
            Matrix4 model = Matrix4.CreateTranslation(_cubeBody.Position.X, _cubeBody.Position.Y, 0f);  // Use cube's position in 2D
            model *= Matrix4.CreateScale(150f); // Scale the cube up (try adjusting this value)

            Matrix4 view = Matrix4.CreateTranslation(0f, 0f, 0f);  // No view transformation (no camera in 2D)
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, 800, 600, 0, -1, 1);  // Orthographic for 2D

            // Send matrices to the shader
            GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "view"), false, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "projection"), false, ref projection);

            // Draw the cube using the updated position and scale
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            _window.SwapBuffers();
        }

    }

}
