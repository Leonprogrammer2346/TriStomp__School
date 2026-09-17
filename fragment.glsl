#version 330 core

in vec2 texCoord;
out vec4 FragColor;

uniform sampler2D texture0;
uniform float opacity;

void main()
{
    vec4 textureColor = texture(texture0, texCoord);

    FragColor = vec4(
        textureColor.rgb,
        textureColor.a * opacity
    );
}