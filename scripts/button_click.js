// Need to save this globally as it is being set and used in differnt button handlers
var intensityIncrement = "1";

// SimfeedBack will take some time to connect/disconnect to/from the Arduino. During this
// time there can be status updates. So if we start/stop the SFX system there can be a delay
// between clicking those buttons and the API reflecting that status change. This would deliver
// confusing status displays. To avoid this we are presenting the status as clicked and give it
// up to 3 status updates to materialize. The two global variables below serve that purpose.
var sfbSetStatus = false; // false is off - true is on
var updatesSinceChange = 0;

// Generic part of the REST request response handlers
function HandleResponse(response) {
    if (response.status !== 200) {
        console.log('Looks like there was a problem. Status Code: ' +
            response.status);
        return;
    }

    // Examine the text in the response
    response.json().then(function (data) {
        console.log(data);
    });
}

// Sets the LEDs for the on/off status of Arduino and telemetry connections
// For the on/off of the Arduino this also modifies the corresponding checkbox setting and
// its appearance. This avoids the checkbox goes back to its previous state as represented by
// upates from the plugin while the connection to the Arduino is turned on or off.
function SetLed(elem_name, on) {
    var led_elem = document.getElementById(elem_name);
    var on_switch = document.getElementById("start_stop");
    
    if (on == true) {
        led_elem.setAttribute('class', 'green led');
        if (elem_name == "onLed") {
            on_switch.setAttribute('checked', 'checked'); //on_switch.checked = true;
            on_switch.style.setProperty("background-image", "url(/images/checked-shift.png)");
        }
    } else {
        led_elem.setAttribute('class', 'red led');
        if (elem_name == "onLed") {
            on_switch.removeAttribute('checked'); // on_switch.checked = false;
            on_switch.style.setProperty("background-image", "url(/images/unchecked-shift.png)");
        }
    }
}

// Web socket for receiving status updates (i.e. whether SFB is connected to the Arduino
// and whether it has telemetry data from the game)
function WebSocketStart() {
    var port = 8181; // default - should get reset via value of hidden 'web_sock_port' input element
    var str_port = document.getElementById("web_sock_port").value;
    console.log("got raw port value " + port);
    if (!isNaN(str_port))
        port = parseInt(str_port);
    console.log("got web socket port ", port);

    console.log(location.hostname);
    var socket = new WebSocket("ws://" + location.hostname + ":" + port + "/websock");
    console.log('connected');
    socket.onopen = function () {
        console.log('in onopen');
        return true;
    };

    // The only event we really handle thus far is receive connection status.
    // In response we set the leds correspodingly
    socket.onmessage = function (event) {
        console.log('in onmessage');
        var result = JSON.parse(event.data);
        console.log('event data parsed: ', result);

        updatesSinceChange++;
        
        // We switch the Arduino connection led either when what has been clicked is in
        // sync with what is reported from the extension plugin or if we have waited for
        // more than 3 status reports and still nothing has changed (which might get caused
        // by a problem SFB has to connect, for instance)
        if (sfbSetStatus == result.isRunning || updatesSinceChange > 3) {
            SetLed("onLed", result.isRunning);
            sfbSetStatus = result.isRunning;
            updatesSinceChange = 0;
        }
        // The telemetry connection led we just set based on the status from the plugin
        SetLed("connLed", result.isActivated);
    };

    socket.onerror = function () {
        window.alert('Error');
        return false;
    };

    socket.onclose = function () {
        window.alert('Disconnected');
        return false;
    };
}

// Handling the Checkbox clicks to turning SFX on/off, i.e. handling the Arduino connection
function StartStopAction() {
    var ck = 0;
    var on_switch = document.getElementById("start_stop");
    // capture whether on or off was checked and be sure the right checkbox background image is displayed
    if (on_switch.checked) {
        ck = 1;
        on_switch.style.setProperty("background-image", "url(/images/checked-shift.png)");
        sfbSetStatus = true;
    } else {
        ck = 0;
        on_switch.style.setProperty("background-image", "url(/images/unchecked-shift.png)");
        sfbSetStatus = false;
    };
    updatesSinceChange = 0; // starting a new cycle of 3 status updates for dis/connect to happen

    // Endpoint: http://ip_adr:port/start/1 for on and http://ip_adr:port/start/0 for off
    fetch("http://" + location.hostname + ":" + location.port + "/start/" + ck.toString(), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            function (response) {
                HandleResponse(response)
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

// Manages web-UI elements that allow to append log messages to the SimFeedback log file
function LogAction() {
    console.log("in log");
    var radio = document.querySelector('input[name="radio"]:checked').value.toString();
    // Endpoint: http://ip_adr:port/log - API message body contains severity and log message
    fetch("http://" + location.hostname + ":" + location.port + "/log", {
        method: 'POST',
        body: '{"val":' + radio + ', "message":"' + document.getElementById("log_input").value + '"}',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            function (response) {
                HandleResponse(response)
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

// Handling the checkbox for enable/disable all effects
function EnableDisableAction() {
    console.log("in enable-disable");
    var ck = 0;
    if (document.getElementById("enable_disable").checked) {
        ck = 1;
    } else {
        ck = 0;
    };

    // Endpoint: http://ip_adr:port/enable/1 for on and http://ip_adr:port/enable/0 for off
    fetch("http://" + location.hostname + ":" + location.port + "/enable/" + ck.toString(), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            function (response) {
                HandleResponse(response)
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

// Incrementing the overall intensity for the profile by a preselected increment amount
function IncrementAction() {
    console.log("in increment");
    // Endpoint: http://ip_adr:port/intensity/incr for up'ing by incr (incr being an integer)
    fetch("http://" + location.hostname + ":" + location.port + "/intensity/" + intensityIncrement, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            function (response) {
                HandleResponse(response)
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

// Decrementing the overall intensity for the profile by a preselected increment amount
function DecrementAction() {
    console.log("in decrement");

        // Endpoint: http://ip_adr:port/intensity/-incr for down'ing by incr (incr being an integer)
    fetch("http://" + location.hostname + ":" + location.port + "/intensity/-" + intensityIncrement, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            function (response) {
                HandleResponse(response)
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

// Action when the Save button is pressed - saves current profile
function SaveAction() {
    console.log("in save");

    // Endpoint: http://ip_adr:port/save
    fetch("http://" + location.hostname + ":" + location.port + "/save", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            function (response) {
                HandleResponse(response)
            }
        )
        .catch(function (err) {
            console.log('Fetch Error :-S', err);
        });
}

// Gets intensity increment/decrement value from pulldown, stores it and sets it on the
// increment/decrements buttons
function GetintensityIncrementValue() {
    console.log("in get intensity");
    var sel = document.getElementById("incr_val");
    intensityIncrement = sel.options[sel.selectedIndex].innerHTML;
    var add = document.getElementById("add");
    var subtract = document.getElementById("subtract");
    add.innerText = "+ " + intensityIncrement;
    subtract.innerText = "- " + intensityIncrement;
    console.log("Increment Text: " + intensityIncrement + " Value: " + sel.value);
}
