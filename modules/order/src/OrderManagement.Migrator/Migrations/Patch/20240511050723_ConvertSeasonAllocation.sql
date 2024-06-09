insert into SeasonAllocation (CreationTime,IsDeleted,Code,Title,SeasonId,[Year])
select distinct GETDATE(),0,ROW_NUMBER() over (order by id),DeliveryDateDescription,1,FORMAT(CreationTime,'yyyy','fa')  from CustomerOrder
where IsDeleted = 0 and isnull(DeliveryDateDescription,'')!=''


update CustomerOrder
set SeasonAllocationId = (select a.Id from SeasonAllocation a where a.Title = DeliveryDateDescription) 
where IsDeleted = 0 and isnull(DeliveryDateDescription,'')!=''