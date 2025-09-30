#version 330 core

in vec2 uvcoord;
in vec3 normalcoord;
in vec3 fragpos;

uniform sampler2D tex;

// Lighting Metadata
uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 cameraPos;

// Silly light properties :3
uniform float shininess;

out vec4 fragColor;

void basicFragment()
{
    // ambient
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    // diffuse
    vec3 norm = normalize(normalcoord);
    vec3 lightDirection = normalize(lightPos - fragpos);

    float diff = max(dot(norm, lightDirection), 0.0);
    vec3 diffuse = diff * lightColor;

    // specular
    float specularStrength = 0.5;
    vec3 viewDirection = normalize(cameraPos - fragpos);
    vec3 reflectDirection = reflect(-lightDirection, norm);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), shininess);
    vec3 specular = specularStrength * spec * lightColor;

    vec3 result = (ambient + diffuse + specular) * objectColor;
    fragColor = texture(tex, uvcoord) * vec4(result, 1.0);
}