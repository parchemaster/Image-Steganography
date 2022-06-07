Steganography
The principle you will use here is to hide the data in a pixel. This will necessarily change
the resulting pixel color, however, if you proceed smarter and change the pixel
only a small number (eg modification of the least significant bit), it will be very difficult
know.
Each pixel consists of several channels. Let us now consider the standard red,
green, blue, alpha (transparency). With this, each channel has one byte (0-255).
Altogether, each pixel has 4 bytes, representing a color. In your implementation
you will initially select one channel, which you modify by encrypting your information. I do not recommend starting with alpha.
Encoding is done by splitting all the bytes of your data into n-bit chunks. The ByteSpliting.Split method is used for splitting.
You insert each of these pieces in place of the least significant bits in the data
channel.
