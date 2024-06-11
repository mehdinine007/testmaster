insert into SeasonAllocation (CreationTime,IsDeleted,Code,Title,SeasonId,[Year])
select distinct GETDATE(),0,ROW_NUMBER() over (order by [year],DeliveryDateDescription),DeliveryDateDescription,1,[year] 
from (
select distinct DeliveryDateDescription,FORMAT(CreationTime,'yyyy','fa') as [year]  from CustomerOrder
where IsDeleted = 0 and isnull(DeliveryDateDescription,'')!=''
) as tb



update CustomerOrder
set SeasonAllocationId = (select a.Id from SeasonAllocation a where a.Title = DeliveryDateDescription and a.[Year] = FORMAT(CreationTime,'yyyy','fa')) 
where IsDeleted = 0 and isnull(DeliveryDateDescription,'')!=''