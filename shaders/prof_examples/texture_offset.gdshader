shader_type spatial;

uniform sampler2D offset;
uniform sampler2D normal;

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX  += texture(offset,UV).rgb;
	NORMAL   = texture(normal,UV).rgb;
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO = vec3(1,1,1);
	ROUGHNESS = 0.;
}
