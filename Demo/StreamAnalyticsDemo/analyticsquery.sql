select *
INTO bloboutput
from devicereadingshub;

SELECT 
    DateAdd(second,-5,System.TimeStamp) as WinStartTime, 
    system.TimeStamp as WinEndTime, 
    DeviceId, 
    Avg(Temperature) as AvgTemperature, 
    Count(*) as EventCount 
INTO powerbi
FROM devicereadingshub
GROUP BY TumblingWindow(second, 5), DeviceId;