# SimFeedback Web Controller

Web Controller and REST API for SFX-100 motion rig systems and the SimFeeback software.

See section [Usage][#usage] and inparticular [Web-UI Usage](#web-ui usage) for the Web Controller usage and [REST-API Usage](#rest-api usage) for the usage of the REST-API. Also see the section on [REST-API Definition](#rest-api definition) for more information on the REST-API.

## Requirements

Requires SimFeedback v0.9 or higher.

You also need to allow for web traffic access on your PC e.g. by using the following commands as administrator:

    netsh http add urlacl url="http://your_ip:8080/" user="Any"
    netsh http add urlacl url="http://your_ip:8181/" user="Any"

Replace `your_ip` with the IP-address of your PC or use `127.0.0.1` for the loopback address. If you want both, 
your IP address and the loopback address be accessible then you will need to enter the commands for both addresses.

Also make sure you modify the port addresses as needed.

Finally note that the `Any` may need to be changed in non-English locales. E.g. for a PC set to the German locale use `Jeder`.

## Installation

Check for packages under [Releases](https://github.com/ffxf/sfb-web-ctrl/releases) and unpack into `extension/sfbctrl` in the SimFeedback directory.

## Usage

Start SimFeedback and click on `Setup`. Enable `Autostart` and `Activated` for the Web Controller plugin as indicated in the following screen shot:

![Enable Autostart and Activated in SFB Setup](https://raw.githubusercontent.com/ffxf/sfb-web-ctrl/master/media/SFB-Setup-WebCtrl.PNG)

If necessary change the HTTP and Websocket port under `Extensions` as shown here:

![Modify HTTP and Websocket Port in SFB Extensions](https://raw.githubusercontent.com/ffxf/sfb-web-ctrl/master/media/SFB-Ext-WebCtrl.PNG)

### Web-UI Usage

Open up a browser and point it to the IP-address enabled under [Requirements](#requirements) and using the port number being changed or 
the defult of `8080`. You should see a screen like this here:

![SimFeedback Default Web-UI](https://raw.githubusercontent.com/ffxf/sfb-web-ctrl/master/media/SFB-WebCtrl-Browser-Off.PNG)

Turning on the `Start` checkbox will boot up the SFX100 systems after a few seconds and selecting `20` for the overall intensity setting would yield the following screen:

![SimFeedback Web-UI set to ON and intensity to 20](https://raw.githubusercontent.com/ffxf/sfb-web-ctrl/master/media/SFB-WebCtrl-Browser-On-Intense20.PNG)


### REST-API Usage

#### Programmatic Examples

The [examples](https://github.com/ffxf/sfb-web-ctrl/tree/master/examples) folder contains examples in several programming languages.

#### Command-Line API Usage

For example use this command to query for the status of your SFX-100 system:

    curl -d "{}" -H "Content-Type: application/json" -X GET http://127.0.0.1:8080/status

This should yield a response like the following:
    {"isSFXOn":false,"isConnected":false,"intensityIncrement":0,"lastMessage":"test"}

Or use

    curl -d "{}" -H "Content-Type: application/json" -X POST http://127.0.0.1:8080/start/1

to start your SFX-100 rig with a REST API request.

# REST-API Definition

## List of API Endpoints

| REST Op  | Endpoint                        | Description                       |
|:---------|:--------------------------------|:----------------------------------|
| POST     | http://your_ip:port/start/val   | turn SFX-100 on/off (val = 1/0)     |
| POST     | http://your_ip:port/enable/val  | enable/disable (vla = 1/0) all effects |
| POST     | http://your_ip:port/log/val/msg | Log message (msg) to SimFeedback log file with log level val 0=info/1=debuf/2=error |
| POST     | http://your_ip:port/save        | save the current profile configuration |
| GET      | http://your_ip:port/status      | get status information from SimFeedback |

## Body of POST Operations

If using the above full endpoints the body can kept empty. Even if there was a body the settings for `x` or `msg` in the above endpoints would override the body paramters.

Alternatively you can use the following shortened endpoint

- http://your_ip:port/start
- http://your_ip:port/enable
- http://your_ip:port/log

and supply an `application/json` formatted body like the following

    { "val":0, "message":"what I have to say" }

The value of `val` corresponds to the same in the full endpoints and `message` will only be relevant for the log-endpoint where it refers to corresponds to `msg` above. You still can set a value for message for the other endpoints and you will receive it back in the response.

An example for a call using a body is the following:

    curl -d "{\"val\":1, \"message\":\"switching on\"}" -H "Content-Type: application/json" -X POST http://127.0.0.1:8080/start

## Response Content

### POST Response

The POST overations will have the following application/json-formatted content

    { "success":sc, "val":val, "message":"msg" }

Where `suc` is either `true` or `false`, `val` is an integer and corresponds to what you have supplied in the POST call, and `msg` is the content of the message string you may have supplied with the POST call.

### GET Response

There is only one GET call currently which is for getting status information. Its response looks as follows

    {"isSFXOn":on, "isConnected":con, "intensityIncrement":val, "lastMessage": "msg"}

with `on` and `con` being booleans describing on/off and connected status, `val` being an integer indicating the last intensity increments that was used (with a default of 1), and `msg` being the content of the last message that was sent via a POST operation (with a default of the empty string).

# Knowns Issues

The enable/disable all effects endpoints and web-UI checkbox do not work currently because of a bug in SimFeedback.

Not much care has been taken to catch errors, e.g. when submitting malformatted REST requests.
