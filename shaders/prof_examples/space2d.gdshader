shader_type spatial;

#include "util/coord_grid.gdshaderinc"

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	ALBEDO = coord_grid_2d(UV,0.2,0.2,2,2).rgb;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
