# Software Inventory

## Introduction

You are working in a company providing software for _Software Inventory Mangagement_. Software Inventory Management refers to the systematic process of tracking, managing, and optimizing an organization's software assets. This involves identifying all software applications installed on devices, monitoring their usage, ensuring compliance with licensing agreements, and assessing security risks.

You have to implement a web API acting as the basis for different business processes. The API should provide the following functionality:

- Manage Computers
  - Add a new computer
  - Mark a computer as decommissioned
  - Search computers by e.g. IP address, MAC address, hostname
- Manage Software
  - Add installed software to a computer
- Security
  - Find all computers with outdated software versions

On other team will implement a software agent that runs on every computer in an organization. The agent will regularly (scheduled, at idle times) collect information about the computer and installed software and send it to the API.

A third team will implement a web frontend for security engineers who use the API to search for affected computers in case of a security incident (e.g. known vulnerability in a certain software version).

## Domains

### Computer Management

In the domain computer management, the following APIs must be implemented:

- **Create or update a computer**: The agent sends data about a computer. If the computer already exists, update its properties. If the computer does not exist, create a new computer. The agent sends the following data:
  - MAC address (unique identifier for the computer, 6 bytes written in 6 groups of 2 hex digits, e.g. _00:1A:2B:3C:4D:5E_; correctness of the format must be checked by the API, mandatory)
  - IP address (does not need to be checked, mandatory)
  - Hostname (mandatory)
  - CPU (string, mandatory)
  - RAM in GB (0-10000, max. digits after the decimal point, mandatory)
  - Total disk size in GB (0-100000, max. 2 digits after the decimal point, mandatory)
  - OS (`Windows`, `Linux`, or `macOS`; mandatory)
  - The web API must store the timestamp when the computer was last updated. This data is not sent by the caller but must be maintained automatically by the API.
- **Mark decommised computers**: Iterates through all computers and marks computers as decommissioned if they have not sent data for more than 30 days. If a computer starts sending data again, the decommissioned flag must be removed.

### Software Management

In the domain software management, the following APIs must be implemented:

- **Add software to a computer**: The agent sends data about installed software. The data includes the MAC address of the computer. **All** installed software for a computer is sent in a single request. For each software, the following data is sent:
  - Identifier (string, unique identifier for the software, mandatory; e.g. GUID of the software component, installation folder, etc.)
  - Name (mandatory)
  - Vendor (optional)
  - Version (string, semantic version with three segments (major, minor, path), e.g. _1.15.3_; mandatory)
  - The web API must store the timestamp when the software was **first** reported. This data is not sent by the caller but must be maintained automatically by the API.
  - The web API must also store the timestamp when the software was **last** reported. This data is not sent by the caller but must be maintained automatically by the API.

### Security: Find computers with outdated software

In the domain security, the following APIs must be implemented:

The caller sends a software identifier and a version number. The API returns a list of computers that have this software installed with this version or an older version.

    * If the clients sends a single segment version number (e.g. _1_), the API must return all computers with major version <= the given major version version number.
    * If the clients sends a two segment version number (e.g. _1.15_), the API must return all computers with
        * major version = the given major version and minor version <= the given minor version,
        * or major version < the given major version.
    * If the clients sends a three segment version number (e.g. _1.15.3_), the API must return all computers with
        * major version = the given major version and minor version = the given minor version and patch version <= the given patch version,
        * or major version = the given major version and minor version < the given minor version,
        * or major version < the given major version.

The following table illustrates the reporting logic:

| Input version | Installed version | Returned |
| ------------- | ----------------- | -------- |
| 1             | 1.0.0             | Yes      |
| 1             | 1.0.1             | Yes      |
| 1             | 1.1.0             | Yes      |
| 1             | 2.0.0             | No       |
| 1.0           | 1.0.0             | Yes      |
| 1.0           | 1.0.1             | Yes      |
| 1.0           | 1.1.0             | No       |
| 1.0           | 2.0.0             | No       |
| 1.0.0         | 1.0.0             | Yes      |
| 1.0.0         | 1.0.1             | No       |
| 1.0.0         | 1.1.0             | No       |
| 1.0.0         | 2.0.0             | No       |
| 2.1.0         | 1.0.0             | Yes      |
| 2.1.0         | 1.0.1             | Yes      |
| 2.1.0         | 1.1.0             | Yes      |
| 2.1.0         | 2.0.0             | Yes      |
| 2.1.0         | 2.1.0             | Yes      |
| 2.1.0         | 2.1.1             | No       |
| 2.1.0         | 2.2.0             | No       |

## Data Access

Store all data in the file system. Create one file per computer. The file contains data about the computer and the installed software. The file name is the MAC address of the computer without the colon. The file format is JSON.
