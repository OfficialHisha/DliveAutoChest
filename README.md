# DliveAutoChest
This program will listen to donations and will, according to configuration, add a percentage of the donation into the community chest of your channel

## Configuration
The configuration file contains various options that can be modified to serve the specific needs of a user.

### Default configuration
```json
{
  "displayName": "",
  "authToken": "",
  "percentage": 10
}
```
* The <i>displayName</i> property should contain the Dlive channel name of the channel to listen for donation events.
* The <i>authToken</i> property should contain the user token of the owner of the channel defined by the displayName property.
* The <i>percentage</i> property decides the percentage of donations that will be added back to the community chest.

### Advanced configuration
In addition to the default configuration, more advanced options are available.
These are enabled by replacing the <i>percentage</i> property with a <i>percentage<b>s</b></i> property and adjusting the individual percentages for the events you want.
If you want to disable an event, either leave out the event from the <i>percentages</i> property or set the value to 0.

An example of an advanced configuration is like so,
```json
{
  "displayName": "",
  "authToken": "",
  "percentages": {
    "subscription": 10,
    "ice_cream": 10,
    "diamond": 10,
    "ninjaghini": 10,
    "ninjet": 10
  }
}
```
<b>NOTE:</b> The <i>percentage</i> property can be applied to the advanced configuration as well, if it is present it will precede the advanced configuration and the <i>percentages</i> property will be ignored.
