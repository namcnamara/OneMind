#ifndef UTIL_COORD_GRID
#define UTIL_COORD_GRID

#include "grid.gdshaderinc"
#include "simple_text.gdshaderinc"

float flip_normal_coord(float value, bool flip) {
	if (flip) {
		return 1.0-value;
	} else {
		return value;
	}
}


vec4 coord_grid_2d (vec2 coord,float scale, float readout_height, uint leading, uint trailing) {

	vec2 scaled_coord = coord/scale;
	// Integer coordinates of cell in grid
	ivec2 cell_coord = cell_coord_2d(scaled_coord,vec2(1,1));
	// Normalized coordinates within the cell boundaries
	vec2 inner_coord = inner_coord_2d(scaled_coord,vec2(1,1));
	// Distance to the x/y borders of thc cell
	vec2 border_dist = 0.5-abs(inner_coord-vec2(0.5,0.5));
	// True if on the outline of the cell (the black portion)
	bool on_outline = any(lessThan(border_dist,vec2(0.01,0.01)));
	// Determine if on the border (coordinate readout) for the cell
	// X/Y boundaries
	bool on_x_border = border_dist.x < readout_height;
	bool on_y_border = border_dist.y < readout_height;
	// By default, return white and assume the fragment is not on text
	vec4 background_color = vec4(1,1,1,1);
	bool on_text = false;

	if (on_outline) {
		// The outline should be black
		background_color = vec4(0,0,0,1);
	} else if (on_x_border && ! on_y_border) {
		background_color = vec4(1,0,0,1);
		bool flip = (inner_coord.x > 0.5);
		vec2 readout_coord = vec2(
			flip_normal_coord((inner_coord.y-readout_height)/(1.0-readout_height*2.0),flip),
			flip_normal_coord(inner_coord.x/readout_height,!flip)
		);
		on_text = float_readout(float(cell_coord.x+int(flip))*scale,readout_coord,leading,trailing);
	} else if (on_y_border && ! on_x_border) {
		background_color = vec4(0,1,0,1);
		bool flip = (inner_coord.y > 0.5);
		vec2 readout_coord = vec2(
			flip_normal_coord((inner_coord.x-readout_height)/(1.0-readout_height*2.0),!flip),
			flip_normal_coord(border_dist.y/readout_height,true)
		);
		on_text = float_readout(float(cell_coord.y+int(flip))*scale,readout_coord,leading,trailing);
	}

	if(on_text) {
		return vec4(0,0,0,1);
	} else {
		return background_color;
	}

}

#endif
