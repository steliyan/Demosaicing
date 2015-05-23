This is the information file of Laurent Condat's Image Database (Preliminary version, June 1st, 2009).

The purpose of this database is to provide a large (150) number of color and grayscale images of natural scenes for use in image processing/computer vision research and teaching. The images are free for any non-commercial use, under the terms of the Creative Commons license available at http://creativecommons.org/licenses/by-nc-sa/2.0/fr/deed.en_CA
Please refer to this set of images as Laurent Condat's Image Database
and cite the link to its repository http://www.greyc.ensicaen.fr/~lcondat/imagebase.html

All images in the database are currently stored in the TIFF format. Some information about this format is available from http://www.awaresystems.be/imaging/tiff.html and from Adobe Systems. The "libtiff" library of C functions for reading and writing TIFF images is available from http://www.remotesensing.org/libtiff/. The "netpbm" collection of image format conversion programs (http://netpbm.sourceforge.net/) can convert between TIFF and many different formats.

All the images come from several megapixels digital pictures compressed in the JPEG format using a high quality setting. Most of the pictures were taken by me using a 7 Mpix Canon Powershot S70 or a 8 Mpix Canon IXY Digital 910IS (the Japanese version of the Powershot SD870 IS). The other pictures were taken by other people who kindly granted me permissions to include them in the database:
IM145 to IM150: Thomas Leibovici (http://www.routard.com/membre_photos/3041__)
IM094, IM095: Jocelyn Chanussot (http://www.lis.inpg.fr/pages_perso/chanussot/index.html)
IM044, IM045, IM062, IM110: unknown

The images in the 3:2 format and a few others were cropped to obtain a satisfying composition in the 4:3 format. Then, they were carefully reduced from several Mpix to the size 720x540 or 540x720. The downscaling process, which uses a dilated cubic cardinal spline as lowpass function, is explained in the report "how to reduce an image size" (to come soon).
Except crop and reduction, no other process (like sharpening, contrast enhancement...) was applied.

The grayscale versions of the image were obtained by calculating pixelwise 0.299*R + 0.587*V + 0.114*B and then rounding this value to the nearest integer in the range 0..255. This corresponds to the definition of the Y luminance channel in the YUV format used for video coding and transmission.

Featuring the following people, who granted permission to use their image:
Tamouna: IM007, IM026, IM050, IM054, IM078, IM100, IM103, IM122
Sveta: IM004
Salome: IM013
JŽr™me: IM009, IM019, IM066, IM102, IM118, IM120
CŽcile: IM036
Pilou: IM036, IM060
Lika: IM076, IM078
Marie: IM078
Levani: IM078
Monique: IM113
Jo‘lle: IM116 
Bernard: IM126
Bruno: IM121
Your servitor: IM045, IM062, IM078, IM092, IM110

Please contact Laurent Condat for any comment, critic or suggestion. Use the Image Database without limit, as it was made to be used!

IMPORTANT NOTE
The color space relevant for the R,G,B values of the images is sRGB (sRGB IEC611966-2.1 profile).
After processing an image with a program, one should pay attention that the output image file generally has NO embedded color profile. This will cause any viewer with color management capabilities to assume the default color profile for displaying the images, which by default is a generic profile and NOT the sRGB IEC611966-2.1 profile. For instance, using Preview on Mac, do Tools->Assign a profile->sRGB IEC611966-2.1. Else, you will not see what you ought to see! This is a common mistake, because the visual difference is not so pronounced. Assign the sRGB profile to a processed image also if you want to use it as an illustration in an article, for instance before conversion to the EPS format for inclusion as a figure in a latex file. 
Another important tip: If you have a Mac notebook, change the default gamma 1.8 to 2.2 in the display settings.

