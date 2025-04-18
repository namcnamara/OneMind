shader_type spatial;

varying vec3 world_vertex;

void vertex() {
	// Called for every vertex the material is visible on.
	world_vertex = (MODEL_MATRIX * vec4(VERTEX,1)).xyz;
	VERTEX = VERTEX + NORMAL * sin(world_vertex.x*20.0+TIME*5.0)*cos(world_vertex.z*20.0+TIME*5.0)*0.001;
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO = vec3(1,1,1);
	ROUGHNESS = 0.1;
	METALLIC = 0.0;
	float x_slope = cos(world_vertex.x*20.0+TIME)*0.001*20.0;
	float y_slope = -sin(world_vertex.z*20.0+TIME)*0.001*20.0;
	vec3 new_normal = -cross(normalize(vec3(1,x_slope,0)),normalize(vec3(0,y_slope,1)));
	NORMAL = new_normal;
	ALPHA=0.01;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
