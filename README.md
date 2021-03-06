# GEONIS gear sample measuring addin

This is a sample GEONIS gear AddIn, which allows the user to measure distances in a map. The tool shows the basic technics and mechanism on how to create an addin for GEONIS gear. It is based on the measuring tool provided with the product. For more information please contact: support@geocom.ch. 

#### DISCLAMER: Please be aware this product is not supported. Further information can be found in the license file.

------
## Requirements

- Installation of GEONIS gear 2017.x 
- Installation of Visual Studion 2017

------
## Installation and compilation

1. Download the *SampleMeasuringAddin* 
2. Open the solution file in Visual Studio (with administrative privileges, in case you want the compiles DLLs be copied directly into the GEONIS gear directory)
3. Adjust any references (shouldn't be necessary, if GEONIS gear is installed in the default location)
4. Compile
5. Adjust the GEONIS gear project file (project_gear.xml) by adding a reference to the addin under the AddIn tag
6. Run the GEONIS gear project


------
## Usage 

Like any other GEONIS gear map tool, the tool is located in the toolbar and activated by clicking on it. Once the tool is activated, click in the map to create a polyline. The lenght of the polyline and its segments will be displayed in the map.
Deactivating the tool will remove the polyline and the measure labels from the map.

![Alt text](https://github.com/geocom-gis/SampleMeasuringAddIn/blob/master/SampleMeasuringAddIn/measuring_sample_screenshot_1.png)

------
## Help

Detailed description on GEONIS gear configuration can be found on the Geocom resource page. Question to the tool, can be posted here


------
## Known issues

Currently none.
