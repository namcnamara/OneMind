shader_type spatial;

render_mode  depth_draw_opaque, cull_back;

void fragment() {
    ALBEDO = vec3(0.0); 
    ALPHA = 0.0;      
}