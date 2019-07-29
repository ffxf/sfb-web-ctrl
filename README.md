# SimFeedback Web Controller

Web Controller and REST API for SFX-100 motion rig systems and the SimFeeback software

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

Open up a browser and point it to the IP-address enabled under Requirements and using the port number being changed or 
the defult of `8080`. You should see a screen like this here:

![SimFeedback Default Web-UI](https://raw.githubusercontent.com/ffxf/sfb-web-ctrl/master/media/SFB-WebCtrl-Browser-Off.PNG)

Turning on the `Start` checkbox will boot up the SFX100 systems after a few seconds and selecting `20` for the overall intensity setting would yield the following screen:

![SimFeedback Web-UI set to ON and intensity to 20](https://raw.githubusercontent.com/ffxf/sfb-web-ctrl/master/media/SFB-WebCtrl-Browser-On-Intense20.PNG)


### REST API Usage

For example us this command to query for the status of your SFX-100 system:

    curl -d "{}" -H "Content-Type: application/json" -X GET http://127.0.0.1:8080/status

This should yield a response like the following:
    {"isSFXOn":false,"isConnected":false,"intensityIncrement":0,"lastMessage":"trala"}

Or use

    curl -d "{}" -H "Content-Type: application/json" -X POST http://127.0.0.1:8080/start/1

to start your SFX-100 rig with a REST API request.

The full list of REST-API endpoints is:

- POST: http://your_ip:port/start/x - turn on/off SFX-100 with x=1/0
- POST: http://your_ip:port/enable/x - enable/disable all effects with x=1/0
- POST: http://your_ip:port/log/x/msg - Log a message to the SimFeedback log file with x as 0=info/1=debuf/2=error logging and msg as the message
- POST: http://your_ip:port/save - save the current profile configuration
- GET: http://your_ip:port/status - get status information from SimFeedback

You can also drop the option paramters from the end endpoints with options (start/enable/log) and use a JSON body for the request as follows:

    curl -d "{\"val\":1, \"message\":\"switching on\"}" -H "Content-Type: application/json" -X POST http://127.0.0.1:8080/start

# Knowns Issues

The enable/disable all effects endpoints and web-UI checkbox do not work currently because of a bug in SimFeedback.

Not much care has been taken to catch errors, e.g. when submitting malformatted REST requests.
