#Devices

This guide demonstrates how to ...

In this demonstration you will show how to

* observe telemetry and alarm history data from simulated devices,
* send commands to devices (cloud-to-device messaging),
* view device properties and command history stored in DocumentDB,
* configure device rules,
* add a new device to the solution

##Pre-requisites

This demonstration requires the following:

* Azure Subscription

##Setup

_Estimated Time: 10 minutes_

1. Open your browser to https://azureiotsuite.com.

2. Authenticate to Azure IoT Suite using your Azure subscription credentials.

3. Click **Create a new solution**.

4. Click **Select** for the **Remote Monitoring solution**.

    a. Set the **Solution name** to _iotsuite-remote-mon_ .

    b. Select a **Region** close to you.

    c. Select your **Azure subscription** to deploy the solutoin into.

    d. Click **Create solution**.

    <img src="./media/setup-01.png" style="max-width: 500px" />

##Demo Steps

_Estimated Time: 5 minutes_

1. Open your browser to https://azureiotsuite.com.

2. Authenticate to Azure IoT Suite using your Azure subscription credentials.

3. Click **Launch** for the remote monitoring solution deployed in the Setup section.

4. Show the Telemetry History for the _SampleDevice001_x_ device.  This telemetry data is coming from a device simulator.  The device simulator is a WebJob (C#) that generates random temperature and humidity data that is then sent to IoT Hub every 5 seconds.  The occasional spike in the temperature data is intentional and was programmed into the device to show how rules are used to invoke alarms (ie: when temperature exceeds a certain threshold).

    <img src="./media/demo-01.png" style="max-width: 500px" />

5.  Show the Alarm History for the _SampleDevice001_x_ device.  The Alarm History shown is a result of rules logic in the system.  When the temperature and humidity data for a device exceeds a specified threshold (the rule), an alarm is triggered.  

    <img src="./media/demo-02.png" style="max-width: 500px" />

6. Click **RULES** on the left navigation and show the two default rules for the _SampleDevice001_x_ device.  These are created by default to bootstrap the system with logic to generate some Alarm History as shown previously on the DASHBOARD page.

    <img src="./media/demo-03.png" style="max-width: 500px" />


### Send Commands to a Device

1. Click **DEVICES** on the left navigation.  The solution creates these four simulated devices when it is first initialized.  We will see how to add new devices to the system later.

2. Click on _SampleDevice001_x_.

3. Stop Telemetry

4. Change Device State

5. Ping

6. Start Telemetry


### View Device properties in DocumentDB

1. Open the Resource Group blade in the Azure portal for the remote monitoring solution.

2. Click on the **DocumentDB** resource to open the DocumentDB account blade.

3. Click **Document Explorer** in the toolbar.  

4. Click on the first document shown under the ID section.  

5. Show command history

6. Show Device State


### Add a new rule

1. Add a new rule for _SampleDevice002_x_.

    a. Set type to Humidity.

    b. Set Threshold to 40.

2. Show Telemetry and Alarm History


### Add a new Device

Show Simulated Device and Custom Device.


##Cleanup

_Estimated Time: 5 minutes_

1. Open the Resource Group blade in the Azure portal for the remote monitoring solution.

2. Click **DELETE** in the toolbar.
 
