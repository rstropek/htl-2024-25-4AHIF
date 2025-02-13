# Software Inventory

## Introduction

You are working in a company providing software for _Software Inventory Mangagement_. Software Inventory Management refers to the systematic process of tracking, managing, and optimizing an organization's software assets. This involves identifying all software applications installed on devices, monitoring their usage, ensuring compliance with licensing agreements, and assessing security risks.

You have to implement a web API acting as the basis for different business processes. The API should provide the following functionality:

- Manage Computers
  - Add a new computer
  - Mark computers as decommissioned
- Manage Software
  - Add installed software to a computer
- Security
  - Find installations of software on all computers

Another team will implement a software agent that runs on every computer in an organization. The agent will regularly (scheduled, at idle times) collect information about the computer and installed software and send it to the API.

A third team will implement a web frontend for security engineers who use the API to search for affected computers in case of a security incident (e.g. known vulnerability in a certain software version).

## Domains

### Computer Management

In the domain _computer management_, the following APIs must be implemented:

- **Create or update a computer**: The agent sends data about a computer. If the computer already exists, update its properties. If the computer does not exist, create a new computer. The agent sends the following data:
  - MAC address (mandatory)
    - Unique identifier for the computer
    - 6 bytes written in 6 groups of 2 hex digits, e.g. _00:1A:2B:3C:4D:5E_
    - Correctness of the format must be checked by the API
  - IP address (mandatory)
    - IPv4 address in the format _x.x.x.x_, where _x_ is a number between 0 and 255
    - Correctness of the format must be checked by the API
  - Hostname (mandatory)
  - CPU (string, mandatory)
  - RAM in GB (mandatory)
    - 0-10000
    - Max. 2 digits after the decimal point
  - Total disk size in GB (mandatory)
    - 0-100000
    - Max. 2 digits after the decimal point
  - OS (`Windows`, `Linux`, or `MacOS`; mandatory)
  - The web API must store the timestamp when the computer was last updated. This data is not sent by the caller but must be maintained automatically by the API.

- **Mark decommised computers**: Iterates through all computers and marks computers as decommissioned if it has not been updated for more than 30 days. If a computer starts sending data again, the decommissioned flag must be removed.

### Software Management

In the domain _software management_, the following APIs must be implemented:

- **Add software to a computer**: The agent sends data about installed software. The data includes the MAC address of the computer. For each software, the following data is sent:
  - Identifier (mandatory, string)
    - Unique identifier for the software
    - E.g. GUID of the software component, installation folder, etc.
  - Name (mandatory)
  - Vendor (optional)
  - Version (mandatory string)
    - Semantic version with three segments (major, minor, path), e.g. _1.15.3_
    - Correctness of the format must be checked by the API
  - The web API must store the timestamp when the software was **first** reported. This data is not sent by the caller but must be maintained automatically by the API.
  - The web API must also store the timestamp when the software was **last** reported. This data is not sent by the caller but must be maintained automatically by the API.

### Security: Find computers with outdated software

In the domain _security_, the following APIs must be implemented:

- **Find installations of software**: The caller sends a software identifier and an optional version filter. The API returns a list of computers that have this software installed. The version filter can be:
  - A version number (e.g. _1.2.3_): The API returns all computers with this exact version installed.
  - a _caret version_ (e.g. _^1.2.3_): The API returns all computers that have a version >= the given version and < the next major version (e.g. _^1.2.3_ -> >= 1.2.3 and < 2.0.0).
  - a _tilde version_ (e.g. _~1.2.3_): The API returns all computers that have a version >= the given version and < the next minor version (e.g. _~1.2.3_ -> >= 1.2.3 and < 1.3.0).

## Non-Functional Requirements

### Data Access

Store all data in the file system. Create one file per computer. The file contains data about the computer and the installed software. The file name is the MAC address of the computer without the colon. The file format is JSON.

### Project and API Structure

It is part of the exercise to design the structure of the project and the API. Regarding API, you can align your work to the sample requests in [requests.http](./requests.http). You can deviate from the sample requests if you have good reasons for it.
