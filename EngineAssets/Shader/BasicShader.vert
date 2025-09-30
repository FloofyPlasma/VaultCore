#version 330 core

in vec3 position;
in vec3 normal;
in vec2 uv;

// World-space metadata
uniform mat4 translation;
uniform mat4 projection;
uniform mat4 view;

out vec2 uvcoord;
out vec3 normalcoord;
out vec3 fragpos;

void basicVertex() {
    gl_Position = projection * view * translation * vec4(position, 1.0);
    uvcoord = uv;
    fragpos = vec3(translation * vec4(position, 1.0));
    normalcoord = mat3(transpose(inverse(translation))) * normal;
}