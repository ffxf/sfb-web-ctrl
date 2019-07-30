#!/usr/bin/env python
# The above works on MacOS or Linux - change for Windows
  
# Allows you to turn your SFX-100 system on or off
# Usage: ./start_stop.py http://ip_addr:port on|off

# Requires 'requests' (https://3.python-requests.org/)
# Also assumes Python3

import requests
import sys

if len(sys.argv) != 3 or sys.argv[2] not in ["on", "off"]:
    print("Usage: ", sys.argv[0], 'http://ip_addr:port on|off', file=sys.stderr)
    exit(1)

end_point = sys.argv[1] + "/start/"
start = end_point + "1"
stop = end_point + "0"

if sys.argv[2] == "on":
    response = requests.post(start, data='{}')
else:
    response = requests.post(stop, data='{}')

if response.json()["success"] == False:
    print("Something went wrong.", file=sys.stderr)
    exit(2)
else:
    print("Your SFX-100 system got switched", sys.argv[2])
