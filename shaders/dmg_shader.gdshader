shader_type spatial; 
uniform sampler2D texture_albedo : source_color;
uniform float flash_strength : hint_range(0.0, 2.0) = 1.0;

void fragment() {
    vec4 base_color = texture(texture_albedo, UV);
    vec3 flash_color = mix(base_color.rgb, vec3(1.0, 0.0, 0.0), flash_strength);
    ALBEDO = flash_color;
}

