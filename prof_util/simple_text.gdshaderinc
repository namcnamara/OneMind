
#ifndef UTIL_SIMPLE_TEXT
#define UTIL_SIMPLE_TEXT

#include "grid.gdshaderinc"

// An array defining a bitmap for all printable ASCII characters.
// The font used here is Unifont. Check it out if you are looking for a
// comprehensive unicode bitmap font.
const uint TEXT_8x16_ARRAY[] = {
// ' '
0x00000000u,0x00000000u,0x00000000u,0x00000000u,
// '!'
0x00000000u,0x10101010u,0x10101000u,0x10100000u,
// '"'
0x00004444u,0x44440000u,0x00000000u,0x00000000u,
// '#'
0x00000000u,0x242424fcu,0x4848fc90u,0x90900000u,
// '$'
0x00000000u,0x107c9290u,0x701c1292u,0x7c100000u,
// '%'
0x00000000u,0x62949468u,0x10102c52u,0x528c0000u,
// '&'
0x00000000u,0x38444428u,0x30528a84u,0x8c720000u,
// '''
0x00001010u,0x10100000u,0x00000000u,0x00000000u,
// '('
0x00000008u,0x10102020u,0x20202020u,0x10100800u,
// ')'
0x00000040u,0x20201010u,0x10101010u,0x20204000u,
// '*'
0x00000000u,0x00001092u,0x54385492u,0x10000000u,
// '+'
0x00000000u,0x00001010u,0x10fe1010u,0x10000000u,
// ','
0x00000000u,0x00000000u,0x00000000u,0x30101020u,
// '-'
0x00000000u,0x00000000u,0x00780000u,0x00000000u,
// '.'
0x00000000u,0x00000000u,0x00000000u,0x30300000u,
// '/'
0x00000000u,0x04040810u,0x10202040u,0x80800000u,
// '0'
0x00000000u,0x3048848cu,0x94a4c484u,0x48300000u,
// '1'
0x00000000u,0x10305010u,0x10101010u,0x107c0000u,
// '2'
0x00000000u,0x78848404u,0x18204080u,0x80fc0000u,
// '3'
0x00000000u,0x78848404u,0x38040484u,0x84780000u,
// '4'
0x00000000u,0x08182848u,0x8888fc08u,0x08080000u,
// '5'
0x00000000u,0xfc808080u,0xf8040404u,0x84780000u,
// '6'
0x00000000u,0x38408080u,0xf8848484u,0x84780000u,
// '7'
0x00000000u,0xfc040408u,0x08081010u,0x10100000u,
// '8'
0x00000000u,0x78848484u,0x78848484u,0x84780000u,
// '9'
0x00000000u,0x78848484u,0x7c040404u,0x08700000u,
// ':'
0x00000000u,0x00003030u,0x00000030u,0x30000000u,
// ';'
0x00000000u,0x00003030u,0x00000030u,0x10102000u,
// '<'
0x00000000u,0x00040810u,0x20402010u,0x08040000u,
// '='
0x00000000u,0x000000fcu,0x000000fcu,0x00000000u,
// '>'
0x00000000u,0x00804020u,0x10081020u,0x40800000u,
// '?'
0x00000000u,0x78848404u,0x08101000u,0x10100000u,
// '@'
0x00000000u,0x384494acu,0xa4a4a49cu,0x403c0000u,
// 'A'
0x00000000u,0x30484884u,0x84fc8484u,0x84840000u,
// 'B'
0x00000000u,0xf8848484u,0xf8848484u,0x84f80000u,
// 'C'
0x00000000u,0x78848480u,0x80808084u,0x84780000u,
// 'D'
0x00000000u,0xf0888484u,0x84848484u,0x88f00000u,
// 'E'
0x00000000u,0xfc808080u,0xf8808080u,0x80fc0000u,
// 'F'
0x00000000u,0xfc808080u,0xf8808080u,0x80800000u,
// 'G'
0x00000000u,0x78848480u,0x809c8484u,0x8c740000u,
// 'H'
0x00000000u,0x84848484u,0xfc848484u,0x84840000u,
// 'I'
0x00000000u,0x7c101010u,0x10101010u,0x107c0000u,
// 'J'
0x00000000u,0x3e080808u,0x08080888u,0x88700000u,
// 'K'
0x00000000u,0x848890a0u,0xc0c0a090u,0x88840000u,
// 'L'
0x00000000u,0x80808080u,0x80808080u,0x80fc0000u,
// 'M'
0x00000000u,0x8484ccccu,0xb4b48484u,0x84840000u,
// 'N'
0x00000000u,0x84c4c4a4u,0xa494948cu,0x8c840000u,
// 'O'
0x00000000u,0x78848484u,0x84848484u,0x84780000u,
// 'P'
0x00000000u,0xf8848484u,0xf8808080u,0x80800000u,
// 'Q'
0x00000000u,0x78848484u,0x848484b4u,0xcc780600u,
// 'R'
0x00000000u,0xf8848484u,0xf8908888u,0x84840000u,
// 'S'
0x00000000u,0x78848480u,0x60180484u,0x84780000u,
// 'T'
0x00000000u,0xfe101010u,0x10101010u,0x10100000u,
// 'U'
0x00000000u,0x84848484u,0x84848484u,0x84780000u,
// 'V'
0x00000000u,0x82828244u,0x44442828u,0x10100000u,
// 'W'
0x00000000u,0x84848484u,0xb4b4ccccu,0x84840000u,
// 'X'
0x00000000u,0x84844848u,0x30304848u,0x84840000u,
// 'Y'
0x00000000u,0x82824444u,0x28101010u,0x10100000u,
// 'Z'
0x00000000u,0xfc040408u,0x10204080u,0x80fc0000u,
// '['
0x0000001cu,0x10101010u,0x10101010u,0x10101c00u,
// '\'
0x00000000u,0x80804020u,0x20101008u,0x04040000u,
// ']'
0x000000e0u,0x20202020u,0x20202020u,0x2020e000u,
// '^'
0x00003048u,0x84000000u,0x00000000u,0x00000000u,
// '_'
0x00000000u,0x00000000u,0x00000000u,0x0000fe00u,
// '`'
0x00402010u,0x00000000u,0x00000000u,0x00000000u,
// 'a'
0x00000000u,0x00007884u,0x047c8484u,0x8c740000u,
// 'b'
0x00000080u,0x8080b8c4u,0x84848484u,0xc4b80000u,
// 'c'
0x00000000u,0x00007884u,0x80808080u,0x84780000u,
// 'd'
0x00000004u,0x0404748cu,0x84848484u,0x8c740000u,
// 'e'
0x00000000u,0x00007884u,0x84fc8080u,0x84780000u,
// 'f'
0x00000018u,0x202020f8u,0x20202020u,0x20200000u,
// 'g'
0x00000000u,0x00047488u,0x88887040u,0x78848478u,
// 'h'
0x00000080u,0x8080b8c4u,0x84848484u,0x84840000u,
// 'i'
0x00000010u,0x10003010u,0x10101010u,0x107c0000u,
// 'j'
0x00000008u,0x08001808u,0x08080808u,0x08089060u,
// 'k'
0x00000080u,0x80808890u,0xa0c0a090u,0x88840000u,
// 'l'
0x00000030u,0x10101010u,0x10101010u,0x107c0000u,
// 'm'
0x00000000u,0x0000ec92u,0x92929292u,0x92920000u,
// 'n'
0x00000000u,0x0000b8c4u,0x84848484u,0x84840000u,
// 'o'
0x00000000u,0x00007884u,0x84848484u,0x84780000u,
// 'p'
0x00000000u,0x0000b8c4u,0x84848484u,0xc4b88080u,
// 'q'
0x00000000u,0x0000748cu,0x84848484u,0x8c740404u,
// 'r'
0x00000000u,0x0000b8c4u,0x84808080u,0x80800000u,
// 's'
0x00000000u,0x00007884u,0x80601804u,0x84780000u,
// 't'
0x00000000u,0x202020f8u,0x20202020u,0x20180000u,
// 'u'
0x00000000u,0x00008484u,0x84848484u,0x8c740000u,
// 'v'
0x00000000u,0x00008484u,0x84484848u,0x30300000u,
// 'w'
0x00000000u,0x00008292u,0x92929292u,0x926c0000u,
// 'x'
0x00000000u,0x00008484u,0x48303048u,0x84840000u,
// 'y'
0x00000000u,0x00008484u,0x8484844cu,0x34040478u,
// 'z'
0x00000000u,0x0000fc04u,0x08102040u,0x80fc0000u,
// '{'
0x00000018u,0x20201010u,0x20402010u,0x10202018u,
// '|'
0x00001010u,0x10101010u,0x10101010u,0x10101010u,
// '}'
0x00000060u,0x10102020u,0x10081020u,0x20101060u,
// '~'
0x00000062u,0x928c0000u,0x00000000u,0x00000000u
};

// Samples a character with ASCII value `codepoint`, using `inner_coordinate`
// as normalized coordinates indicating what part of the character's image
// is to be sampled.
bool text_16x16_sample(uint codepoint,vec2 inner_coordinate) {
	// If the codepint does not correspond to a printable ASCII character,
	// just return false;
	if ( (codepoint < 32u) || (codepoint >= 128u) ) {
		return false;
	}
	// Offset so the first printable ASCII char (space) is 1
	codepoint -= 32u;
	// Get which pixel we are trying to sample from the character's image
	uvec2 inner_tile_position = uvec2(inner_coordinate * vec2(8,16));
	// Get the index of the integer element we need to sample (each char
	// consists of 4 32-bit ints, making an 8x16-bit image
	uint int_index = codepoint*4u+inner_tile_position.y/4u;
	// Get the index of the bit we need to sample in the integer we are
	// sampling
	uint bit_index = (3u-(inner_tile_position.y % 4u)) * 8u + 8u-inner_tile_position.x;
	// Sample our calculated bit position for our calculated integer index
	return ((TEXT_8x16_ARRAY[int_index] >> bit_index) & 1u) != 0u;
}

// Displays a float value `value` consisting of `leading`+1+`trailing`
// characters, with `leading` leading characters, followed by a period,
// followed by `trailing` trailing characters.
// The `inner_coordinate` is used to indicate the position in the 1-char high,
// `leading`+1+`trailing`-char wide readout is being sampled.
bool float_readout(float value, vec2 inner_coordinate, uint leading, uint trailing) {
	// Calculate how many characters wide the readout is
	uint total_digits = leading + trailing;
	uint total_tiles = total_digits + 1u;
	// Calculate how we need to scale the x position to map between normalized
	// `inner_coordinate` and tile coordinate
	float hrz_scaling = 1.0 / float(total_tiles);
	// Find the integer x coordinate of the character being sampled
	int tile = int(inner_coordinate.x * float(total_tiles));
	// If we were given bad coordinates, just return false.
	if ((tile < 0) || (tile>=int(total_tiles))) {
		return false;
	}
	// By default, assume we are rendering a space, in case things get weird.
	uint codepoint = 32u;
	// Perform digit calculations for all character positions that don't
	// correspond with the decimal point.
	if (tile != int(leading)) {
		int  digit_position = int(leading)-tile-1;
		if (tile > int(leading)) {
			digit_position ++;
		}
		uint digit = 0u;
		float scaled_value = value * pow(10.0,-float(leading)+1.0);
		scaled_value = min(10.0,scaled_value);
		for (int i=int(leading)-1; i>=-int(trailing); i--) {
			if (scaled_value >= 1.0) {
				digit = uint(floor(abs(scaled_value)))%10u;
				if (digit == 0u) {
					digit = 1u;
				}
				scaled_value -= float(digit);
			} else {
				digit = 0u;
			}
			if (i == digit_position) {
				break;
			}
			scaled_value *= 10.0;
			scaled_value = min(10.0,scaled_value);
		}
		codepoint = uint(48u + digit);
	} else {
		// Set the code point for the decimal character
		codepoint = 46u;
	}
	// Sample the bitmap of the codepoint we calculated
	return text_16x16_sample(codepoint,inner_coord_2d(inner_coordinate,vec2(hrz_scaling,1.)));
}


#endif // UTIL_SIMPLE_TEXT
