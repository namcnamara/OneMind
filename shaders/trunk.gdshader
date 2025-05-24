shader_type spatial;

float rand(float x) {
    return fract(sin(x * 12.9898) * 43758.5453);
}

void fragment() {
    float stripes = 10.0;
    float yOffset = (rand(UV.x * 100.0) - 0.5) * 0.1;
    float xPos = UV.x + sin(UV.y * 20.0 + yOffset * 10.0) * 0.05;
    float stripe_index = floor(xPos * stripes);

    float r = rand(stripe_index * 10.0);
    vec3 brown = vec3(0.3, 0.15, 0.05);
    vec3 black = vec3(0.0);

    vec3 baseColor = mix(brown, black, r);
    vec3 color = mod(stripe_index, 2.0) < 1.0 ? baseColor : black;
    ALBEDO = color;
}