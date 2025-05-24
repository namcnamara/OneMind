shader_type canvas_item;

uniform sampler2D sprite_texture;

uniform float flash_strength = 0.0;
uniform vec3 flash_color = vec3(1.0, 0.0, 0.0); // red

void fragment() {
    vec4 base_color = texture(sprite_texture, UV);
    vec3 final_color = mix(base_color.rgb, flash_color, flash_strength);
    COLOR = vec4(final_color, base_color.a);
}