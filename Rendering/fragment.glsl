#version 330 core

uniform vec3 cubeColor;  // The color of the cube

out vec4 FragColor;

void main()
{
    FragColor = vec4(cubeColor, 1.0f);
}
