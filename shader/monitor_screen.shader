shader_type spatial;
render_mode blend_mix, depth_draw_opaque, cull_back, diffuse_burley, specular_schlick_ggx, unshaded;

uniform vec4 bgColor : hint_color = vec4(0.56, 0.56, 0.56, 1.0);
uniform vec4 color : hint_color = vec4(0.7, 0.8, 2.4, 1.0);
uniform float bgColorFactor = 0.11;
uniform sampler2D textureAlbedo : hint_albedo;
uniform vec3 uv1_scale;
uniform vec3 uv1_offset;


void vertex()
{
	UV = UV * uv1_scale.xy + uv1_offset.xy;
}

vec3 calculateEffect(vec4 at)
{
	if(((at.r * 0.299) + (at.g * 0.587) + (at.g * 0.114)) <= bgColorFactor)
		return bgColor.rgb * at.rgb;
	
	return color.rgb * at.rgb;
}


void fragment()
{
	vec4 albedoTex = texture(textureAlbedo, UV);
	ALBEDO = calculateEffect(albedoTex);
}
