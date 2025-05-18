# Bike Computer

## Introduction

In this exercise, you will implement a data analysis software for data gathered by a simple bike computer.

The bike computer consists of a small magnet attached to the spokes of the front wheel. A sensor is attached to the fork of the bike. Each time the magnet passes the sensor, it sends a signal to the computer. The computer stores an exact timestamp of the event. You can find an example of such a system [here](https://support.polar.com/e_manuals/Speed_Sensor_BT_Smart/Polar_Speed_Sensor_BT_Smart_accessory_manual_English/content/installing_the_speed_sensor.htm).

## Minimum Requirements

### Register a bike

The user must be able to register a bike. For our application, it is critical to know the circumference of the wheel/tire.Therefore, the user must provide the following information when registering a bike:

* Title of the bike (e.g. "My racer", "Mountain climber", "Fixie")
* Serial number of the bike computer on the bike
* Circumference of the wheel/tire in one of the following ways:
  * ETRTO size designation (_European Tire and Rim Technical Organization_; e.g. _58-622_)
  * Diameter in mm. You have to convert the diameter to circumference using the formula `circumference = diameter * pi`.

You can find a list of ETRTO sizes relevant for this exercise in [*tire_types.json*](./tire_types.json).^

Design the user interface in a way that the user can easily select the size of the tire (e.g. dropdown list). If our ETRTO list does not contain the size that the user wants, the user must be able to enter the size manually. Note that the conversion from ETRTO or diameter to circumference *must be done in the backend*, not in the frontend.

Internally, our software must create some kind of unique identifier for the bike. However, the user always uses the title of the bike to refer to it.

### Upload ride data

Our application must offer a web API to upload ride data. This web API will *not* be called by the web frontend. It will be called by the bike computer during the synchronization process. The bike computer sends the following data to our application:

* Serial number of the bike computer
* Array of timestamps of the events (magnet passing the sensor)

The system generates a default title for the ride. It is _Ride on {date}_ where _{date}_ is the date of the first timestamp in the array.

With this exercise you get two sample files with timestamps:

* [*constant_ride.json*](./constant_ride.json)
* [*city_ride_timestamps.json*](./city_ride_timestamps.json)
* [*broken_timestamps.json*](./broken_timestamps.json)

If the serial number of the bike computer is not registered in our application, the API must return a meaningful error.

You can assume that the timestamps are all in the same format (ISO 8601, `2025-05-18T16:30:12.654`). However, our bike computers have a known error. Sometimes they "travel back in time". This means that the timestamps are not always in ascending order. If this happens, the API must return a meaningful error. [*broken_timestamps.json*](./broken_timestamps.json) contains an example of such a case.

You must write at least one meaningful unit test for the detection of broken timestamps.

### View rides

The user must be able to request a list of his registered bikes. If a new ride has been uploaded since she viewed the list, the corresponding bike must be marked with a _new ride available_ marker.

The user must be able to navigate to the rides of a specific bike. The rides must be displayed in a list. The user must be able to change the title of a ride. The user must be able to delete a ride.

### Advanced requirements

### Analyze rides (part 1)

From the list of rides, the user must be able to select a ride and view the following information:

* Total duration of the ride in hours, minutes and seconds
* Total distance of the ride in kilometers
* Average speed of the ride in km/h

Write at least three meaningful unit tests for the analysis of rides. Tipp: The average speed of the ride in [*constant_ride.json*](./constant_ride.json) is approx. 25km/h. 

### Analyze rides (part 2)

In addition to the information above, the user must be able to view the following information:

* Number of stops during the ride. A gap larger of equal to 3 seconds between two consecutive timestamps is considered a stop.
* Total duration of the stops in minutes and seconds

Write at least two meaningful unit tests for the analysis of rides. Tipp: The number of stops in [*city_ride_timestamps.json*](./city_ride_timestamps.json) is 9.

### Generate test data

Use an AI to generate test data for the following case:

* After a short acceleration (meaningful acceleration for a bike), the user drives uphill for 20 minutes with an average speed of 10km/h +/- 5%.
* After that, the user drives downhill for 6 minutes and 40 seconds with an average speed of 30km/h +/- 5%.
* After that, the user decelerates to a stop (meaningful deceleration for a bike)

## Quality criteria

* Backend with ASP.NET Core Minimal API and C#
* SQLite database with Entity Framework Core
* xUnit tests
* Angular for frontend
