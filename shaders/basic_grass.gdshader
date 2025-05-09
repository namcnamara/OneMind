shader_type spatial;
render_mode unshaded;

// Inversely affects the size, but increases the number of cells
// 1 makes a cell the size of a tree, 10 is tiny, 100 is granular
const float frequency = 23.4;
// .1 looks more like cobblestones, .9 is way less uniform
const float cell_variation = .9;
// Change the diameter of edge dividers
const float edge_thickness = 0.1;

float fake_randomness_color(vec2 p) 
{
	// Uses the x and y values of a vec2 to generate a sort of randomness,
	// return a relatively random float
	float rand_1 = p.x * p.y + 543.0f - 0.1f* p.x * p.y;
	if (rand_1 > 0.5)
	{
		rand_1 = cos(rand_1 * p.y - p.x);
	}
	else 
	{
		rand_1 = (rand_1);
	}
	return fract(rand_1);
}

float fake_randomness(vec2 p) 
{
	// Uses the x and y values of a vec2 to generate a sort of randomness,
	// return a relatively random float
	float rand_1 = p.x * p.y + 543.0f - 0.1f* p.x * p.y;
	if (rand_1 > 0.5)
	{
		rand_1 = cos(rand_1 * p.y - p.x);
	}
	return fract(rand_1);
}

// Gets the color for a given cell
vec3 get_cell_color(vec2 cell_id) {
    // Need to change this to be lighter by default, too dark rn
    return vec3(
        fake_randomness_color(cell_id) * 0.9,               // Red 
        fake_randomness_color(cell_id + vec2(10.0, 10.0)) * 0.9,  // Green 
        fake_randomness_color(cell_id + vec2(20.0, 20.0)) * 0.05 + 0.0  // Blue 
    );
}

varying vec3 world_pos;

void vertex() {
    world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

void fragment() {
	// Follows camera, pattern is fixed to screen like in chowder
    //vec2 uv = FRAGCOORD.xy / VIEWPORT_SIZE;
	vec2 uv = world_pos.xz;
	// This maps the (x,z) and (-x,-z) quadrants to perform correctly and not make the weird squares
	uv = abs(uv) * .2;
    uv *= frequency;

    float min_dist = 100.0;
    float second_min_dist = 200.0;
    vec2 closest_cell_id;

    for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
            vec2 cell_id = floor(uv) + vec2(float(x), float(y));
            vec2 jitter = vec2(
                fake_randomness(cell_id + vec2(10.0, 10.0)),
                fake_randomness(cell_id + vec2(10.0, 10.0))
            );
            vec2 cell_pos = cell_id + jitter * cell_variation;

            float dist = distance(uv, cell_pos);

            if (dist < min_dist) {
                second_min_dist = min_dist;
                min_dist = dist;
                closest_cell_id = cell_id;
            } else if (dist < second_min_dist) {
                second_min_dist = dist;
            }
        }
    }

    float edge = second_min_dist - min_dist;
    float edge_intensity = smoothstep(0.0, edge_thickness, edge);
    vec3 base_color = get_cell_color(closest_cell_id);
    vec3 color = mix(vec3(0.0), base_color, edge_intensity);
	//Update the color
    // Darken the final color by scaling it down, using 1.0 is fully bright, 0.0 is blakc
    ALBEDO = color * 0.085;
}