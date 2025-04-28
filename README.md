# Magnetics
This repo contains a C# class library for calculating the magnetic flux density (measured in Tesla) and magnetic field intensity (measured in Henry or A/m) field vectors at any point around a magnet. It is an unfinished work, and currently only models rectangular magnets.

## Background
I wanted to design a sensor which could measure the rotation (angular position) as one part of an assembly is rotated within another. 
For reliability, I chose to base my design on a linear Hall effect sensor moving with respect to a couple of magnets. To begin, I experimented with a couple of 0.25" cubic neodymium magnets and a DRV5055 Hall sensor.
My experimental rig was set up like the illustration below:

![Sensor setup](/Magnetics/SensorSimulator/SensorSetup.png)

In my test rig, the magnets moved and the sensor was fixed, but we can think of it as follows:

* Two magnets are placed a set distance from an axis of rotation; in other words, both magnets lie on an arc at some radius from the axis.
* One magnet has its north pole facing the axis, the other has its south pole facing the axis
* The sensor moves on an arc inside the arc of the magnets and is always facing directly away from the axis
* The sensor will detect a more "north" or a more "south" field, depending which pole it is most directly facing, and will be quiescent at the midpoint between the magnets where their effects cancel out.

The rig gave me some basic information but I needed to make refinements. 
I thought it would be a good idea to try to mathematically model the setup to guide my design efforts.
After some reasearch I found a website [here](https://www.e-magnetica.pl/doku.php/calculator/field_of_cuboid_magnet_or_rectangular_solenoid) based on a paper [here](https://doi.org/10.1063/5.0010982) 
which set out formulas for calculating the field around rectangular magnets. This library implements those formulas in a way that I hope can be extended for other magnet shapes.
This repo includes the mathematical model of my test rig; its output matches values I recorded from the actual rig closely enough that I consider the model valid, and useful for my design efforts.
For what it's worth I've included a chart of the model output below (note that sign reflects the orientation of the field):

![SensorModelOutput](/Magnetics/SensorSimulator/Response.png)

## The Magnets Library

The library contains a framework for modeling the fields around magnets of different shapes, although it currently only implements a model for rectangular magnets.
* **IMagnet** is the interface for a magnet, predicated on certain assumptions and notions:
  * Uniform magnetization
  * Magnets have a scalar **remanence** property and a related scalar **surface field** property, notionally the magnitude of the **B** (flux density) vector at the point where the magnetization axis intersects the magnet's surface
  * Magnets produce **H** (magnetic field intensity) and **B** (magnetic flux density) fields resulting in **H** and **B** vectors at any given point in 3D space
  * Position and field vectors are with respect to the magnet; magnetization is along the Z axis.
* **Magnet** is an abstract class implementing **IMagnet** and introduces a few traits and behaviors common to any magnet
* **RectangularMagnet** derivess from **Magnet** and implements the calculations specific to a rectangular magnet per the references cited above
* **MagnetWithPosition2** derives from **IMagnet** and implements a wrapper around a given magnet to position and orient it in a 2D space to faciliate 2D modeling.
* **CubicMagnet** is a convenience class derived from **RectangularMagnet** to simplify dimensioning.
* **Constants** contains various useful unit conversion values in addition to physical constants
* **GeometryHelper** and **MagnetHelper** contain extension methods which include support for projection from 3D to 2D and corresponding inverses.
Since the referenced site/paper use a convention in which magnetization is along the Z axis,
the mapping convention used here is as follows:
  * The X axis in 3D space is removed by the projection so is not represented in the 2D space.
  * The Y axis in 3D space is preserved by the projection into the 2D space.
  * The Z axis in 3D space is projected onto the X axis in the 2D space.
  
## The SensorSimulator Application

This is a simple console app which models my test rig. It outputs both the **B** vector component experienced by the sensor and the expected voltage value for a range of angular positions
