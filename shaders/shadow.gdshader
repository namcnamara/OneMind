shader_type spatial;
render_mode unshaded, cull_disabled;
uniform float radius : hint_range(0.1, 2.0) = 1.0;
uniform float opacity : hint_range(0.0, 1.0) = 0.5;

void fragment() {
    vec2 centered_uv = (UV - vec2(0.5)) * 2.0;
    float dist = length(centered_uv);

    float alpha = smoothstep(radius, 0.0, dist);
    vec4 shadow_color = vec4(0.0, 0.0, 0.0, alpha * opacity);
    ALBEDO = shadow_color.rgb;
    ALPHA = shadow_color.a;
}