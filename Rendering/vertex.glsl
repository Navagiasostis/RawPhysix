#version 330 core

layout(location = 0) in vec2 aPosition;  // Position of the vertex

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 0.0, 1.0);
}
