SELECT Tab1.DeviceID, Sum_EC, Count_EC, Sum_EC - Count_EC
FROM
(
	SELECT DeviceID, SUM(EventCount) AS Sum_EC
	FROM dbo.AvgReadings
	GROUP BY DeviceID
) Tab1
INNER JOIN
(
	SELECT DeviceID, COUNT(*) AS Count_EC
	FROM dbo.PassthroughReadings
	GROUP BY DeviceID
) Tab2
   ON (Tab2.DeviceID = Tab1.DeviceID)
ORDER BY 1


select * from AvgReadings
select * from PassthroughReadings

delete from AvgReadings
delete from PassthroughReadings