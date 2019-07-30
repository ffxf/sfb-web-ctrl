#!/usr/bin/env python
# The above works on MacOS or Linux. May need to change for Windows

# Toggles the SFX-100 system from on to off or vice versa
# Usage: ./toggle.py http://ip_addr:portf

# Requires 'requests' (https://3.python-requests.org/)
# Also assumes Python3

import requests
import sys

if len(sys.argv) != 2:
    print("Usage: ", sys.argv[0], 'http://ip_addr:port', file=sys.stderr)
    exit(1)

status = sys.argv[1] + "/status"
end_point = sys.argv[1] + "/start/"
start = end_point + "1"
stop = end_point + "0"
    
response = requests.get(status, data='{}')

on = response.json()["isSFXOn"]
toggle = "off" if on else "on"

if on:
    response = requests.post(stop, data='{}')
else:
    response = requests.post(start, data='{}')

if response.json()["success"] == False:
    print("Something went wrong with request switch to", toggle, file=sys.stderr)
    exit(2)
else:
    print("Your SFX-100 system got switched", toggle)