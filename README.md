# SimFeedback Web Controller

Web Controller and REST API for SFX-100 motion rig systems and the SimFeeback software

## Requirements

Requires SimFeedback v0.9 or higher.

You also need to allow for web traffic access on your PC e.g. by using the following command as administrator:

    netsh http add urlacl url="http://your_ip:8080/" user="Any"
    netsh http add urlacl url="http://your_ip:8181/" user="Any"

Replace `your_ip` with the IP-address of your PC or use `127.0.0.1` for the loopback address. If you want both, 
your IP address and the loopback address be accessible then you will need to enter the commands for both addresses.

Also make sure you modify the port addresses as needed.

Finally note that the `Any` may need to be changed in non-English locales. E.g. for a PC set to the German locale use `Jeder`.

## Installation

Check for pagackages under [Releases](https://github.com/ffxf/sfb-web-ctrl/releases) and unpack into `extension/sfbctrl` in the SimFeedback directory.

## Usage

Start SimFeedback and click on `Setup`. Enable `Autostart` and `Activated` for the Web Controller plugin.

If necessary change to HTTP and Websocket port under `Extensions`.

### Web-UI Usage

Open up a browser and point it to the IP-address enabled under Requirements and using the port number being changed or 
the defult of `8080`.

### REST API Usage

For example us this command to query for the status of your SFX-100 system:

    curl -d "{}" -H "Content-Type: application/json" -X GET http://127.0.0.1:8080/status

This should yield a response like the following:
    {"isSFXOn":false,"isConnected":false,"intensityIncrement":0,"lastMessage":"trala"}

Or use

    curl -d "{}" -H "Content-Type: application/json" -X POST http://127.0.0.1:8080/start/1

to start your SFX-100 rig with a REST API request.
