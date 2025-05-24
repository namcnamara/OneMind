shader_type spatial;

uniform float time;

float rand(float x) {
    return fract(sin(x * 12.9898) * 43758.5453);
}

void fragment() {
    float stripes = 10.0;
    float speed = 1.0;

    float yOffset = (rand(UV.x * 100.0 + time * speed) - 0.5) * 0.1;
    float xPos = UV.x + sin(UV.y * 20.0 + yOffset * 10.0 + time * speed * 2.0) * 0.05;
    float stripe_index = floor(xPos * stripes);

    float r = rand(stripe_index * 10.0 + time * speed);
    vec3 green = vec3(0.1, 0.5, 0.1);
    vec3 yellow = vec3(0.8, 0.8, 0.2);

    vec3 baseColor = mix(green, yellow, r);
    vec3 color = mod(stripe_index, 2.0) < 1.0 ? baseColor : green * 0.5;
    ALBEDO = color;
}