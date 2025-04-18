

uniform float step_offset = 0;

vec3 step_3d(vec3 old_value, vec3 new_value, int step_index) {
	if (float(step_index) > step_offset+1.0) {
		return old_value;
	} else if (float(step_index) > step_offset) {
		return mix(old_value,new_value,step_offset+1.0-float(step_index));
	} else {
		return new_value;
	}
}

