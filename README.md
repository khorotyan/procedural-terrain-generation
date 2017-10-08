# README #

Dwelland is a procedural terrain generation tool, made for the Numerical Analysis course. 
The application generates a terrain using Perlin Noise and makes it to look natural by applying some techniques. 
Noise scale is the size of the noise that is picked from the perlin noise, the higher it is the smaller part of it is chosen causing the terrain to look flat. 
Lacunarity and Persistence control the frequency and the amplitude of the terrain by changing the function parameters that makes the terrain more natural like. 
Octaves is the number of function applied to the terrain. X and Y offset move the terrain towards the x and y axis. 
Warp m and Warp n parameters control the spline that curves the terrain and the level of detail controls the coloring of the terrain which is applied using bilinear interpolation.